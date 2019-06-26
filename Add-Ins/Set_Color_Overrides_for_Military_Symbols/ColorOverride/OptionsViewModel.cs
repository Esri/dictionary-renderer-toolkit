/*
Copyright 2019 Esri
Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at
    http://www.apache.org/licenses/LICENSE-2.0
Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.​
*/

using ArcGIS.Core.CIM;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Input;
using System.Windows;
using ArcGIS.Desktop.Catalog;
using System.Threading.Tasks;

namespace DictionaryToolkit
{
  public class OptionsViewModel : ViewModelBase
  {
    private int _updatedSymbols = 0;

    #region Dictionary path
    private string _dictionaryPath = ""; //"C:\\ArcGIS\\Resources\\Dictionaries\\app6b\\app6b.stylx";
    public string DictionaryPath
    {
      get { return _dictionaryPath; }
      set
      {
        SetProperty(ref _dictionaryPath, value, () => DictionaryPath);
      }
    }

    private ICommand _browseDictionaryCmd = null;
    public ICommand BrowseDictionaryCommand
    {
      get { return _browseDictionaryCmd ?? (_browseDictionaryCmd = new RelayCommand(() => BrowseDictionary())); }
    }
    private void BrowseDictionary()
    {
      BrowseItem(ItemFilters.styleFiles, (path) => DictionaryPath = path);
    }
    #endregion

    private ICommand _updateColorLockingCmd = null;
    public ICommand UpdateColorLockingCommand
    {
      get { return _updateColorLockingCmd ?? (_updateColorLockingCmd = new RelayCommand(() => this.UpdateColorLocking())); }
    }

    private async Task UpdateColorLocking()
    {
      StyleProjectItem style = await StyleUtil.GetStyleItem(_dictionaryPath);
      if (!StyleUtil.IsStyleEditable(style))
        return;

      await _UpdateColorLocking(style);
    }

    private async Task _UpdateColorLocking(StyleProjectItem style)
    {
      _updatedSymbols = 0;

      await QueuedTask.Run(() =>
      {
        UpdateColorLocking(style, StyleItemType.PointSymbol);
        UpdateColorLocking(style, StyleItemType.LineSymbol);
        UpdateColorLocking(style, StyleItemType.PolygonSymbol);
      });

      ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(
          "Merge Complete: " + style +
          "\nNumber of Symbols Updated: " + _updatedSymbols,
          "Set Color Override", MessageBoxButton.OK, MessageBoxImage.Exclamation);
    }

    private void UpdateColorLocking(StyleProjectItem style, StyleItemType type)
    {
      IList<SymbolStyleItem> items = style.SearchSymbols(type, String.Empty);
      foreach (var item in items)
      {
        var symbol = item.GetObject() as CIMMultiLayerSymbol;
        if (symbol == null)
          continue;

        if (item.Category.IndexOf("Frame") == 0)
        {
          if (symbol is CIMPointSymbol)
            SetFramePrimitiveNames(symbol as CIMPointSymbol, item.Key);
        }
        else if (item.Category.IndexOf("Main Icon") != -1 || item.Category.IndexOf("Modifier 1") != -1 || item.Category.IndexOf("Modifier 2") != -1)
        {
          SetIconPrimitiveNames(symbol as CIMPointSymbol, true);
        }

        // update the symbol
        item.SetObject(symbol);
        style.UpdateItem(item);
      }
    }

    private void SetFramePrimitiveNames(CIMPointSymbol symbol, string key)
    {
      if (symbol == null)
        return;

      // verify that the point symbol has only one vector marker layer

      if (symbol.SymbolLayers == null)
        return;
      if (symbol.SymbolLayers.Length != 1)
        return;
      CIMVectorMarker marker = symbol.SymbolLayers[0] as CIMVectorMarker;
      if (marker == null)
        return;
      if (marker.MarkerGraphics == null)
        return;

      // find the fill marker
      CIMMarkerGraphic fill = null;
      CIMMarkerGraphic outline = null;
      double maxArea = 0;
      double maxLength = 0;
      foreach (var graphic in marker.MarkerGraphics)
      {
        if (graphic == null)
          continue;
        var hasFill = SymbolUtil.HasFill(graphic.Symbol as CIMMultiLayerSymbol);
        var hasStroke = SymbolUtil.IsStroke(graphic.Symbol as CIMMultiLayerSymbol);
        if (graphic.Geometry is ArcGIS.Core.Geometry.Multipart geom)
        {
          if (hasFill)
          {
            double area = Math.Abs(geom.Area);
            if (area > maxArea)
            {
              maxArea = area;
              fill = graphic;
            }
          }
          if (hasStroke)
          {
            double length = Math.Abs(geom.Length);
            if (length > maxLength)
            {
              maxLength = length;
              outline = graphic;
            }
          }
        }
      }

      // add the primitive names
      foreach (var graphic in marker.MarkerGraphics)
      {
        if (graphic == fill)
          SymbolUtil.SetPrimitiveNames(graphic.Symbol as CIMMultiLayerSymbol, "frame_fill", "frame_outline", false, false);
        else if (graphic == outline)
          SymbolUtil.SetPrimitiveNames(graphic.Symbol as CIMMultiLayerSymbol, "frame_outline", "frame_outline", false, false);
        else
          SymbolUtil.SetPrimitiveNames(graphic.Symbol as CIMMultiLayerSymbol, "frame_outline", "frame_outline", false, false);
      }

      ++_updatedSymbols;
    }

