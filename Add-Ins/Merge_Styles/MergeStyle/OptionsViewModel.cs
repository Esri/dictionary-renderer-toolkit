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

using ArcGIS.Desktop.Core;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;
using ArcGIS.Desktop.Framework.Threading.Tasks;
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
    #region Dictionary path
    private string _dictionaryPath = ""; //"C:\\ArcGIS\\Resources\\Dictionaries\\app6b\\app6b.stylx";
    public string DictionaryPath
    {
      get { return _dictionaryPath; }
      set
      {
        if (SetProperty(ref _dictionaryPath, value, () => DictionaryPath))
        {
          NotifyPropertyChanged(() => IsMergeEnabled);
        }
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

#region Merge Style
    private string _styleToMerge = string.Empty;
    public string StyleToMerge
    {
      get { return _styleToMerge; }
      set
      {
        if (SetProperty(ref _styleToMerge, value, () => StyleToMerge))
        {
          NotifyPropertyChanged(() => IsMergeEnabled);
        }
      }
    }

    private bool _replaceKeys = false;
    public bool ReplaceKeys
    {
      get { return _replaceKeys; }
      set { SetProperty(ref _replaceKeys, value, () => ReplaceKeys); }
    }

    private ICommand _browseStyleCmd = null;
    public ICommand BrowseStyleCommand
    {
      get { return _browseStyleCmd ?? (_browseStyleCmd = new RelayCommand(() => BrowseStyle())); }
    }
    public void BrowseStyle()
    {
      BrowseItem(ItemFilters.styleFiles, (path) => StyleToMerge = path);
    }

    private ICommand _mergeStyleCmd = null;
    public ICommand MergeStyleCommand
    {
      get { return _mergeStyleCmd ?? (_mergeStyleCmd = new RelayCommand(() => MergeStyle())); }
    }
    public bool IsMergeEnabled
    {
      get
      {
        // Enable if Styles are set, are different and exist
        return !string.IsNullOrEmpty(_dictionaryPath) && !string.IsNullOrEmpty(_styleToMerge) && (_dictionaryPath != _styleToMerge) &&
            File.Exists(_dictionaryPath) && File.Exists(_styleToMerge);
      }
    }
    private async Task MergeStyle()
    {
      var result = ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(
          "Merging Style: \n" + _styleToMerge + "\n Into: \n" +
          _dictionaryPath,
          "Merge Style", MessageBoxButton.OKCancel, MessageBoxImage.Exclamation);

      if (result.ToString() != "OK")
        return;

      var styleToMerge = await StyleUtil.GetStyleItem(_styleToMerge);
      var style = await StyleUtil.GetStyleItem(_dictionaryPath);

      if (!StyleUtil.IsStyleEditable(style))
        return;

      var mergeStyle = new MergeStyle(style);
      await QueuedTask.Run(() =>
      {
        mergeStyle.Merge(styleToMerge, ReplaceKeys);
      });

      ArcGIS.Desktop.Framework.Dialogs.MessageBox.Show(
          "Merge Complete: " + style +
          "\nNumber of Symbols Added: " + mergeStyle.NumSymbolsAdded +
          "\nNumber of Symbols Ignored: " + mergeStyle.NumSymbolsNotAdded,
          "Merge Style", MessageBoxButton.OK, MessageBoxImage.Exclamation);
    }
#endregion

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
