using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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

namespace Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        string APIKey = "67fe3285486a7f123b0fb08665aa9d51";

        private void Button_Click(object sender, EventArgs e)
        {
            WP.Children.Clear();
            getWeather();
            getForecast();
        }

        double lon;
        double lat;
        void getWeather()
        {
            using (WebClient web = new WebClient())
            {
                string url = string.Format("https://api.openweathermap.org/data/2.5/weather?q={0}&appid={1}", TBCity.Text, APIKey);
                var json = web.DownloadString(url);
                WeatherInfo.root Info = JsonConvert.DeserializeObject<WeatherInfo.root>(json);


                picIcon.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("https://api.openweathermap.org/img/w/" + Info.weather[0].icon + ".png"));
                labCondition.Text = Info.weather[0].main;
                labCondition.Text = Info.weather[0].main;
                labDetails.Text = Info.weather[0].description;
                labsunrise.Text = converDateTime(Info.sys.sunrise).ToShortTimeString();
                labsunset1.Text = converDateTime(Info.sys.sunset).ToShortTimeString();

                labWindSpeed.Text = Info.wind.speed.ToString();
                labPressure.Text = Info.main.pressure.ToString();

                lon = Info.coord.lon;
                lat = Info.coord.lat;
            }
        }

        DateTime converDateTime(long sec)
        {
            DateTime day = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc).ToLocalTime();
            day = day.AddSeconds(sec).ToLocalTime();

            return day;
        }

        void getForecast()
        {
            using (WebClient web = new WebClient())
            {
                string url = string.Format("https://api.openweathermap.org/data/2.5/onecall?lat={0}&lon={1}&exclude=current.minutely.hourly.alerts&appid={2}", lat, lon, APIKey);
                var json = web.DownloadString(url);

                WeatherForecast.ForecastInfo ForecastInfo = JsonConvert.DeserializeObject<WeatherForecast.ForecastInfo>(json);

                ForecastUC FUC;
                for (int i = 0; i < 7; i++)
                {
                    FUC = new ForecastUC();
                    FUC.picWeatherIcon.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri("https://api.openweathermap.org/img/w/" + ForecastInfo.daily[i].weather[0].icon + ".png"));
                    FUC.labMainWeather.Text = ForecastInfo.daily[i].weather[0].main;
                    FUC.labWeatherDescription.Text = ForecastInfo.daily[i].weather[0].description;
                    FUC.labDT.Text = converDateTime(ForecastInfo.daily[i].dt).DayOfWeek.ToString();

                    WP.Children.Add(FUC);
                }
            }
        }

        void displayClothes()
        {
            
        }
        
    }
}
