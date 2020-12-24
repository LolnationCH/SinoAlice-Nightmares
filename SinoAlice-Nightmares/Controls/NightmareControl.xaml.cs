using SinoAlice_Nightmares.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace SinoAlice_Nightmares.Controls
{
  enum TimerState
  {
    Initial,
    Summoning,
    Summoned,
    Finished
  }

  public partial class NightmatreControl : UserControl
  {
    private NightmareInfo nightmareInfo;

    private TimerState state = TimerState.Initial;
    DispatcherTimer _timer;
    TimeSpan _time;

    public NightmatreControl(NightmareInfo nightmareInfo)
    {
      InitializeComponent();
      this.nightmareInfo = nightmareInfo;
      SetImagePath();

      ResetComponent();
    }

    public void ResetComponent()
    {
      state = TimerState.Initial;
      _time = TimeSpan.FromSeconds(nightmareInfo.Summoning_time);
      UpdateTimerLabel();
      SetTextState();
      Timer_btn.IsEnabled = true;
    }

    private void Timer_btn_Click(object sender, RoutedEventArgs e)
    {
      ((Button)sender).IsEnabled = false;

      this.state = TimerState.Summoning;
      SetTextState();
      StartTimer(nightmareInfo.Summoning_time);
    }

    private void SetImagePath()
    {
      this.Image.Source = new BitmapImage(new Uri(nightmareInfo.Image_path));
    }

    private void SetTextState()
    {
      switch (this.state)
      {
        case TimerState.Initial:
          this.State.Content = "Not Summoned";
          break;
        case TimerState.Summoning:
          this.State.Content = "Summoning";
          break;
        case TimerState.Summoned:
          this.State.Content = "Summoned";
          break;
        case TimerState.Finished:
          this.State.Content = "Finished";
          break;
      }
    }

    private void UpdateTimerLabel()
    {
      Timer.Content = "Time remaining : " + _time.ToString("c");
    }

    private void StartTimer(int interval)
    {
      _time = TimeSpan.FromSeconds(interval); 
      UpdateTimerLabel();

      _timer = new DispatcherTimer(new TimeSpan(0,0,1), DispatcherPriority.Normal, delegate
      {
        UpdateTimerLabel();
        if (_time == TimeSpan.Zero)
        {
          _timer.Stop();
          OnTimerEnd();
        }
        _time = _time.Add(TimeSpan.FromSeconds(-1));
      }, Application.Current.Dispatcher);

      _timer.Start();
    }

    private void OnTimerEnd()
    {
      int time = 0;
      switch (this.state)
      {
        case TimerState.Summoning:
          this.state = TimerState.Summoned;
          time = nightmareInfo.Summoned_time;
          break;
        case TimerState.Summoned:
          this.state = TimerState.Finished;
          this.Dispatcher.Invoke(() =>
          {
            Image.Opacity = 0.55;
          });
          break;
      }
      this.Dispatcher.Invoke(() =>
      {
        SetTextState();
      });
      if (time != 0)
        StartTimer(time);

    }
  }
}
