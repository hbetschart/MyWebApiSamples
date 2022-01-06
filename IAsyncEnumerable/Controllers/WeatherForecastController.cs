using Microsoft.AspNetCore.Mvc;

namespace IAsyncEnumerable.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IAsyncEnumerable<int> Get()
        {
            return FetchItems();
        }
        static async IAsyncEnumerable<int> FetchItems()
        {
            for (int i = 1; i <= 10; i++)
            {
                await Task.Delay(1000);
                yield return i;
            }
        }
        
        [HttpGet]
        [Route("GetFileContent")]
        public async IAsyncEnumerable<string> GetFileContent()
        {
            var path = Path.Combine(Environment.CurrentDirectory, "file.txt");

            var lines = GetLines(path);
            await foreach (var line in lines)
            {
                yield return line;
            }
        }

        async IAsyncEnumerable<string> GetLines(string filePath)
        {
            string line;
            StreamReader file = new System.IO.StreamReader(filePath);
            while ((line = await file.ReadLineAsync()) != null)
            {
                await Task.Delay(300);
                yield return line;
            }
        }
    }
}