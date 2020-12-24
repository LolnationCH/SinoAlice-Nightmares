using Newtonsoft.Json;
using SinoAlice_Nightmares.Controls;
using SinoAlice_Nightmares.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace SinoAlice_Nightmares
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    List<NightmatreControl> nightmares = new List<NightmatreControl>();
    String _configPath = Directory.GetCurrentDirectory() + "\\Configs\\";

    public MainWindow()
    {
      InitializeComponent();

      RefreshConfigList();
    }

    private void CreateNightmaresControl(List<NightmareInfo> nightmareInfos)
    {
      nightmares.Clear();
      Nightmare_Stack.Children.Clear();
      foreach (var nightinfo in nightmareInfos)
      {
        var nightControl = new NightmatreControl(nightinfo);
        nightmares.Add(nightControl);
        Nightmare_Stack.Children.Add(nightControl);
      }
    }

    private void RefreshConfigList()
    {
      var files = Directory.GetFiles(_configPath);
      List<string> configs = new List<string>();
      foreach (var f in files)
      {
        configs.Add(f.Replace(_configPath, "").Replace(".json", ""));
      }
      nightmare_combobox.ItemsSource = configs;
      if (configs.Count >= 1)
        nightmare_combobox.SelectedIndex = 0;
    }

    private void refresh_btn_Click(object sender, RoutedEventArgs e)
    {
      RefreshConfigList();
    }

    private void nightmare_combobox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      var selectedItem = (string)((ComboBox)sender).SelectedItem;
      var nightmareInfo = DataHandler.ReadNightmareInfosFromFile($"{_configPath}{selectedItem}.json");
      CreateNightmaresControl(nightmareInfo);
    }

    private void reset_btn_Click(object sender, RoutedEventArgs e)
    {
      foreach (var night in nightmares)
      {
        night.ResetComponent();
      }
    }
  }
}
