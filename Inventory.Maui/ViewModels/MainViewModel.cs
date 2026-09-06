using CommunityToolkit.Mvvm.ComponentModel;
using Inventory.Maui.Services;

namespace Inventory.Maui.ViewModels;

public partial class MainViewModel : BaseViewModel
{
    private readonly ISettingsService _settingsService;

    [ObservableProperty]
    private string _version;

    public MainViewModel(ISettingsService settingsService)
    {
        _settingsService = settingsService;
        Title = "Dashboard";
        Version = _settingsService.GetString("AppVersion", "v1.0.0");
    }
}