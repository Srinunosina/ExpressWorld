using ExpressWorld.Application.DTOs;
using ExpressWorld.Application.Interfaces;
using MediatR;

namespace ExpressWorld.Application.Handlers
{
    //public class SearchProductsHandler : IRequestHandler<SearchProductsQuery, IEnumerable<ProductDTO>>
    //{
    //    private readonly IDataSourceManager _dataSourceManager;

    //    public SearchProductsHandler(IDataSourceManager dataSourceManager)
    //    {
    //        _dataSourceManager = dataSourceManager;
    //    }

    //    public async Task<IEnumerable<ProductDTO>> Handle(SearchProductsQuery request, CancellationToken cancellationToken)
    //    {
    //        var products = await _dataSourceManager.GetProductsAsync(request.Filters);
    //        return products.Select(p => new ProductDTO
    //        {
    //            Id = p.Id,
    //            Name = p.Name,
    //            Price = p.Price,
    //            AvailabilityStatus = p.AvailabilityStatus
    //        });
    //    }
    //}
}
