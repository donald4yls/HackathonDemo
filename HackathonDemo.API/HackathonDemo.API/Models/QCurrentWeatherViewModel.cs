namespace HackathonDemo.API.Models
{
    public class QCurrentWeatherViewModel : QWeatherBaseViewModel
    {
        public Now now { get; set; }
    }

    public class Now
    {
        public string obsTime { get; set; }
        public string temp { get; set; }
        public string feelsLike { get; set; }
        public string icon { get; set; }
        public string text { get; set; }
        public string wind360 { get; set; }
        public string windDir { get; set; }
        public string windScale { get; set; }
        public string windSpeed { get; set; }
        public string humidity { get; set; }
        public string precip { get; set; }
        public string pressure { get; set; }
        public string vis { get; set; }
        public string cloud { get; set; }
        public string dew { get; set; }
    }

    public class Refer
    {
        public List<string> sources { get; set; }
        public List<string> license { get; set; }
    }

    public class QWeatherBaseViewModel
    {
        public string code { get; set; }
        public string updateTime { get; set; }
        public string fxLink { get; set; }
        public Refer refer { get; set; }
    }
}
