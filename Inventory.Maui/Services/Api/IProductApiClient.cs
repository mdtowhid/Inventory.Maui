using Android.Media.TV;
using Inventory.Application.Products;
using Inventory.Application.Products.Commands;
using Refit;

namespace Inventory.Maui.Services.Api;

public interface IProductApiClient
{
    [Get("/products")]
    Task<ApiResponse<PagedResult<ProductDto>>> GetProductsAsync(
        [Query] int pageNumber = 1,
        [Query] int pageSize = 20,
        [Query] string? searchTerm = null,
        [Query] int? categoryId = null,
        [Query] bool? isActive = null);

    [Get("/products/{id}")]
    Task<ApiResponse<ProductDto>> GetProductAsync(int id);

    [Post("/products")]
    Task<ApiResponse<ProductDto>> CreateProductAsync([Body] CreateProductCommand command);

    [Put("/products/{id}")]
    Task<ApiResponse> UpdateProductAsync(int id, [Body] UpdateProductCommand command);

    [Delete("/products/{id}")]
    Task<ApiResponse> DeleteProductAsync(int id);

    [Get("/products/search")]
    Task<ApiResponse<List<ProductDto>>> SearchProductsAsync([Query] string term);

    [Get("/products/low-stock")]
    Task<ApiResponse<List<ProductDto>>> GetLowStockProductsAsync([Query] int threshold = 10);
}