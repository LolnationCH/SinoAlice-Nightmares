using SinoAlice_Nightmares.Objects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SinoAlice_Nightmares.Controls
{
  /// <summary>
  /// Interaction logic for NightmareSelect.xaml
  /// </summary>
  public partial class NightmareSelect : UserControl
  {
    public bool IsSelected = false;
    public NightmareInfo nightmareInfo;

    public NightmareSelect(NightmareInfo nightmareInfo)
    {
      InitializeComponent();
      this.nightmareInfo = nightmareInfo;
      SetImagePath();

      ResetComponent();
    }

    public void ResetComponent()
    {
      Name_Label.Content = nightmareInfo.Name;
    }


    private void SetImagePath()
    {
      var path = System.IO.Path.GetFullPath(nightmareInfo.ImagePath);
      if (File.Exists(path))
        this.Image.Source = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
      else
      {
        using (var client = new WebClient())
        {
          client.DownloadFile(nightmareInfo.ImageUrl, nightmareInfo.ImagePath);
          this.Image.Source = new BitmapImage(new Uri(path, UriKind.RelativeOrAbsolute));
        }
      }
    }

    private void Select_btn_Click(object sender, RoutedEventArgs e)
    {
      if (IsSelected)
      {
        this.Select_border.BorderBrush = new SolidColorBrush(Colors.Transparent);
        IsSelected = false;
      }
      else
      {
        this.Select_border.BorderBrush = new SolidColorBrush(Colors.Black);
        IsSelected = true;
      }
    }

    public void MakeSelected()
    {
      this.Select_border.BorderBrush = new SolidColorBrush(Colors.Black);
      IsSelected = true;
    }
  }
}