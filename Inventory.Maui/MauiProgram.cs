using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using CommunityToolkit.Maui;
using Refit;
using Inventory.Maui.Services;
using Inventory.Maui.Services.Api;
using Inventory.Application;  // ✅ This is correct (not Inventory.Application)

namespace Inventory.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        // 1. Configure MAUI
        builder
            .UseMauiApp<App>()
            .UseMauiCommunityToolkit()
            //.UseMauiCommunityToolkitMediaElement()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // 2. Configuration
        var configBuilder = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        builder.Configuration.AddConfiguration(configBuilder);

        // 3. Logging
        Log.Logger = new LoggerConfiguration()
            .WriteTo.File(
                Path.Combine(FileSystem.AppDataDirectory, "logs", "inventory_log.txt"),
                rollingInterval: RollingInterval.Day,
                outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level:u3}] {Message:lj}{NewLine}{Exception}"
            )
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();

        // 4. Register Services
        ConfigureServices(builder.Services, builder.Configuration);

        return builder.Build();
    }

    private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
    {
        // API Configuration
        var apiBaseUrl = configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7001/api/";

        // Register Refit API Clients
        services.AddRefitClient<IProductApiClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiBaseUrl));

        services.AddRefitClient<IInventoryApiClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiBaseUrl));

        services.AddRefitClient<IOrderApiClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiBaseUrl));

        services.AddRefitClient<IAuthApiClient>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri(apiBaseUrl));

        // API Service
        services.AddScoped<IApiService, ApiService>();

        // Navigation Services
        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IDialogService, DialogService>();
        services.AddSingleton<ISettingsService, SettingsService>();

        // Register ViewModels
        services.AddTransient<ViewModels.MainViewModel>();
        services.AddTransient<ViewModels.Products.ProductListViewModel>();

        // Application Layer
        services.AddApplication();  // ✅ This comes from Inventory.Application

        // Add HTTP Client for direct calls if needed
        services.AddHttpClient("ApiClient", client =>
        {
            client.BaseAddress = new Uri(apiBaseUrl);
            client.DefaultRequestHeaders.Add("Accept", "application/json");
        });
    }
}