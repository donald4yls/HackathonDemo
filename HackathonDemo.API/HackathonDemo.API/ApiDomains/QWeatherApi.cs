using HackathonDemo.API.Models;
using HackathonDemo.API.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HackathonDemo.API.ApiDomains
{
    public static class QWeatherApi
    {
        public static IEndpointRouteBuilder MapQWeatherApi(this IEndpointRouteBuilder builder)
        {
            var group = builder
                .MapGroup("/QWeather")
                .WithDescription("QWeather API doc");

            group.MapGet("/CurrentWeather/{location}",
                async Task<Results<Ok<QCurrentWeatherViewModel>, NotFound>>
                    (IQweatherService qweatherService, [FromRoute] string location) => {
                        var result = await qweatherService.GetQCurrentWeatherAsync(location);
                        if (result == null)
                        {
                            return TypedResults.NotFound();
                        }

                        return TypedResults.Ok(result);
                    })
                .Produces<QCurrentWeatherViewModel>(StatusCodes.Status200OK, "application/json")
                .Produces(StatusCodes.Status404NotFound)
                .WithName("GetQWeather");

            group.MapGet("/ForecastWeather/{location}",
                async Task<Results<Ok<QForecastWeatherViewModel>, NotFound>>
                    (IQweatherService qweatherService, [FromRoute] string location) => {
                        var result = await qweatherService.GetForecastWeatherAsync(location);
                        if (result == null)
                        {
                            return TypedResults.NotFound();
                        }

                        return TypedResults.Ok(result);
                    })
                .Produces<QForecastWeatherViewModel>(StatusCodes.Status200OK, "application/json")
                .Produces(StatusCodes.Status404NotFound)
                .WithName("GetForecastWeather");

            return group;
        }
    }
}
