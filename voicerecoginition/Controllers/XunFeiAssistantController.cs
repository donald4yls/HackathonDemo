using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using XunFei;
namespace voiceRecognition.Api;

public static class VoiceRecognitionApi
{
    public static IEndpointRouteBuilder MapVoiceRecognitionApi(this IEndpointRouteBuilder builder)
    {
        var group = builder
            .MapGroup("/VoiceRecognition")
            .WithDescription("Voice Recognition API doc");

        group.MapPost("/{voicefile}", async (string voicefile) =>
        {
            XunFeiAssistant xunFeiAssistant = new XunFeiAssistant() { testFile = voicefile };
            var result = await xunFeiAssistant.GetResultAsync();
            return TypedResults.Ok(result);
        });

        return group;
    }
}

