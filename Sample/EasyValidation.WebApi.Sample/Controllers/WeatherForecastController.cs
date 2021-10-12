using EasyValidation.Core;
using EasyValidation.Core.Results;
using EasyValidation.DependencyInjection;
using EasyValidation.WebApi.Sample.Commands;
using EasyValidation.WebApi.Sample.Validators;
using Microsoft.AspNetCore.Mvc;

namespace EasyValidation.WebApi.Sample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost]
        public IActionResult Post([FromBody] PersonCommand command)
        {
            var validator = new PersonValidator();

            validator.SetValue(command);
            validator.Validate();

            var jsonResult = validator.ResultData.ToJson();

            if (validator.HasErrors)
                return BadRequest(jsonResult);

            return Ok(jsonResult);
        }

        [HttpPost("Post2")]
        public IActionResult Post2(
            [FromServices] IValidatorLocator validator,
            [FromBody] PersonCommand command
        )
        {
            var resultData = validator.ValidateCommand(command);

            var jsonResult = resultData.ToJson();

            if (!resultData.IsValid)
                return BadRequest(jsonResult);

            return Ok(jsonResult);
        }
    }
}