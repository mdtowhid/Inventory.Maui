namespace Inventory.Domain.Enums;

public enum OrderStatus
{
    Draft = 0,
    Pending = 1,
    Processing = 2,
    Shipped = 3,
    Delivered = 4,
    Completed = 5,
    Cancelled = 6,
    Returned = 7,
    OnHold = 8
}