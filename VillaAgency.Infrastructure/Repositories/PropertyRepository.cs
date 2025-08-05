using MongoDB.Driver;
using System.Linq.Expressions;
using VillaAgency.Application.Common.Interfaces;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Infrastructure.Repositories;

public class PropertyRepository : IPropertyRepository
{
    private readonly IApplicationDbContext _context;

    public PropertyRepository(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Property entity, CancellationToken cancellationToken)
    {
        await _context.Properties.InsertOneAsync(entity, null, cancellationToken);

    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var filter = Builders<Property>.Filter.Eq(p => p.Id, id);
        await _context.Properties.DeleteOneAsync(filter, cancellationToken);
    }

    public async Task<IEnumerable<Property>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Properties.Find(_ => true).ToListAsync(cancellationToken);
    }

    public async Task<Property> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        var filter = Builders<Property>.Filter.Eq(p => p.Id, id);
        return await _context.Properties.Find(filter).FirstOrDefaultAsync(cancellationToken);
    }
    public async Task UpdateAsync(Property entity, CancellationToken cancellationToken)
    {
        var filter = Builders<Property>.Filter.Eq(p => p.Id, entity.Id);
        await _context.Properties.ReplaceOneAsync(filter, entity, new ReplaceOptions(), cancellationToken);
    }

    public async Task<IEnumerable<Property>> GetPropertiesByAgentIdAsync(Guid agentId)
    {
        return await _context.Properties.Find(p => p.AgentId==agentId).ToListAsync();
    }

    public async Task<long> CountAsync()
    {
        return await _context.Properties.CountDocumentsAsync(_ => true);
    }

    public async Task<long> CountAsync(Expression<Func<Property, bool>> filter)
    {
        return await _context.Properties.CountDocumentsAsync(filter);
    }

    public async Task<IEnumerable<Property>> GetSomeAsync<TKey>(Expression<Func<Property, bool>> filter, Expression<Func<Property, TKey>> keySelector, int count, bool descending = true)
    {
        var sortDefinition = new ExpressionFieldDefinition<Property, TKey>(keySelector);

        var sort = descending
            ? Builders<Property>.Sort.Descending(sortDefinition)
            : Builders<Property>.Sort.Ascending(sortDefinition);

        return await _context.Properties.Find(filter).Sort(sort).Limit(count).ToListAsync();
    }

    public async Task<(IEnumerable<Property> Properties, int TotalCount)> GetAllPaginatedAsync(int pageNumber, int pageSize)
    {
        var count = await _context.Properties.CountDocumentsAsync(_ => true);
        var items = await _context.Properties.Find(_ => true)
            .SortByDescending(p => p.CreatedAt)
            .Skip((pageNumber-1)*pageSize)
            .Limit(pageSize)
            .ToListAsync();
        return (items, (int)count);

    }
    // متد SearchAsync قدیمی حذف و این متد جایگزین می‌شود
    public async Task<(IEnumerable<Property> Properties, int TotalCount)> SearchAndPaginateAsync(
        string? searchTerm, Guid? categoryId, decimal? minPrice, decimal? maxPrice,
        int? minArea, int? maxArea, int pageNumber, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var builder = Builders<Property>.Filter;
        var filter = builder.Empty;

        // 1. ساخت فیلتر دقیقا مانند قبل
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var textSearchOptions = new TextSearchOptions { Language = "none", CaseSensitive = false };
            filter &= builder.Text(searchTerm, textSearchOptions);
        }
        if (categoryId.HasValue) { filter &= builder.Eq(p => p.CategoryId, categoryId.Value); }
        if (minPrice.HasValue) { filter &= builder.Gte(p => p.Price, minPrice.Value); }
        if (maxPrice.HasValue) { filter &= builder.Lte(p => p.Price, maxPrice.Value); }
        if (minArea.HasValue) { filter &= builder.Gte(p => p.Area, minArea.Value); }
        if (maxArea.HasValue) { filter &= builder.Lte(p => p.Area, maxArea.Value); }

        // 2. شمارش تعداد کل اسناد با فیلتر اعمال شده (قبل از صفحه‌بندی)
        var totalCount = await _context.Properties.CountDocumentsAsync(filter, null, cancellationToken);

        // 3. اعمال صفحه‌بندی و دریافت داده‌های صفحه فعلی
        var properties = await _context.Properties.Find(filter)
            .SortByDescending(p => p.CreatedAt) // مرتب‌سازی برای نتایج یکنواخت
            .Skip((pageNumber - 1) * pageSize)
            .Limit(pageSize)
            .ToListAsync(cancellationToken);

        return (properties, (int)totalCount);
    }

    public async Task<IEnumerable<Property?>> FindAsync(Expression<Func<Property, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _context.Properties.Find(predicate).ToListAsync(cancellationToken);
    }

    public async Task<Property?> FindOneAsync(Expression<Func<Property, bool>> predicate, CancellationToken cancellationToken)
    {
        return await _context.Properties.Find(predicate).FirstOrDefaultAsync(cancellationToken);
    }
}

