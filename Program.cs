using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using System.Net.Http;
using BBIReporting;
using System.Runtime.InteropServices;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System.Security.Cryptography.X509Certificates;

namespace DynamicDashboardWinForm
{
    static class Program
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();
        public static ServiceProvider ServiceProvider { get; private set; }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Setup the DI container and register services
            var services = new ServiceCollection();
            ConfigureServices(services);

            // Build the service provider
            ServiceProvider = services.BuildServiceProvider();

            // Resolve the main form and run the application
            var dashboardForm = (DashboardForm)ServiceProvider.GetService<IDashboardView>();
            Application.Run(dashboardForm);
        }

        private static void ConfigureServices(ServiceCollection services)
        {

            // Add NLog to the service collection and integrate it with Microsoft.Extensions.Logging
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders(); // Clear other loggers
                loggingBuilder.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Trace);
                loggingBuilder.AddNLog(); // Add NLog to the logging providers
            });

            // Register HttpClient
            services.AddSingleton<HttpClient>();


            // Register API clients
            services.AddSingleton<IMerakiAPIClient>(provider =>
                new MerakiAPIClient(
                    provider.GetRequiredService<HttpClient>(),
                    Environment.GetEnvironmentVariable("BBIMerakiKey"),
                    provider.GetRequiredService<ILogger<MerakiAPIClient>>() // NLog logger for MerakiAPIClient
                )
            );
            services.AddSingleton<IAirWatchAPIClient>(provider =>
               new AirWatchAPIClient(
                   provider.GetRequiredService<HttpClient>(),
                   "your-airwatch-api-key",
                   provider.GetRequiredService<ILogger<AirWatchAPIClient>>()
               )
           );



            // Register the MerakiService, which depends on IMerakiAPIClient and ILogger
            services.AddScoped<IMerakiService, MerakiService>(provider =>
            {
                var apiClient = provider.GetRequiredService<IMerakiAPIClient>();
                var logger = provider.GetRequiredService<ILogger<MerakiService>>();
                return new MerakiService(apiClient, logger);
            });

            // Register the AirWatchService, which depends on IAirWatchAPIClient and ILogger
            services.AddScoped<IAirWatchService, AirWatchService>(provider =>
            {
                var apiClient = provider.GetRequiredService<IAirWatchAPIClient>();
                var logger = provider.GetRequiredService<ILogger<AirWatchService>>();
                return new AirWatchService(apiClient, logger);
            });

            // Register the DashboardPresenter
            services.AddSingleton<DashboardPresenter>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<DashboardPresenter>>();
                var airWatchService = provider.GetRequiredService<IAirWatchService>();
                var merakiService = provider.GetRequiredService<IMerakiService>();
                var progressForm = provider.GetRequiredService<ProgressForm>();
                var view = provider.GetRequiredService<IDashboardView>();

                return new DashboardPresenter(view, logger, airWatchService, merakiService, progressForm);
            });

            // Register the ProgressForm
            services.AddSingleton<ProgressForm>();

            // Register the DashboardForm and inject its dependencies
            services.AddSingleton<IDashboardView>(provider =>
            {
                var logger = provider.GetRequiredService<ILogger<DashboardForm>>();
                var restaurantLogger = provider.GetRequiredService<ILogger<Restaurant>>();                var airWatchService = provider.GetRequiredService<IAirWatchService>();
                var merakiService = provider.GetRequiredService<IMerakiService>();
                var progressForm = provider.GetRequiredService<ProgressForm>();

                return new DashboardForm(ServiceProvider, logger, restaurantLogger, airWatchService, merakiService, progressForm);
            });
        }
    }

}
