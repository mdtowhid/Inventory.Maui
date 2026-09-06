using AutoMapper;
using Inventory.Application.Products.Commands;
using Inventory.Domain.Entities;
using InventorySystem.Application.Auth;
using InventorySystem.Application.Inventory;
using InventorySystem.Application.Orders;
using InventorySystem.Application.Products;
using InventorySystem.Application.Products.Commands;
using InventorySystem.Domain.Entities;

namespace InventorySystem.Application.Common.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Product mappings - FIXED: Remove InventoryItem references
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.CategoryName,
                opt => opt.MapFrom(src => src.Category != null ? src.Category.Name : null))
            .ForMember(dest => dest.SupplierName,
                opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : null))
            .ForMember(dest => dest.CurrentStock,
                opt => opt.Ignore())  // Will be set separately
            .ForMember(dest => dest.AvailableStock,
                opt => opt.Ignore()); // Will be set separately

        CreateMap<CreateProductCommand, Product>();
        CreateMap<UpdateProductCommand, Product>();

        // Category mappings
        CreateMap<Category, CategoryDto>();

        // Inventory mappings
        CreateMap<InventoryItem, InventoryDto>()
            .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
            .ForMember(dest => dest.ProductCode,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Code : string.Empty));

        CreateMap<InventoryMovement, InventoryMovementDto>()
            .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
            .ForMember(dest => dest.MovementTypeName,
                opt => opt.MapFrom(src => src.MovementType.ToString()));

        // Order mappings
        CreateMap<Order, OrderDto>()
            .ForMember(dest => dest.StatusName,
                opt => opt.MapFrom(src => src.Status.ToString()))
            .ForMember(dest => dest.OrderTypeName,
                opt => opt.MapFrom(src => src.OrderType.ToString()))
            .ForMember(dest => dest.SupplierName,
                opt => opt.MapFrom(src => src.Supplier != null ? src.Supplier.Name : null));

        CreateMap<OrderItem, OrderItemDto>()
            .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Name : string.Empty))
            .ForMember(dest => dest.ProductCode,
                opt => opt.MapFrom(src => src.Product != null ? src.Product.Code : string.Empty));

        // User mappings
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.RoleName,
                opt => opt.MapFrom(src => src.Role.ToString()));
    }
}