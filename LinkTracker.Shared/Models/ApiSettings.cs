using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LinkTracker.Shared.Models
{
    public class ApiSettings
    {
        public string BaseHttpURL { get; set; }
        public string BaseHttpsURL { get; set; }
    }

    public static class ApiSettingsExtensions
    {
        public static IConfigurationBuilder ConfigureApiSettingsFromSettingsFile(this IConfigurationBuilder configuration)
        {
            string folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            configuration.AddJsonFile(Path.Combine(folder, "projectsettings.json"), optional: false, reloadOnChange: false);
            return configuration;
        }

        public static WebApplicationBuilder UseUrlsFromProjectSettings(this WebApplicationBuilder builder)
        {
            ApiSettings settings = builder.Configuration.GetApiSettings();
            builder.WebHost.UseUrls(settings.BaseHttpURL, settings.BaseHttpsURL);
            return builder;
        }

        public static ApiSettings GetApiSettings(this IConfiguration configuration)
        {
            ApiSettings settings = new();
            configuration.GetSection("ApiSettings").Bind(settings);
            return settings;
        }
    }
}
