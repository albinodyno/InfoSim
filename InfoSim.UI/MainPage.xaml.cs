using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace InfoSim.UI
{
    public sealed partial class MainPage : Page
    {
        int ticks = 0;
        int firstCheck = 0;
        int firstInt = 900;
        int secondCheck = 0;
        int secondInt = 1900;

        bool sqlOnline = false;

        DispatcherTimer timer = new DispatcherTimer();

        public MainPage()
        {
            this.InitializeComponent();

            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += TimerTick;
            CheckAllSubs();
            timer.Start();
        }

        public void CheckAllSubs()
        {
            sqlOnline = Tools.SQLSetup.SetupSqlConn();
            //GetWeather();
            //GetGarMon();
            //GetCalendar();
        }

        public void TimerTick(object sender, object e)
        {
            txbClock.Text = $"{DateTime.Now.ToString("h:mm:ss tt")}";
            txbDate.Text = $"{DateTime.Now.DayOfWeek}, {DateTime.Now.Month}/{DateTime.Now.Day}/{DateTime.Now.Year}";

            ticks++;
            firstCheck++;
            secondCheck++;

            if(firstCheck >= firstInt)
            {
                sqlOnline = Tools.SQLSetup.SetupSqlConn();
                //GetGarmon();
                firstCheck = 0;
            }

            if(secondCheck >= secondInt)
            {
                //GetWeather();
                //GetCalendar();
                secondCheck = 0;
            }
        }

        public void GetGarmon()
        {

        }

        public void GetWeather()
        {

        }

        public void GetCalendar()
        {

        }
        

    }
}
