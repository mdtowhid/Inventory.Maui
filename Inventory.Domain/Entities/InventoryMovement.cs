using Inventory.Domain.Common;
using Inventory.Domain.Enums;
using InventorySystem.Domain.Entities;

namespace Inventory.Domain.Entities;

public class InventoryMovement : BaseEntity
{
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public MovementType MovementType { get; set; }
    public int? ReferenceId { get; set; }
    public string? ReferenceType { get; set; }
    public int? PreviousQuantity { get; set; }
    public int? NewQuantity { get; set; }
    public string? Notes { get; set; }
    public int? UserId { get; set; }

    // Navigation Properties
    public virtual Product? Product { get; set; }
}