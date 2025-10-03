using NeoSimpleLogger;

var builder = WebApplication.CreateBuilder(args);

builder.Logging
    .ClearProviders()
    .AddProvider(new LoggerProvider());

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

await app.RunAsync();
