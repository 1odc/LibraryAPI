// Program.cs
using System.Text.Json.Serialization;
using LibraryAPI.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi("v1", options =>
{
    options.ShouldInclude = apiDescription => apiDescription.GroupName is null or "v1";
});
builder.Services.AddControllers();
builder.Services.AddSingleton<BooksRepository>();
builder.Services.AddAutoMapper(cfg => { }, typeof(Program));
builder.Services.AddControllers()
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false)));
builder.Services.ConfigureHttpJsonOptions(o =>
    o.SerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false)));

var app = builder.Build();

app.UseHttpsRedirection();
app.MapOpenApi();
app.MapScalarApiReference(options =>
{
    options.AddDocuments(["v1"]);
    options.WithTitle("Library API");
});
app.UseSwaggerUI(options =>
{
    options.SwaggerEndpoint("/openapi/v1.json", "Library API v1");
});
app.MapControllers();
app.Run();