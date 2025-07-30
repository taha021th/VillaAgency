using System.Linq.Expressions;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Common.Interfaces.Repositories
{
    public interface IContactMessageRepository
    {
        Task AddAsync(ContactMessage message);
        Task<IEnumerable<ContactMessage>> GetAllAsync();
        Task<long> CountAsync(Expression<Func<ContactMessage, bool>> filter);
        Task MarkAllAsReadAsync();
    }
}
