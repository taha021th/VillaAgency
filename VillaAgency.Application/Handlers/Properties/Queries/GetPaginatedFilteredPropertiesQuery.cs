using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;
namespace VillaAgency.Application.Handlers.Properties.Queries;


public class GetPaginatedFilteredPropertiesQuery : IRequest<(IEnumerable<Property> Properties, int TotalCount)>
{
    public string? SearchTerm { get; set; }
    public Guid? CategoryId { get; set; } // <<<< این خط جایگزین Type شد
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public int? MinArea { get; set; }
    public int? MaxArea { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 6;
}

public class GetPaginatedFilteredPropertiesQueryHandler : IRequestHandler<GetPaginatedFilteredPropertiesQuery, (IEnumerable<Property> Properties, int TotalCount)>
{
    // Handler اکنون به ریپازیتوری وابسته است، نه DbContext
    private readonly IPropertyRepository _propertyRepository;

    public GetPaginatedFilteredPropertiesQueryHandler(IPropertyRepository propertyRepository)
    {
        _propertyRepository = propertyRepository;
    }

    public async Task<(IEnumerable<Property> Properties, int TotalCount)> Handle(GetPaginatedFilteredPropertiesQuery request, CancellationToken cancellationToken)
    {
        // فراخوانی متد جستجو از ریپازیتوری
        return await _propertyRepository.SearchAndPaginateAsync(
            request.SearchTerm,
            request.CategoryId,
            request.MinPrice,
            request.MaxPrice,
            request.MinArea,
            request.MaxArea,
            request.PageNumber,
            request.PageSize,
            cancellationToken
        );
    }
}

