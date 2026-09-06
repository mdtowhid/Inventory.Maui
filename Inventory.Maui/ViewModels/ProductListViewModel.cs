using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Inventory.Maui.Services;
using Inventory.Maui.Services.Api;
using InventorySystem.Application.Products;
using System.Collections.ObjectModel;

namespace Inventory.Maui.ViewModels.Products;

public partial class ProductListViewModel : BaseViewModel
{
    private readonly IApiService _apiService;
    private readonly IDialogService _dialogService;
    private readonly INavigationService _navigationService;

    [ObservableProperty]
    private ObservableCollection<ProductDto> _products = new();

    [ObservableProperty]
    private string _searchTerm = string.Empty;

    [ObservableProperty]
    private bool _isRefreshing;

    [ObservableProperty]
    private ProductDto? _selectedProduct;

    public ProductListViewModel(
        IApiService apiService,
        IDialogService dialogService,
        INavigationService navigationService)
    {
        Title = "Products";
        _apiService = apiService;
        _dialogService = dialogService;
        _navigationService = navigationService;
    }

    public IAsyncRelayCommand LoadProductsCommand => new AsyncRelayCommand(LoadProductsAsync);
    public IAsyncRelayCommand SearchCommand => new AsyncRelayCommand(SearchAsync);
    public IAsyncRelayCommand RefreshCommand => new AsyncRelayCommand(RefreshAsync);

    public override async Task OnAppearingAsync()
    {
        await LoadProductsAsync();
        await base.OnAppearingAsync();
    }

    private async Task LoadProductsAsync()
    {
        if (IsBusy) return;

        try
        {
            IsBusy = true;
            var response = await _apiService.Products.GetProductsAsync();

            if (response.IsSuccessStatusCode && response.Content != null)
            {
                Products.Clear();
                foreach (var product in response.Content.Items)
                {
                    Products.Add(product);
                }
            }
            else
            {
                await _dialogService.ShowAlertAsync("Error", "Failed to load products");
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowAlertAsync("Error", $"Failed to load products: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task SearchAsync()
    {
        if (string.IsNullOrWhiteSpace(SearchTerm))
        {
            await LoadProductsAsync();
            return;
        }

        try
        {
            IsBusy = true;
            var response = await _apiService.Products.SearchProductsAsync(SearchTerm);

            if (response.IsSuccessStatusCode && response.Content != null)
            {
                Products.Clear();
                foreach (var product in response.Content)
                {
                    Products.Add(product);
                }
            }
        }
        catch (Exception ex)
        {
            await _dialogService.ShowAlertAsync("Error", $"Search failed: {ex.Message}");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task RefreshAsync()
    {
        IsRefreshing = true;
        await LoadProductsAsync();
        IsRefreshing = false;
    }
}