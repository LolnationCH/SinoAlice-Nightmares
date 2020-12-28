using SinoAlice_Nightmares.Controls;
using SinoAlice_Nightmares.Objects;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace SinoAlice_Nightmares.Windows
{
  /// <summary>
  /// Interaction logic for night02.xaml
  /// </summary>
  public partial class night02 : Window
  {
    List<NightmareInfo> _nightmareInfos;
    string _configName;

    public List<NightmareInfo> NightmareInfos { get => _nightmareInfos; }
    public string ConfigName { get => _configName; }

    public night02(List<NightmareInfo> nigthmareInfos, string configName)
    {
      InitializeComponent();
      _nightmareInfos = nigthmareInfos;
      _configName = configName;
      config_box.Text = configName;

      FillNightmare(_nightmareInfos);
    }

    private void Add_btn_Click(object sender, RoutedEventArgs e)
    {
      var window = new night03(_nightmareInfos);
      window.ShowDialog();
      if (window.SelectedNightmareInfo.Count != 0)
      {
        _nightmareInfos.AddRange(window.SelectedNightmareInfo);
        FillNightmare(_nightmareInfos);
      }
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

    private void Save_btn_Click(object sender, RoutedEventArgs e)
    {
      _configName = config_box.Text;
      Close();
    }

    private void Cancel_btn_Click(object sender, RoutedEventArgs e)
    {
      _nightmareInfos.Clear();
      Close();
    }

    private Dictionary<int, NightmareInfo> GetSelectedNightmare()
    {
      Dictionary<int, NightmareInfo> SelectedNightmareInfo = new Dictionary<int, NightmareInfo>();
      foreach (var x in Nightmare_Stack.Children)
      {
        int index = Nightmare_Stack.Children.IndexOf(x as UIElement);
        if (x is NightmareSelect)
        {
          var con = x as NightmareSelect;
          if (con.IsSelected)
            SelectedNightmareInfo.Add(index, con.nightmareInfo);
        }
      }
      return SelectedNightmareInfo;
    }

    private void ChangePosition(int up, Dictionary<int, NightmareInfo> selectedNight)
    {
      var selectedNightList = up == 1 ? selectedNight.Reverse() : selectedNight;
      var newIndexes = new List<int>();
      foreach (var x in selectedNightList)
      {
        if (x.Key == 0 && up == -1)
          continue;
        else if (x.Key == _nightmareInfos.Count-1 && up == 1)
          continue;
        _nightmareInfos.RemoveAt(x.Key);
        _nightmareInfos.Insert(x.Key+up, x.Value);
        newIndexes.Add(_nightmareInfos.IndexOf(x.Value));
      }
      FillNightmare(_nightmareInfos);
      foreach (var x in newIndexes)
        ((NightmareSelect)Nightmare_Stack.Children[x]).MakeSelected();
    }

    private void MoveUp_btn_Click(object sender, RoutedEventArgs e)
    {
      var selected = GetSelectedNightmare();
      ChangePosition(-1, selected);
    }

    private void MoveDown_btn_Click(object sender, RoutedEventArgs e)
    {
      var selected = GetSelectedNightmare();
      ChangePosition(1, selected);
    }

    private void Remove_btn_Click(object sender, RoutedEventArgs e)
    {
      var selected = GetSelectedNightmare();
      foreach (var x in selected)
      {
        _nightmareInfos.RemoveAt(x.Key);
      }
      FillNightmare(_nightmareInfos);
    }
  }
}
