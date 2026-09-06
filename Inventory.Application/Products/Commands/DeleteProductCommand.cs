using Inventory.Application.Common.Exceptions;
using InventorySystem.Domain.Entities;
using InventorySystem.Domain.Interfaces;
using MediatR;

namespace InventorySystem.Application.Products.Commands;

public class DeleteProductCommand : IRequest<bool>
{
    public int Id { get; set; }
}

public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, bool>
{
    private readonly IProductRepository _productRepository;

    public DeleteProductCommandHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<bool> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);
        if (product == null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }

        return await _productRepository.DeleteAsync(product);
    }
}