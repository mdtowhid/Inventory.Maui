using Inventory.Maui.Helpers;
using Microsoft.Maui.Controls;

namespace Inventory.Maui.Services;

public interface IDialogService
{
    Task ShowAlertAsync(string title, string message, string cancel = "OK");
    Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No");
    Task<string> ShowPromptAsync(string title, string message, string placeholder = "");
}

public class DialogService : IDialogService
{
    public async Task ShowAlertAsync(string title, string message, string cancel = "OK")
    {
        var mainPage = Microsoft.Maui.Controls.Application.Current?.MainPage;
        await mainPage.DisplayAlertAsync(title, message, cancel);
    }

    public async Task<bool> ShowConfirmationAsync(string title, string message, string accept = "Yes", string cancel = "No")
    {
        var mainPage = Microsoft.Maui.Controls.Application.Current?.MainPage;
        return await mainPage.DisplayConfirmationAsync(title, message, accept, cancel);
    }

    public async Task<string> ShowPromptAsync(string title, string message, string placeholder = "")
    {
        var mainPage = Microsoft.Maui.Controls.Application.Current?.MainPage;
        return await mainPage.DisplayPromptAsync(title, message, placeholder);
    }
}