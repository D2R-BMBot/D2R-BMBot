using System;

using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;

using BMBot.GUI.Avalonia.Models.DataStructures.Logging;
using BMBot.GUI.Avalonia.Models.IO.Files;
using BMBot.GUI.Avalonia.Models.Services.Game;
using BMBot.GUI.Avalonia.Models.Services.Navigation;
using BMBot.GUI.Avalonia.Models.UI;
using BMBot.GUI.Avalonia.Models.Utilities;
using BMBot.GUI.Avalonia.ViewModels;
using BMBot.GUI.Avalonia.ViewModels.MainWindow;
using BMBot.GUI.Avalonia.ViewModels.MainWindow.MainWorkspace;
using BMBot.GUI.Avalonia.Views;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using Serilog;

namespace BMBot.GUI.Avalonia;

public class BotApp : Application
{
    private static IConfigurationRoot Configuration   { get; } = GetConfiguration();
    private static IServiceProvider   ServiceProvider { get; } = ConfigureServices();

    public static ViewModelLocator Locator { get; } = new(ServiceProvider);
    
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if ( ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop )
        {
            desktop.MainWindow = new MainWindowView();
        }

        base.OnFrameworkInitializationCompleted();
    }
    
    private static ServiceProvider ConfigureServices()
        {
            var serviceCollection = new ServiceCollection();

            serviceCollection.AddLogging(ConfigureLogging);

            PrepareServices(serviceCollection);

            var services = serviceCollection.BuildServiceProvider();

            return services;

            void ConfigureLogging(ILoggingBuilder p_builder)
            {
                var configuredLogLevel =
                    LogLevelUtilities.GetLogLevel(Configuration["Logging:LogLevel:Default"]);

                p_builder.ClearProviders();

                Log.Logger = new LoggerConfiguration()
                             .MinimumLevel
                             .Is(LogLevelUtilities.GetSerilogLogLevel(configuredLogLevel))
                             .WriteTo.Sink(new CollectionSink())
                             .WriteTo.Debug()
                             .WriteTo.File(ApplicationFiles.LogsFilePath,
                                           rollingInterval: RollingInterval.Day,
                                           retainedFileCountLimit: 31,
                                           fileSizeLimitBytes: 1024 * 1024 * 10,
                                           rollOnFileSizeLimit: true)
                             .CreateLogger();

                // Add logging to collection sink. - Comment by Matt Heimlich on 07/10/2023@11:28:22
                p_builder.AddSerilog(Log.Logger);
            }

            void PrepareServices(IServiceCollection p_services)
            {
                p_services.AddSingleton<NavigationService>();
                
                p_services.AddSingleton<MainWindowViewModel>();

                p_services.AddSingleton<MainWorkspaceViewModel>();
                
                p_services.AddSingleton<AccountManagementViewModel>();

                p_services.AddSingleton<InstanceService>();
            }
        }
        
        private static IConfigurationRoot GetConfiguration()
        {
            var configurationBuilder = new ConfigurationBuilder();

            var environment = Environment.GetEnvironmentVariable("DOTNET_ENVIRONMENT") ?? "Production";
        
            configurationBuilder.AddJsonFile(environment.Equals("Development") ? "appsettings.Development.json" : "appsettings.json", true, true);

            return configurationBuilder.Build();
        }
}