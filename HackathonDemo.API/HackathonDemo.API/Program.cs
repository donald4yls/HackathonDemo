using HackathonDemo.API.ApiDomains;
using HackathonDemo.API.Services;
using Serilog;

#if DEBUG
Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.Logger(configure => configure // 输出到文件
                .MinimumLevel.Debug()
                .WriteTo.File(  //单个日志文件，总日志，所有日志存到这里面
                    $"logs\\log.txt",
                    rollingInterval: RollingInterval.Day,
                    outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] {Message:lj}{NewLine}{Exception}"))
    .CreateLogger();
#else
Log.Logger = new LoggerConfiguration()
    // json may be better for ELK to index
    .WriteTo.Console(new Serilog.Formatting.Compact.CompactJsonFormatter()) 
    .CreateLogger();
#endif

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHttpClient();

builder.Services.AddTransient<IQweatherService, QweatherService>();
builder.Services.AddSingleton<IFaceLandmarkService, FaceLandmarkService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();
//app.MapGroup("/api/v1")
//    .MapQWeatherApi();

app.MapGroup("/api/v1")
    .MapFaceRecognitionApi();

app.MapGroup("/api/v1")
    .MapVoiceRecognitionApi();

app.Run();
