using BookStoreDocker.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddElasticSearch(builder.Configuration);


var app = builder.Build();

app.UseRouting();


app.MapControllers();

app.Run();
