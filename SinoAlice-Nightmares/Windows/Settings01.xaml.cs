using SinoAlice_Nightmares.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SinoAlice_Nightmares.Windows
{
  /// <summary>
  /// Interaction logic for Settings01.xaml
  /// </summary>
  public partial class Settings01 : Window
  {
    public Settings settings;
    private static readonly Regex _regex = new Regex("[^0-9.-]+");

    public Settings01(Settings settings)
    {
      InitializeComponent();

      this.settings = settings;
      setFields();
    }

    private void setFields()
    {
      num_columns.Text = settings.NumColumns.ToString();
    }

    private void columns_TextChanged(object sender, TextChangedEventArgs e)
    {
      var text = ((TextBox)sender).Text.ToString();
      e.Handled = !_regex.IsMatch(text);        
    }

    private bool SaveSettings()
    {
      bool hasError = false;
      int num_col;
      if (!Int32.TryParse(num_columns.Text, out num_col))
      {
        MessageBox.Show("Couldn't parse the number of columns");
        hasError = true;
      }
      else
        settings.NumColumns = num_col;

      return hasError;
    }

    private void save_Click(object sender, RoutedEventArgs e)
    {
      if (!SaveSettings())
        Close();
    }
  }
}
