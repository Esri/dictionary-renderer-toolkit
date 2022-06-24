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

using ArcGIS.Desktop.Mapping;
using System;
using System.Collections.Generic;

namespace DictionaryToolkit
{
  public class MergeStyle
  {
    private StyleProjectItem _style = null;
    private Action<string> _report = null;

    private int _numSymbolsAdded = 0;
    private int _numSymbolsNotAdded = 0;

    public MergeStyle(StyleProjectItem style, Action<string> report = null)
    {
      _style = style;
      _report = report;
    }

    public void Merge(StyleProjectItem styleToMerge, bool replaceKeys)
    {
      _numSymbolsAdded = 0;
      _numSymbolsNotAdded = 0;

      // point symbols
      IList<SymbolStyleItem> sourcePointSymbols = styleToMerge.SearchSymbols(StyleItemType.PointSymbol, string.Empty);
      foreach (var styleItem in sourcePointSymbols)
      {
        try
        {
          if (replaceKeys)
          {
            var item = _style.LookupItem(StyleItemType.PointSymbol, styleItem.Key);
            if (item != null)
              _style.RemoveItem(item);
          }
          _style.AddItem(styleItem);
          //System.Diagnostics.Debug.WriteLine("Merging item: " + styleItem.Name);
          _numSymbolsAdded++;
        }
        catch (Exception)
        {
          if (_report != null)
            _report("Could not add key " + styleItem.Key);
          _numSymbolsNotAdded++;
        }
      }

      // point symbols
      IList<SymbolStyleItem> sourceLineSymbols = styleToMerge.SearchSymbols(StyleItemType.LineSymbol, string.Empty);
      foreach (var styleItem in sourceLineSymbols)
      {
        try
        {
          if (replaceKeys)
          {
            var item = _style.LookupItem(StyleItemType.LineSymbol, styleItem.Key);
            if (item != null)
              _style.RemoveItem(item);
          }
          _style.AddItem(styleItem);
          //System.Diagnostics.Debug.WriteLine("Merging item: " + styleItem.Name);
          _numSymbolsAdded++;
        }
        catch (Exception)
        {
          if (_report != null)
            _report("Could not add key " + styleItem.Key);
          _numSymbolsNotAdded++;
        }
      }

      // point symbols
      IList<SymbolStyleItem> sourcePolygonSymbols = styleToMerge.SearchSymbols(StyleItemType.PolygonSymbol, string.Empty);
      foreach (var styleItem in sourcePolygonSymbols)
      {
        try
        {
          if (replaceKeys)
          {
            var item = _style.LookupItem(StyleItemType.PolygonSymbol, styleItem.Key);
            if (item != null)
              _style.RemoveItem(item);
          }
          _style.AddItem(styleItem);
          //System.Diagnostics.Debug.WriteLine("Merging item: " + styleItem.Name);
          _numSymbolsAdded++;
        }
        catch (Exception)
        {
          if (_report != null)
            _report("Could not add key " + styleItem.Key);
          _numSymbolsNotAdded++;
        }
      }
    }

    public int NumSymbolsAdded
    {
      get { return _numSymbolsAdded; }
    }

    public int NumSymbolsNotAdded
    {
      get { return _numSymbolsNotAdded; }
    }
  }

}
