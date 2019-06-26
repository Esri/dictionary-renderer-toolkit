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
using ArcGIS.Core.Geometry;
using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework.Threading.Tasks;
using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace DictionaryToolkit
{
  class StyleUtil
  {
    static public async Task<StyleProjectItem> GetStyleItem(string styleFileFullPath)
    {
      StyleProjectItem style = null;

      if (!File.Exists(styleFileFullPath))
      {
        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(
            "Style File does not exist: \n" + styleFileFullPath,
            "Style File Missing", MessageBoxButton.OK, MessageBoxImage.Exclamation);
      }
      else
      {
        if (Project.Current != null)
        {
          await QueuedTask.Run(() =>
          {
            //Get all styles in the project
            var styles = Project.Current.GetItems<StyleProjectItem>();

            //Get the style in the project
            style = styles.FirstOrDefault(x => x.Path == styleFileFullPath);

            if (style == null)
            {
              // add it, if it wasn't found
              Project.Current.AddStyle(styleFileFullPath);

              // then check again for style (just in case)
              styles = Project.Current.GetItems<StyleProjectItem>();
              style = styles.FirstOrDefault(x => x.Path == styleFileFullPath);
            }
          });
        }
      }

      return style;
    }

    static public async Task RemoveStyleItem(string styleFileFullPath)
    {
      await QueuedTask.Run(() =>
      {
        var styles = Project.Current.GetItems<StyleProjectItem>();

        //Get the style in the project
        StyleProjectItem style = styles.FirstOrDefault(x => x.Path == styleFileFullPath);

        if (style != null)
        {
          // remove it, if it was found
          Project.Current.RemoveStyle(styleFileFullPath);
        }
      });
    }

    static public Dictionary<string, StyleItem> ReadSymbolItems(StyleProjectItem style, StyleItemType itemType)
    {
      Dictionary<string, StyleItem> items = new Dictionary<string, StyleItem>();
      IList<SymbolStyleItem> pointItems = style.SearchSymbols(itemType, "");
      foreach (var item in pointItems)
      {
        if (!items.ContainsKey(item.Key))
          items.Add(item.Key, item);
      }
      return items;
    }

    static public bool IsStyleEditable(StyleProjectItem style)
    {
      if (style == null)
        return false;

      if (!style.IsCurrent)
      {
        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(
            "Style is not current",
            "Check Style", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        return false;
      }

      if (style.IsReadOnly)
      {
        ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(
            "Style is read only",
            "Check Style", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        return false;
      }

      return true;
    }

  }

  class ColorUtil
  {
    static public bool IsBlackColor(CIMColor color, bool close = true)
    {
      CIMRGBColor rgb = color as CIMRGBColor;
      if (rgb == null)
        return false;
      int max = close ? 45 : 0;
      return rgb.R <= max && rgb.G <= max && rgb.B <= max && rgb.Alpha == 100;
    }

    static public bool IsBlackLayer(CIMSymbolLayer layer)
    {
      if (layer is CIMSolidFill)
      {
        return IsBlackColor((layer as CIMSolidFill).Color);
      }
      if (layer is CIMSolidStroke)
      {
        return IsBlackColor((layer as CIMSolidStroke).Color);
      }
      return false;
    }

    static public bool IsWhiteColor(CIMColor color)
    {
      CIMRGBColor rgb = color as CIMRGBColor;
      if (rgb == null)
        return false;
      return rgb.R == 255 && rgb.G == 255 && rgb.B == 255 && rgb.Alpha == 100;
    }

    static public bool IsWhiteLayer(CIMSymbolLayer layer)
    {
      if (layer is CIMSolidFill)
      {
        return IsWhiteColor((layer as CIMSolidFill).Color);
      }
      if (layer is CIMSolidStroke)
      {
        return IsWhiteColor((layer as CIMSolidStroke).Color);
      }
      return false;
    }

    static public void CollectColors(CIMMultiLayerSymbol symbol, List<CIMColor> colors)
    {
      if (symbol == null)
        return;
      if (symbol.SymbolLayers == null)
        return;
      foreach (var layer in symbol.SymbolLayers)
      {
        if (layer is CIMVectorMarker)
        {
          var markerGraphics = (layer as CIMVectorMarker).MarkerGraphics;
          if (markerGraphics != null)
            foreach (var markerGraphic in markerGraphics)
            {
              if (markerGraphic.Symbol is CIMMultiLayerSymbol)
                CollectColors(markerGraphic.Symbol as CIMMultiLayerSymbol, colors);
              if (markerGraphic.Symbol is CIMTextSymbol)
                CollectColors((markerGraphic.Symbol as CIMTextSymbol).Symbol as CIMMultiLayerSymbol, colors);
            }
        }

        // mark the layer as fill or outline
        if (layer is CIMSolidFill)
        {
          if (!ContainsColor(colors, (layer as CIMSolidFill).Color))
            colors.Add((layer as CIMSolidFill).Color);
        }
        if (layer is CIMSolidStroke)
        {
          if (!ContainsColor(colors, (layer as CIMSolidStroke).Color))
            colors.Add((layer as CIMSolidStroke).Color);
        }
        if (layer is CIMHatchFill)
        {
          CollectColors((layer as CIMHatchFill).LineSymbol, colors);
        }
      }
    }

    static public bool ContainsColor(List<CIMColor> colors, CIMColor color)
    {
      var c1 = color as CIMRGBColor;
      if (c1 == null)
        return false;

      foreach (var c in colors)
      {
        var c2 = c as CIMRGBColor;
        if (c2 == null)
          continue;
        if (c1.R == c2.R && c1.G == c2.G && c1.B == c2.B && c1.Alpha == c2.Alpha)
          return true;
      }
      return false;
    }

    static public bool IsNeutralColor(CIMSymbolLayer layer)
    {
      if (layer is CIMSolidStroke solidStroke)
      {
        return IsNeutralColor(solidStroke.Color);
      }
      if (layer is CIMSolidFill solidFill)
      {
        return IsNeutralColor(solidFill.Color);
      }
      return false;
    }

    static public bool IsNeutralColor(CIMColor color)
    {
      CIMRGBColor rgb = color as CIMRGBColor;
      if (rgb == null)
        return false;
      // almost black
      if (rgb.R < 45 && rgb.G < 45 && rgb.B < 45 && rgb.Alpha == 100)
        return true;
      // or gray
      if (rgb.R < 128 && rgb.G < 128 && rgb.B < 128 && Math.Round(rgb.R) == Math.Round(rgb.G) && Math.Round(rgb.R) == Math.Round(rgb.B) && rgb.Alpha == 100)
        return true;
      return false;
    }

  }

  class SymbolUtil
  {
    static public bool HasFill(CIMMultiLayerSymbol symbol)
    {
      if (symbol == null)
        return false;
      if (symbol.SymbolLayers == null)
        return false;
      foreach (var layer in symbol.SymbolLayers)
      {
        if (layer is CIMFill)
          return true;
      }
      return false;
    }

    static public bool IsStroke(CIMMultiLayerSymbol symbol)
    {
      if (symbol == null)
        return false;
      if (symbol.SymbolLayers == null)
        return false;
      foreach (var layer in symbol.SymbolLayers)
      {
        if (layer is CIMStroke)
          return true;
        return false; // stroke only
      }
      return false;
    }

    static public void SetPrimitiveNames(CIMMultiLayerSymbol symbol, string frame_fill, string frame_outline, bool just_black, bool markers)
    {
      if (symbol == null)
        return;

      // check if we can remove some layers
      // TODO

      if (symbol.SymbolLayers == null)
        return;
      foreach (var layer in symbol.SymbolLayers)
      {
        if (markers && layer is CIMVectorMarker)
        {
          var markerGraphics = (layer as CIMVectorMarker).MarkerGraphics;
          if (markerGraphics != null)
            foreach (var markerGraphic in markerGraphics)
            {
              if (markerGraphic.Symbol is CIMMultiLayerSymbol)
                SetPrimitiveNames(markerGraphic.Symbol as CIMMultiLayerSymbol, frame_fill, frame_outline, just_black, markers);
            }
        }

        // mark the layer as fill or outline
        if (layer is CIMFill && frame_fill != null)
        {
          if (!just_black || ColorUtil.IsNeutralColor(layer))
            layer.PrimitiveName = frame_fill;
          if (layer is CIMHatchFill hatchFill)
            SetPrimitiveNames(hatchFill.LineSymbol, frame_fill, frame_outline, just_black, markers);
        }
        if (layer is CIMStroke && frame_outline != null)
        {
          if (!just_black || ColorUtil.IsNeutralColor(layer))
            layer.PrimitiveName = frame_outline;
        }
      }
    }

    static public void LockAllLayers(CIMMultiLayerSymbol symbol)
    {
      if (symbol == null)
        return;
      if (symbol.SymbolLayers == null)
        return;
      foreach (var layer in symbol.SymbolLayers)
      {
        if (layer == null)
          continue;
        layer.ColorLocked = true;
      }
    }

  }

}
