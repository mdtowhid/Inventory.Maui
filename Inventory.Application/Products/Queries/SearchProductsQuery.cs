using AutoMapper;
using Inventory.Application.Products;
using InventorySystem.Application.Products;
using InventorySystem.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Products.Queries;

public class SearchProductsQuery : IRequest<List<ProductDto>>
{
    public string SearchTerm { get; set; } = string.Empty;
}

public class SearchProductsQueryHandler : IRequestHandler<SearchProductsQuery, List<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public SearchProductsQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<List<ProductDto>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.SearchProductsAsync(request.SearchTerm);
        return _mapper.Map<List<ProductDto>>(products);
    }
}