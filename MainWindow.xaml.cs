using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace StoperDesktop
{
    public partial class MainWindow : Window
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        private readonly DispatcherTimer _timer = new DispatcherTimer();
        private readonly ObservableCollection<LapItem> _laps = new ObservableCollection<LapItem>();
        private TimeSpan _lastLapTime = TimeSpan.Zero;

        private static readonly Brush GreenBrush = new SolidColorBrush(Color.FromRgb(16, 185, 129));
        private static readonly Brush RedBrush = new SolidColorBrush(Color.FromRgb(244, 63, 94));
        private static readonly Brush WhiteBrush = new SolidColorBrush(Color.FromRgb(248, 250, 252));
        private static readonly Brush SkyBrush = new SolidColorBrush(Color.FromRgb(56, 189, 248));

        public MainWindow()
        {
            InitializeComponent();

            _timer.Interval = TimeSpan.FromMilliseconds(15);
            _timer.Tick += Timer_Tick;

            LapsItemsControl.ItemsSource = _laps;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            UpdateDisplay();
        }

        private void UpdateDisplay()
        {
            TimeSpan ts = _stopwatch.Elapsed;
            TimerDisplay.Text = $"{ts.Hours:00}:{ts.Minutes:00}:{ts.Seconds:00}.{(ts.Milliseconds / 10):00}";
        }

        private void StartStopBtn_Click(object sender, RoutedEventArgs e)
        {
            ToggleStartStop();
        }

        private void ToggleStartStop()
        {
            if (!_stopwatch.IsRunning)
            {
                // Start / Resume
                _stopwatch.Start();
                _timer.Start();

                StartStopBtn.Content = "Pauza";
                StartStopBtn.Background = RedBrush;
                LapBtn.IsEnabled = true;
                ResetBtn.IsEnabled = true;

                StatusTextBlock.Text = "Mierzenie czasu w toku";
                StatusTextBlock.Foreground = GreenBrush;
            }
            else
            {
                // Pause
                _stopwatch.Stop();
                _timer.Stop();
                UpdateDisplay();

                StartStopBtn.Content = "Wznów";
                StartStopBtn.Background = GreenBrush;
                LapBtn.IsEnabled = false;

                StatusTextBlock.Text = "Wstrzymano";
                StatusTextBlock.Foreground = SkyBrush;
            }
        }

        private void LapBtn_Click(object sender, RoutedEventArgs e)
        {
            AddLap();
        }

        private void AddLap()
        {
            if (!_stopwatch.IsRunning) return;

            TimeSpan current = _stopwatch.Elapsed;
            TimeSpan split = current - _lastLapTime;
            _lastLapTime = current;

            var item = new LapItem
            {
                LapNumber = $"#{_laps.Count + 1}",
                SplitTime = split,
                TotalTime = current,
                ColorBrush = WhiteBrush,
                Tag = string.Empty
            };

            _laps.Insert(0, item);
            RecalculateLapHighlights();

            LapCountText.Text = $"Okrążeń: {_laps.Count}";
        }

        private void RecalculateLapHighlights()
        {
            if (_laps.Count < 2)
            {
                if (_laps.Count == 1)
                {
                    _laps[0].ColorBrush = WhiteBrush;
                    _laps[0].Tag = string.Empty;
                }
                return;
            }

            TimeSpan min = _laps.Min(l => l.SplitTime);
            TimeSpan max = _laps.Max(l => l.SplitTime);

            foreach (var lap in _laps)
            {
                if (lap.SplitTime == min)
                {
                    lap.ColorBrush = GreenBrush;
                    lap.Tag = "[NAJSZYBSZE]";
                }
                else if (lap.SplitTime == max)
                {
                    lap.ColorBrush = RedBrush;
                    lap.Tag = "[NAJWOLNIEJSZE]";
                }
                else
                {
                    lap.ColorBrush = WhiteBrush;
                    lap.Tag = string.Empty;
                }
            }

            // Odświeżenie widoku kolekcji
            LapsItemsControl.Items.Refresh();
        }

        private void ResetBtn_Click(object sender, RoutedEventArgs e)
        {
            ResetStopwatch();
        }

        private void ResetStopwatch()
        {
            _stopwatch.Reset();
            _timer.Stop();
            _lastLapTime = TimeSpan.Zero;
            _laps.Clear();

            UpdateDisplay();

            StartStopBtn.Content = "Start";
            StartStopBtn.Background = GreenBrush;
            LapBtn.IsEnabled = false;
            ResetBtn.IsEnabled = false;

            LapCountText.Text = "Brak okrążeń";
            StatusTextBlock.Text = "Gotowy do startu";
            StatusTextBlock.Foreground = GreenBrush;
        }

        private void TopmostCheck_Checked(object sender, RoutedEventArgs e)
        {
            this.Topmost = true;
        }

        private void TopmostCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            this.Topmost = false;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                ToggleStartStop();
                e.Handled = true;
            }
            else if (e.Key == Key.L)
            {
                if (LapBtn.IsEnabled) AddLap();
                e.Handled = true;
            }
            else if (e.Key == Key.R)
            {
                if (ResetBtn.IsEnabled) ResetStopwatch();
                e.Handled = true;
            }
        }
    }

    public class LapItem
    {
        public string LapNumber { get; set; } = "";
        public TimeSpan SplitTime { get; set; }
        public TimeSpan TotalTime { get; set; }
        public string LapSplitFormatted => $"+{SplitTime.Hours:00}:{SplitTime.Minutes:00}:{SplitTime.Seconds:00}.{(SplitTime.Milliseconds / 10):00}";
        public string TotalTimeFormatted => $"{TotalTime.Hours:00}:{TotalTime.Minutes:00}:{TotalTime.Seconds:00}.{(TotalTime.Milliseconds / 10):00}";
        public Brush ColorBrush { get; set; } = Brushes.White;
        public string Tag { get; set; } = "";
    }
}