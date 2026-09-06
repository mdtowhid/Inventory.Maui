using Inventory.Application.Products.Commands;
using InventorySystem.Application.Products;
using InventorySystem.Application.Products.Commands;
using Refit;

namespace Inventory.Maui.Services.Api;

public interface IProductApiClient
{
    [Get("/products")]
    Task<IApiResponse<PagedResult<ProductDto>>> GetProductsAsync(
        [Query] int pageNumber = 1,
        [Query] int pageSize = 20,
        [Query] string? searchTerm = null,
        [Query] int? categoryId = null,
        [Query] bool? isActive = null);

    [Get("/products/{id}")]
    Task<IApiResponse<ProductDto>> GetProductAsync(int id);

    [Post("/products")]
    Task<IApiResponse<ProductDto>> CreateProductAsync([Body] CreateProductCommand command);

    [Put("/products/{id}")]
    Task<IApiResponse> UpdateProductAsync(int id, [Body] UpdateProductCommand command);

    [Delete("/products/{id}")]
    Task<IApiResponse> DeleteProductAsync(int id);

    [Get("/products/search")]
    Task<IApiResponse<List<ProductDto>>> SearchProductsAsync([Query] string term);

    [Get("/products/low-stock")]
    Task<IApiResponse<List<ProductDto>>> GetLowStockProductsAsync([Query] int threshold = 10);
}