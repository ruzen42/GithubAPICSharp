var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapGet("/", () => "use POST request \n.../api/query/getrepo");
app.MapControllers();

app.Run();
