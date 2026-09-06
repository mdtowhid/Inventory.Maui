using AutoMapper;
using FluentValidation;
using Inventory.Domain.Entities;
using InventorySystem.Application.Products;
using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Interfaces;
using MediatR;

namespace InventorySystem.Application.Products.Commands;

public class CreateProductCommand : IRequest<ProductDto>
{
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? CategoryId { get; set; }
    public int? SupplierId { get; set; }
    public decimal UnitPrice { get; set; }
    public int MinStock { get; set; } = 0;
    public int MaxStock { get; set; } = 100;
    public string? Barcode { get; set; }
    public string? SKU { get; set; }
    public decimal? Weight { get; set; }
    public string? UnitOfMeasure { get; set; }
    public int? UserId { get; set; }
}

public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        // Check if product code already exists
        var existingProduct = await _productRepository.GetByCodeAsync(request.Code);
        if (existingProduct != null)
        {
            throw new InvalidOperationException($"Product with code '{request.Code}' already exists.");
        }

        var product = _mapper.Map<Product>(request);
        product.CreatedAt = DateTime.UtcNow;
        product.CreatedBy = request.UserId;

        await _unitOfWork.BeginTransactionAsync();
        try
        {
            var productId = await _productRepository.AddAsync(product);
            product.Id = productId;

            // Create initial inventory record
            var inventoryItem = new InventoryItem
            {
                ProductId = productId,
                Quantity = 0,
                ReservedQuantity = 0,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.UserId
            };

            var inventoryRepository = _unitOfWork.GetRepository<InventoryItem>();
            await inventoryRepository.AddAsync(inventoryItem);

            await _unitOfWork.CommitAsync();
        }
        catch
        {
            await _unitOfWork.RollbackAsync();
            throw;
        }

        return _mapper.Map<ProductDto>(product);
    }
}

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.UnitPrice)
            .GreaterThanOrEqualTo(0);
    }
}