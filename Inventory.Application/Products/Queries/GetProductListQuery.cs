using AutoMapper;
using Inventory.Application.Products;
using Inventory.Application.Products;
using Inventory.Domain.Entities;
using Inventory.Domain.Interfaces;
using MediatR;

namespace Inventory.Application.Products.Queries;

public class GetProductListQuery : IRequest<PagedResult<ProductDto>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SearchTerm { get; set; }
    public int? CategoryId { get; set; }
    public bool? IsActive { get; set; }
    public string? SortBy { get; set; }
    public bool SortDescending { get; set; } = false;
}

public class GetProductListQueryHandler : IRequestHandler<GetProductListQuery, PagedResult<ProductDto>>
{
    private readonly IProductRepository _productRepository;
    private readonly IMapper _mapper;

    public GetProductListQueryHandler(IProductRepository productRepository, IMapper mapper)
    {
        _productRepository = productRepository;
        _mapper = mapper;
    }

    public async Task<PagedResult<ProductDto>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
    {
        IEnumerable<Product> products;

        if (!string.IsNullOrEmpty(request.SearchTerm))
        {
            products = await _productRepository.SearchProductsAsync(request.SearchTerm);
        }
        else if (request.CategoryId.HasValue)
        {
            products = await _productRepository.GetProductsByCategoryAsync(request.CategoryId.Value);
        }
        else
        {
            products = await _productRepository.GetAllAsync();
        }

        if (request.IsActive.HasValue)
        {
            products = products.Where(p => p.IsActive == request.IsActive.Value);
        }

        var productList = products.ToList();
        if (!string.IsNullOrEmpty(request.SortBy))
        {
            var property = typeof(Product).GetProperty(request.SortBy);
            if (property != null)
            {
                productList = request.SortDescending
                    ? productList.OrderByDescending(p => property.GetValue(p)).ToList()
                    : productList.OrderBy(p => property.GetValue(p)).ToList();
            }
        }

        var totalCount = productList.Count;
        var pagedItems = productList
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new PagedResult<ProductDto>
        {
            Items = _mapper.Map<List<ProductDto>>(pagedItems),
            TotalCount = totalCount,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalPages = (int)Math.Ceiling(totalCount / (double)request.PageSize)
        };
    }
}