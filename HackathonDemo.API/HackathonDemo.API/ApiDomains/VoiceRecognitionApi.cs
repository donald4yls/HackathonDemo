using HackathonDemo.API.Services;

namespace HackathonDemo.API.ApiDomains
{
    public static class VoiceRecognitionApi
    {
        public static IEndpointRouteBuilder MapVoiceRecognitionApi(this IEndpointRouteBuilder builder)
        {
            var group = builder
                .MapGroup("/VoiceRecognition")
                .WithDescription("Voice Recognition API doc");

            group.MapPost("/{voicefile}", async (IFormFile voicefile) =>
            {
                if(voicefile == null)
                    throw new ArgumentNullException(nameof(voicefile));

                using ( var ms = new MemoryStream())
                {
                    await voicefile.CopyToAsync(ms);
                    ms.Seek(0, SeekOrigin.Begin);
                    XunFeiAssistant xunFeiAssistant = new XunFeiAssistant() { _filestream = ms };
                    var result = await xunFeiAssistant.GetResultAsync();
                    return TypedResults.Ok(result);
                }                
            });

            return group;
        }
    }
}
