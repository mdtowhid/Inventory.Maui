namespace InventorySystem.Application.Products;

public class ProductDto
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? CategoryId { get; set; }
    public string? CategoryName { get; set; }
    public int? SupplierId { get; set; }
    public string? SupplierName { get; set; }
    public decimal UnitPrice { get; set; }
    public int MinStock { get; set; }
    public int MaxStock { get; set; }
    public string? Barcode { get; set; }
    public string? ImagePath { get; set; }
    public bool IsActive { get; set; }
    public int CurrentStock { get; set; }
    public int AvailableStock { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class PagedResult<T>
{
    public List<T> Items { get; set; } = new();
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
}