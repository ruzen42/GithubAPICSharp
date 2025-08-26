using NeoSimpleLogger;

var builder = WebApplication.CreateBuilder(args);

builder.Logging
    .ClearProviders()
    .AddProvider(new LoggerProvider());

builder.Services.AddControllers();
var app = builder.Build();

app.MapControllers();
app.MapGet("/", () => "use POST request \n.../api/query/getrepo");

app.Run();
