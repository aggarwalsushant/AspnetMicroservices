using Catalog.API;

var builder = WebApplication.CreateBuilder(args);

var startup = new Startup();
startup.RegisterServices(builder.Services);

var app = builder.Build();

startup.SetupMiddleware(app, app.Environment);

app.Run();
