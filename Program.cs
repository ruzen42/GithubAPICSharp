using GithubAPICSharp.Services;
using NeoSimpleLogger;
using Octokit;

var builder = WebApplication.CreateBuilder(args);

builder.Logging
    .ClearProviders()
    .AddProvider(new LoggerProvider());

builder.Services.AddSwaggerGen();

builder.Services.AddControllers();
builder.Services.AddSingleton<GitHubClient>(_ => new GitHubClient(new ProductHeaderValue("RuzenBot")));
builder.Services.AddScoped<IGitHubService,  GitHubService>();
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

await app.RunAsync();
