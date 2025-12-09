using Elytar.Services;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient();
builder.Services.AddMemoryCache();

builder.Services.AddScoped<IElyByService, ElyByService>();
builder.Services.AddScoped<IImageProcessingService, ImageProcessingService>();

var app = builder.Build();

app.Use(async (context, next) =>
{
    var version = Assembly.GetEntryAssembly()
        ?.GetCustomAttribute<AssemblyInformationalVersionAttribute>()
        ?.InformationalVersion;

    if (!string.IsNullOrEmpty(version) && version.Contains('+'))
    {
        version = version.Split('+')[0];
    }

    context.Response.Headers.Append("X-Elytar-Version", version ?? "0.0.0");
    await next();
});

app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllers();

app.Run();
