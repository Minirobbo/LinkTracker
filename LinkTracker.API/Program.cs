using LinkTracker.API.Services.Analytics;
using LinkTracker.API.Services.FileStorage;
using LinkTracker.API.Services.RedirectionManager;
using LinkTracker.Shared.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.OpenApi.Models;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var urlSettings = builder.Configuration.GetSection("ApiSettings").Get<UrlSettings>();
builder.WebHost.UseUrls(urlSettings.BaseAPIURLs);
// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSingleton<IFileStorage, InMemFileStorage>();
builder.Services.AddSingleton<IRedirectionManager, InMemRedirectionManager>();
builder.Services.AddSingleton<IAnalyticsTracker, InMemAnalytics>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy.WithOrigins(urlSettings.BaseWASMURLs)
        .AllowAnyMethod()
        .AllowAnyHeader()
        .AllowCredentials();
    });
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "LinkTracker API",
        Description = "An ASP.NET Web API for redirecting links / files and recording analytics on link redirection"
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

var app = builder.Build();

app.UseCors("CorsPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
