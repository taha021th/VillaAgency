using MongoDB.Driver;
using System.Linq.Expressions;
using VillaAgency.Application.Common.Interfaces;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Infrastructure.Repositories
{
    public class ContactMessageRepository : IContactMessageRepository
    {
        private readonly IApplicationDbContext _context;

        public ContactMessageRepository(IApplicationDbContext context)
        {
            _context=context;
        }
        public async Task AddAsync(ContactMessage message)
        {
            await _context.ContactMessages.InsertOneAsync(message);
        }

        public async Task<long> CountAsync(Expression<Func<ContactMessage, bool>> filter)
        {
            return await _context.ContactMessages.CountDocumentsAsync(filter);
        }

        public async Task<long> CountUnreadAsync()
        {
            return await _context.ContactMessages.CountDocumentsAsync(m => !m.IsRead);
        }

        public async Task<IEnumerable<ContactMessage>> GetAllAsync()
        {
            return await _context.ContactMessages.Find(_ => true).SortByDescending(m => m.CreatedAt).ToListAsync();
        }

        public async Task MarkAllAsReadAsync()
        {
            var filter = Builders<ContactMessage>.Filter.Eq(m => m.IsRead, false);
            var update = Builders<ContactMessage>.Update.Set(m => m.IsRead, true);

            // UpdateManyAsync تمام اسناد مطابق با فیلتر را به‌روزرسانی می‌کند.
            await _context.ContactMessages.UpdateManyAsync(filter, update);
        }
    }
}
