using HackathonDemo.API.Models;
using HackathonDemo.API.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace HackathonDemo.API.ApiDomains
{
    public static class FaceRecognitionApi
    {
        public static IEndpointRouteBuilder MapFaceRecognitionApi(this IEndpointRouteBuilder builder)
        {
            var group = builder
                .MapGroup("/FaceRecognition")
                .WithDescription("Face Recognition API doc");

            group.MapPost("/drowsyCheck", async (IFaceLandmarkService faceLandmarkService, IFormFile file) =>
            {
                if(file == null)
                {
                    throw new ArgumentNullException(nameof(file));
                }

                var fileName = file.FileName;
                var fileExtension = fileName.Split('.').Last();
                string tempfile = CreateTempfilePath(fileExtension);
                using (var stream = File.OpenWrite(tempfile))
                {
                    await file.CopyToAsync(stream);
                }

                var drowsyCheck = faceLandmarkService.detect_landmark(tempfile);

                return TypedResults.Ok(drowsyCheck);
            })
                .Produces<FaceLandmarkResponse>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .WithName("drowsyCheck");


            group.MapPost("/drowsyCheckPostman", async (IFaceLandmarkService faceLandmarkService, IFormFile file) =>
            {
                if (file == null)
                {
                    throw new ArgumentNullException(nameof(file));
                }

                var fileName = file.FileName;
                var fileExtension = fileName.Split('.').Last();
                string tempfile = CreateTempfilePath(fileExtension);
                using (var stream = File.OpenWrite(tempfile))
                {
                    await file.CopyToAsync(stream);
                }

                var drowsyCheck = faceLandmarkService.detect_landmark(tempfile, saveResult: true);

                return TypedResults.Ok(drowsyCheck);
            })
               .Produces<FaceLandmarkResponse>(StatusCodes.Status200OK)
               .Produces(StatusCodes.Status400BadRequest)
               .WithName("drowsyCheckPostman");

            group.MapGet("/{imgName}", Results<FileContentHttpResult, NotFound> ([FromQuery] string imgPath) => {

                if(!File.Exists(imgPath))
                {
                    return TypedResults.NotFound();
                }

                using (var sw = new FileStream(imgPath, FileMode.Open))
                {
                    var bytes = new byte[sw.Length];
                    sw.Read(bytes, 0, bytes.Length);
                    sw.Close();
                    return TypedResults.File(bytes, "image/jpeg");
                }
            })
                .Produces<FileContentResult>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status400BadRequest)
                .WithName("GetMarkedImage");


            static string CreateTempfilePath(string extension)
            {
                var filename = $"{DateTime.Now.Ticks}." + extension;
                var directoryPath = Path.Combine("temp", "uploads");
                if (!Directory.Exists(directoryPath)) Directory.CreateDirectory(directoryPath);

                return Path.Combine(directoryPath, filename);
            }

            static string GetTempfilePath(string fileName)
            {
                var directoryPath = Path.Combine("temp", "marked");
                return Path.Combine(directoryPath, fileName);
            }

            return group;
        }
    }
}
