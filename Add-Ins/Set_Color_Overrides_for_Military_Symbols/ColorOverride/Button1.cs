using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ArcGIS.Desktop.Framework;
using ArcGIS.Desktop.Framework.Contracts;

namespace DictionaryToolkit
{
  internal class Button1 : Button
  {
    private OptionsViewModel _vm = new OptionsViewModel();

    protected override void OnClick()
    {
      Options dlg = new Options(_vm);
      dlg.Owner = System.Windows.Application.Current.MainWindow;
      var ok = dlg.ShowDialog();
    }
  }
}
