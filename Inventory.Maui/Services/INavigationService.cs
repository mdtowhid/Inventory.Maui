namespace Inventory.Maui.Services;

public interface INavigationService
{
    Task GoToAsync(string route, IDictionary<string, object>? parameters = null);
    Task GoBackAsync();
    Task GoToRootAsync();
}

public class NavigationService : INavigationService
{
    public async Task GoToAsync(string route, IDictionary<string, object>? parameters = null)
    {
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync(route, parameters);
        }
    }

    public async Task GoBackAsync()
    {
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync("..");
        }
    }

    public async Task GoToRootAsync()
    {
        if (Shell.Current != null)
        {
            await Shell.Current.GoToAsync("//main");
        }
    }
}