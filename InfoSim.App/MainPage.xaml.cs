using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using System.Windows;
using Windows.UI.Core;
using InfoSim.App.Controls;
using InfoSim.App.Models;
using Windows.UI.Xaml.Media.Imaging;
using InfoSim.App.Tools;
using Windows.UI;
using Google.Apis.Calendar.v3.Data;

namespace InfoSim
{
    public sealed partial class MainPage : Windows.UI.Xaml.Controls.Page
    {
        int ticks = 0;

        int calUpdateTick = 0;
        int calUpdateInt = 21600000;

        int weaUpdateTick = 0;
        int weaUpdateInt = 21600000;

        DispatcherTimer timer = new DispatcherTimer();

        bool sqlOnline = false;
        public MainPage()
        {
            this.InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTick;

            SetupControl.SetupSqlConn();

            GetWeather();
            GetGarMon();

            timer.Start();
            //StartTimer();
            //WeatherTick(null);
            //StartCalendar();
        }

        public void TimerTick(object sender, object e)
        {
            txbClock.Text = $"{DateTime.Now.ToString("h:mm:ss tt")}";
            txbDate.Text = $"{DateTime.Now.DayOfWeek}, {DateTime.Now.Month}/{DateTime.Now.Day}/{DateTime.Now.Year}";

            ticks++;
            calUpdateTick++;
            weaUpdateTick++;

            if (ticks >= 3600)
            {
                if (!sqlOnline)
                    sqlOnline = SetupControl.SetupSqlConn();

                ticks = 0;
                GetGarMon();
            }

            if (calUpdateTick >= calUpdateInt)
                GetCalendarEvents();

            if (weaUpdateTick >= weaUpdateInt)
                GetWeather();
        }

        private async void GetWeather()
        {
            RootObject myWeather = await WeatherControl.GetWeather();

            var ignored = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            {
                tbxLocation.Text = myWeather.name + ", " + myWeather.sys.country;
                tbxTemp.Text = Convert.ToString(Math.Round(myWeather.main.temp * (1.8) - 459.67)) + "°F |";
                tbxHigh.Text = Convert.ToString(Math.Round(myWeather.main.temp_max * (1.8) - 459.67)) + "°";
                tbxLow.Text = Convert.ToString(Math.Round(myWeather.main.temp_min * (1.8) - 459.67)) + "°";
                tbxDesc.Text = FormatTools.Capitalize(myWeather.weather[0].description);
                string icon = myWeather.weather[0].icon.Replace('d', 'n');
                icoDesc.Source = new BitmapImage(new Uri($"http://openweathermap.org/img/wn/{icon}@2x.png", UriKind.Absolute));
                tbxClouds.Text = " " + Convert.ToString(myWeather.clouds.all) + "%";
                tbxHumidty.Text = " " + Convert.ToString(myWeather.main.humidity) + "%";
                tbxSunrise.Text = " " + FormatTools.UnixTimeStampToDateTime(myWeather.sys.sunrise).ToString("h:mm tt");
                tbxSunset.Text = " " + FormatTools.UnixTimeStampToDateTime(myWeather.sys.sunset).ToString("h:mm tt");
            });
        }

        private void TglWeather_Toggled(object sender, RoutedEventArgs e)
        {
            if(tglWeather.IsOn)
            {
                //pnlWeather.Visibility = Visibility.Visible;
            }
            else
            {
                //pnlWeather.Visibility = Visibility.Collapsed;
            }
        }

        private void BtnShowOptions_Click(object sender, RoutedEventArgs e)
        {
            if (pnlOptions.Visibility == Visibility.Collapsed)
            {
                pnlOptions.Visibility = Visibility.Visible;
                pnlOptions.Visibility = Visibility.Collapsed;
            }
            else
            {
                pnlOptions.Visibility = Visibility.Collapsed;
                pnlOptions.Visibility = Visibility.Visible;
            }
        }

        private void GetGarMon()
        {
            if (!sqlOnline)
            {
                sqlOnline = SetupControl.SetupSqlConn();
                txbGStatus.Foreground = new SolidColorBrush(Windows.UI.Colors.OrangeRed);
                txbGStatus.Text = "Unknown";
            }
            else
            {
                string status = GarMonControl.GetStatus();

                switch (status)
                {
                    case "Closed":
                        txbGStatus.Foreground = new SolidColorBrush(Windows.UI.Colors.DarkCyan);
                        txbGStatus.Text = "Closed";
                        break;
                    case "Open":
                        txbGStatus.Foreground = new SolidColorBrush(Windows.UI.Colors.OrangeRed);
                        txbGStatus.Text = "Open";
                        break;
                    case "Unknown":
                        txbGStatus.Foreground = new SolidColorBrush(Windows.UI.Colors.OrangeRed);
                        txbGStatus.Text = "Unknown";
                        break;
                    default:
                        txbGStatus.Foreground = new SolidColorBrush(Windows.UI.Colors.OrangeRed);
                        txbGStatus.Text = "Unknown";
                        break;
                }
            }
        }

        private void GetCalendarEvents()
        {
            if (!sqlOnline)
                sqlOnline = SetupControl.SetupSqlConn();
            else
            {
                lsvEvents.Items.Clear();

                TextBlock temp = new TextBlock();
                temp.VerticalAlignment = VerticalAlignment.Center;
                temp.HorizontalAlignment = HorizontalAlignment.Left;
                temp.Foreground = new SolidColorBrush(Windows.UI.Colors.DarkCyan);
                temp.FontSize = 30;
                temp.FontFamily = new FontFamily("Segoe UI");

                List<EventModel> events = CalendarControl.GetCalItems();

                if (events.Count > 0)
                {
                    foreach (EventModel e in events)
                    {
                        temp.Text = e.description;
                        lsvEvents.Items.Add(temp);
                    }
                }
                else
                {
                    temp.Text = "No upcoming events you loser";
                    lsvEvents.Items.Add(temp);
                }

            }
        }
    }
}