    private void SetIconPrimitiveNames(CIMPointSymbol symbol, bool useWhite)
    {
      List<CIMColor> colors = new List<CIMColor>();
      ColorUtil.CollectColors(symbol, colors);

      if (colors.Count == 1)
      {
        var color = colors[0];
        bool colorIt = !useWhite || ColorUtil.IsBlackColor(color);
        if (colorIt)
        {
          UnlockAndSetPrimitiveName(symbol, (layer) => !useWhite || ColorUtil.IsBlackLayer(layer), "icon_element");
        }
        else
        {
          SymbolUtil.LockAllLayers(symbol); // all locked - no color primitive name
        }
      }
      else if (colors.Count == 2)
      {
        var color1 = colors[0];
        var color2 = colors[1];
        bool colorIt = useWhite ?
          ((ColorUtil.IsBlackColor(color1) && ColorUtil.IsWhiteColor(color2)) || (ColorUtil.IsBlackColor(color2) && ColorUtil.IsWhiteColor(color1))) :
          ((ColorUtil.IsBlackColor(color1) && !ColorUtil.IsBlackColor(color2)) || (ColorUtil.IsBlackColor(color2) && !ColorUtil.IsBlackColor(color1)));
        if (colorIt)
        {
          UnlockAndSetPrimitiveName(symbol, (layer) => !ColorUtil.IsBlackLayer(layer), "icon_element");
        }
        else
        {
          SymbolUtil.LockAllLayers(symbol);
        }
      }
      else
      {
        // all locked
        SymbolUtil.LockAllLayers(symbol);
      }

      ++_updatedSymbols;
    }

    private void UnlockAndSetPrimitiveName(CIMMultiLayerSymbol symbol, Func<CIMSymbolLayer, bool> UnlockIt, string name)
    {
      if (symbol == null)
        return;
      if (symbol.SymbolLayers == null)
        return;
      foreach (var layer in symbol.SymbolLayers)
      {
        layer.ColorLocked = false;

        if (layer is CIMVectorMarker)
        {
          var markerGraphics = (layer as CIMVectorMarker).MarkerGraphics;
          if (markerGraphics != null)
            foreach (var markerGraphic in markerGraphics)
            {
              if (markerGraphic.Symbol is CIMMultiLayerSymbol)
                UnlockAndSetPrimitiveName(markerGraphic.Symbol as CIMMultiLayerSymbol, UnlockIt, name);
              if (markerGraphic.Symbol is CIMTextSymbol)
              {
                // we don't set the primitive name on the internal polygon symbol, but rather on the text marker graphic
                // still, we want the color locking to be set
                UnlockAndSetPrimitiveName((markerGraphic.Symbol as CIMTextSymbol).Symbol as CIMMultiLayerSymbol, UnlockIt, "");
                markerGraphic.PrimitiveName = name;
              }
            }
        }

        // mark the layer as fill or outline
        if (layer is CIMFill || layer is CIMStroke)
        {
          if (UnlockIt(layer))
            layer.PrimitiveName = name;
          else
            layer.ColorLocked = true;
          if (layer is CIMHatchFill hatchFill)
            UnlockAndSetPrimitiveName(hatchFill.LineSymbol, UnlockIt, name);
        }
      }
    }

    private bool BrowseItem(string itemFilter, Action<string> setPath, string initialPath = "")
    {
      if (string.IsNullOrEmpty(initialPath))
      {
        var myDocs = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        initialPath = Path.Combine(System.IO.Path.Combine(myDocs, "ArcGIS"));
      }

      OpenItemDialog pathDialog = new OpenItemDialog()
      {
        Title = "Select Folder",
        InitialLocation = initialPath,
        MultiSelect = false,
        Filter = itemFilter,
      };

      bool? ok = pathDialog.ShowDialog();
      if ((ok == true) && (pathDialog.Items.Count() > 0))
      {
        IEnumerable<Item> selectedItems = pathDialog.Items;
        setPath(selectedItems.First().Path);
        return true;
      }

      return false;
    }
  }
}
