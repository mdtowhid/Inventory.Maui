using AutoMapper;
using Inventory.Application.Products;
using Inventory.Application.Products;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Products.Queries;

public class GetLowStockProductsQuery : IRequest<List<ProductDto>>
{
    public int Threshold { get; set; } = 10;
}

public class GetLowStockProductsQueryHandler : IRequestHandler<GetLowStockProductsQuery, List<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetLowStockProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> Handle(GetLowStockProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetLowStockProductsAsync(request.Threshold);
        return _mapper.Map<List<ProductDto>>(products);
    }
}