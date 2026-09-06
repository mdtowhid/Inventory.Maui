using MediatR;

namespace Inventory.Application.Products.Commands;

public record UpdateProductCommand : IRequest<bool>
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? CategoryId { get; set; }
    public int? SupplierId { get; set; }
    public decimal UnitPrice { get; set; }
}