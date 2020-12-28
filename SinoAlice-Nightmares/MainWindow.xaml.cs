using SinoAlice_Nightmares.Controls;
using SinoAlice_Nightmares.Objects;
using SinoAlice_Nightmares.Windows;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SinoAlice_Nightmares
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    const string _Title = "night01";
    List<NightmareControl> nightmaresControl = new List<NightmareControl>();
    List<NightmareInfo> nightmaresInfo = new List<NightmareInfo>();

    String _configPath = Directory.GetCurrentDirectory() + "\\Configs\\";

    Settings _settings;

    public MainWindow()
    {
      InitializeComponent();

      _settings = DataHandler.GetSettings();

      // Set the window size and other things
      HandleSettings();

      // Show the nightmares
      RefreshConfigList();
    }

    private void HandleSettings()
    {
      this.Width = _settings.Width;
      this.Height = _settings.Height;
    }

    private void CreateNightmaresControl(List<NightmareInfo> nightmareInfos)
    {
      nightmaresControl.Clear();
      Nightmare_Stack.Children.Clear();
      StackPanel sp = new StackPanel();
      for (var i = 0; i < nightmareInfos.Count; i++)
      {
        var nightinfo = nightmareInfos[i];
        var nightControl = new NightmareControl(nightinfo);
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
        nightmaresControl.Add(nightControl);
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
      if (((ComboBox)sender).SelectedItem != null)
        RefreshView();
    }

    private void reset_btn_Click(object sender, RoutedEventArgs e)
    {
      foreach (var night in nightmaresControl)
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

    private void SavingConfiguration(List<NightmareInfo> nightmareInfos, string configName)
    {
      if (nightmareInfos.Count != 0)
      {
        var configNameS = configName;
        if (string.IsNullOrWhiteSpace(configName))
          configNameS = DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss");
        DataHandler.SaveNightmareInfoToFile(nightmareInfos, $"{_configPath}{configNameS}.json");
        nightmaresInfo = nightmareInfos;
        RefreshConfigList();
        nightmare_combobox.SelectedItem = configNameS;
        RefreshView();
      }
    }

    private void Exit_Click(object sender, RoutedEventArgs e)
    {
      System.Windows.Application.Current.Shutdown();
    }

    private void RefreshView()
    {
      var selectedItem = this.nightmare_combobox.SelectedItem;
      var nightmaresBase = DataHandler.ReadNightmareInfosFromFile($"{_configPath}{selectedItem}.json");
      nightmaresInfo = DataHandler.FetchNightmareInfos(nightmaresBase);
      CreateNightmaresControl(nightmaresInfo);
      this.Title = $"{_Title} - {selectedItem}";
    }

    private void View_Click(object sender, RoutedEventArgs e)
    {
      var window = new Settings01(_settings);
      window.ShowDialog();
      _settings = window.settings;

      HandleSettings();
      RefreshView();
    }

    private void Delete_Click(object sender, RoutedEventArgs e)
    {
      var selectedItem = this.nightmare_combobox.SelectedItem;
      File.Delete($"{_configPath}{selectedItem}.json");
      RefreshConfigList();
    }

    private void NewCommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
      var window = new night02(new List<NightmareInfo>(), "");
      window.ShowDialog();
      SavingConfiguration(window.NightmareInfos, window.ConfigName);
    }

    private void ModifyCommandBinding_Executed(object sender, System.Windows.Input.ExecutedRoutedEventArgs e)
    {
      var window = new night02(nightmaresInfo.ToList(), this.nightmare_combobox.SelectedItem as string);
      window.ShowDialog();
      SavingConfiguration(window.NightmareInfos, window.ConfigName);
    }
  }
}
