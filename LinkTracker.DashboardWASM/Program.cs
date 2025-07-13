using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using LinkTracker.DashboardWASM;
using MudBlazor.Services;
using LinkTracker.Shared.Models;
using Microsoft.Extensions.DependencyInjection;
using LinkTracker.DashboardWASM.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiSettings = builder.Configuration.GetSection("ApiSettings").Get<UrlSettings>();
if (apiSettings is not null) builder.Services.AddSingleton(apiSettings);

builder.Services.AddMudServices();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiSettings.BaseAPIURLs.First()) });

builder.Services.AddScoped<IFetchAnalytics, FetchAnalyticsFromAPI>();

await builder.Build().RunAsync();
