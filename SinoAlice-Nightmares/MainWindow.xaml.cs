using SinoAlice_Nightmares.Controls;
using SinoAlice_Nightmares.Objects;
using System;
using System.Collections.Generic;
using System.IO;
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

    Settings _settings;

    public MainWindow()
    {
      InitializeComponent();

      // Set the window size and other things
      HandleSettings();    

      // Show the nightmares
      RefreshConfigList();
    }

    private void HandleSettings()
    {
      _settings = DataHandler.GetSettings();

      this.Width = _settings.Width;
      this.Height = _settings.Height;
    }

    private void CreateNightmaresControl(List<NightmareInfo> nightmareInfos)
    {
      nightmares.Clear();
      Nightmare_Stack.Children.Clear();
      StackPanel sp = new StackPanel();
      for (var i = 0; i < nightmareInfos.Count; i++)
      {
        var nightinfo = nightmareInfos[i];
        var nightControl = new NightmatreControl(nightinfo);
        if (i % _settings.NumColumns == 0)
        {
          if (i != 0) Nightmare_Stack.Children.Add(sp);
          sp = new StackPanel();
          sp.Orientation = Orientation.Horizontal;
          sp.Children.Add(nightControl);
        }
        else
        {
          nightControl.Margin = new Thickness(10, 0, 0, 0);
          sp.Children.Add(nightControl);
        }
        nightmares.Add(nightControl);
      }
      Nightmare_Stack.Children.Add(sp);
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

    private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
    {
      _settings.Width = this.Width;
      _settings.Height = this.Height;
      DataHandler.SetSettings(_settings);
    }
  }
}
