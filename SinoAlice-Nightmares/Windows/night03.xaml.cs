using SinoAlice_Nightmares.Controls;
using SinoAlice_Nightmares.Objects;
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

namespace SinoAlice_Nightmares.Windows
{
  /// <summary>
  /// Interaction logic for night03.xaml
  /// </summary>
  public partial class night03 : Window
  {
    List<NightmareInfo> _nightmareInfos;
    List<NightmareInfo> _curNightmareInfos = new List<NightmareInfo>();
    public List<NightmareInfo> SelectedNightmareInfo = new List<NightmareInfo>();

    public night03(List<NightmareInfo> excludedNightmareInfos)
    {
      InitializeComponent();
      _nightmareInfos = DataHandler.FetchNightmareInfos();
      RemoveDuplicates(excludedNightmareInfos);
      FillNightmare(_nightmareInfos);
    }

    private void RemoveDuplicates(List<NightmareInfo> excludedNightmareInfos)
    {
      foreach(var x in excludedNightmareInfos)
      {
        NightmareInfo nightmareInfo = null;
        foreach(var y in _nightmareInfos)
        {
          if (x.Name == y.Name)
          {
            nightmareInfo = y;
            break;
          }
        }
        if (nightmareInfo != null)
          _nightmareInfos.Remove(nightmareInfo);
      }
    }

    private void Add_btn_Click(object sender, RoutedEventArgs e)
    {
      foreach (var x in Nightmare_Stack.Children)
      {
        if (x is NightmareSelect)
        {
          var con = x as NightmareSelect;
          if (con.IsSelected)
            SelectedNightmareInfo.Add(con.nightmareInfo);
        }
      }
      if (SelectedNightmareInfo.Count == 0)
        MessageBox.Show("Please select at least one nightmare.");
      else
        Close();
    }

    private void FillNightmare(List<NightmareInfo> nightmareInfos)
    {
      Nightmare_Stack.Children.Clear();
      foreach (var x in nightmareInfos)
      {
        var nightSelect = new NightmareSelect(x);
        nightSelect.Margin = new Thickness(0, 0, 0, 5);
        Nightmare_Stack.Children.Add(nightSelect);
      }
    }

    private void Cancel_btn_Click(object sender, RoutedEventArgs e)
    {
      SelectedNightmareInfo.Clear();
      Close();
    }

    private void search_btn_Click(object sender, RoutedEventArgs e)
    {
      var text = ((TextBox)search_box).Text;
      _curNightmareInfos.Clear();
      foreach (var x in _nightmareInfos)
      {
        if (x.Name.ToLower().Contains(text.ToLower()))
          _curNightmareInfos.Add(x);
      }
      FillNightmare(_curNightmareInfos);
    }
  }
}
