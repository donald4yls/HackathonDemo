using System.IO.Compression;
using System.Text.Json;
using System.Text;
using HackathonDemo.API.Models;

namespace HackathonDemo.API.Services
{
    public class QweatherService: IQweatherService
    {
        private readonly HttpClient _client;
        private readonly ILogger<QweatherService> _logger;


        public QweatherService(ILogger<QweatherService> logger,
            IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _client = clientFactory.CreateClient();
        }

        public async Task<QCurrentWeatherViewModel> GetQCurrentWeatherAsync(string location)
        {
            var content = await _client.GetStreamAsync($"https://devapi.qweather.com/v7/weather/now?location={location}&key=42e9a37b0669452f8b195c2620d10afd&lang=en");

            return GetObjectFromGZipStream<QCurrentWeatherViewModel>(content);
        }

        public async Task<QForecastWeatherViewModel> GetForecastWeatherAsync(string location)
        {
            var content = await _client.GetStreamAsync($"https://devapi.qweather.com/v7/weather/3d?location={location}&key=42e9a37b0669452f8b195c2620d10afd&lang=en");

            return GetObjectFromGZipStream<QForecastWeatherViewModel>(content);
        }

        private T? GetObjectFromGZipStream<T>(Stream content)
        {
            if(content == null)
            {
                return default(T);
            }

            using (var gZipStream = new GZipStream(content, CompressionMode.Decompress))
            using (var output = new MemoryStream())
            {
                gZipStream.CopyTo(output);
                var result = Encoding.UTF8.GetString(output.ToArray());
                return JsonSerializer.Deserialize<T>(result);
            }
        }
    }

    public interface IQweatherService
    {
        Task<QCurrentWeatherViewModel> GetQCurrentWeatherAsync(string location);

        Task<QForecastWeatherViewModel> GetForecastWeatherAsync(string location);
    }
}
