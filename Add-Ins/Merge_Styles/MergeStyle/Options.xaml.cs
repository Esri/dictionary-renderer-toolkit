using ArcGIS.Desktop.Framework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace DictionaryToolkit
{
  /// <summary>
  /// Interaction logic for Options.xaml
  /// </summary>
  public partial class Options
  {
    public Options(OptionsViewModel vm)
    {
      InitializeComponent();
      this.DataContext = vm;
    }
  }
}
