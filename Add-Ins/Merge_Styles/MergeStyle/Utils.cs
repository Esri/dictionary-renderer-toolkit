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
}
