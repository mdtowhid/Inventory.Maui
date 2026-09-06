using Microsoft.Maui.Controls;

namespace Inventory.Maui.Helpers;

public static class PageExtensions
{
    public static async Task DisplayAlertAsync(this Page? page, string title, string message, string cancel = "OK")
    {
        if (page != null)
        {
            await page.DisplayAlertAsync(title, message, cancel);
        }
    }

    public static async Task<bool> DisplayConfirmationAsync(this Page? page, string title, string message, string accept = "Yes", string cancel = "No")
    {
        if (page != null)
        {
            return await page.DisplayAlertAsync(title, message, accept, cancel);
        }
        return false;
    }

    public static async Task<string> DisplayPromptAsync(this Page? page, string title, string message, string placeholder = "")
    {
        if (page != null)
        {
            return await page.DisplayPromptAsync(title, message, placeholder: placeholder) ?? string.Empty;
        }
        return string.Empty;
    }
}