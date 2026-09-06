using AutoMapper;
using Inventory.Application.Common.Exceptions;
using Inventory.Application.Products;
using Inventory.Application.Products;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Products.Queries;

public class GetProductDetailQuery : IRequest<ProductDto>
{
    public int Id { get; set; }
}

public class GetProductDetailQueryHandler : IRequestHandler<GetProductDetailQuery, ProductDto>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductDetailQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<ProductDto> Handle(GetProductDetailQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id);
        if (product == null)
        {
            throw new NotFoundException(nameof(Product), request.Id);
        }

        return _mapper.Map<ProductDto>(product);
    }
}