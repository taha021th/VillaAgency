using MediatR;
using Microsoft.AspNetCore.Identity;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Application.Handlers.ViewModels;
using VillaAgency.Domain.Entities.Users;

namespace VillaAgency.Application.Handlers.Dashboard.Queries
{
    public record GetDashboardStatsQuery : IRequest<DashboardViewModel>
    {
        public int PeriodInDays { get; set; } = 7;
    }
    public class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, DashboardViewModel>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IVisitRequestRepository _visitRequestRepository;
        private readonly IContactMessageRepository _messageRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetDashboardStatsQueryHandler(
            IPropertyRepository propertyRepository,
            ICategoryRepository categoryRepository,
            IVisitRequestRepository visitRequestRepository,
            IContactMessageRepository messageRepository,
            UserManager<ApplicationUser> userManager)
        {
            _propertyRepository = propertyRepository;
            _categoryRepository = categoryRepository;
            _visitRequestRepository = visitRequestRepository;
            _messageRepository = messageRepository;
            _userManager = userManager;
        }

        public async Task<DashboardViewModel> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
        {

            var startDate = DateTime.UtcNow.AddDays(-request.PeriodInDays);


            var totalPropertiesTask = _propertyRepository.CountAsync(p => true);
            var propertiesInPeriodTask = _propertyRepository.CountAsync(p => p.CreatedAt >= startDate);


            var totalUsers = _userManager.Users.Count();
            var usersInPeriod = _userManager.Users.Count(u => u.CreatedOn >= startDate);

            var totalVisitRequestsTask = _visitRequestRepository.CountAsync(v => true);
            var visitRequestsInPeriodTask = _visitRequestRepository.CountAsync(v => v.RequestDate >= startDate);
            var pendingVisitRequestsTask = _visitRequestRepository.CountAsync(v => v.Status == "Pending");

            var totalCategoriesTask = _categoryRepository.CountAsync(c => true);
            var unreadMessagesTask = _messageRepository.CountAsync(m => !m.IsRead);
            var recentVisitRequestsTask = _visitRequestRepository.GetSomeAsync(v => true, v => v.RequestDate, 5);


            await Task.WhenAll(
                totalPropertiesTask, propertiesInPeriodTask, totalVisitRequestsTask,
                visitRequestsInPeriodTask, pendingVisitRequestsTask, totalCategoriesTask,
                unreadMessagesTask, recentVisitRequestsTask
            );


            var viewModel = new DashboardViewModel
            {
                TotalProperties = (int)await totalPropertiesTask,
                PropertiesRegisteredInPeriod = (int)await propertiesInPeriodTask,
                TotalUsers = totalUsers,
                UsersRegisteredInPeriod = usersInPeriod,
                TotalVisitRequests = (int)await totalVisitRequestsTask,
                VisitRequestsInPeriod = (int)await visitRequestsInPeriodTask,
                PendingVisitRequests = (int)await pendingVisitRequestsTask,
                PeriodInDays = request.PeriodInDays,
                TotalCategories = (int)await totalCategoriesTask,
                UnreadMessages = (int)await unreadMessagesTask,
                RecentVisitRequests = (await recentVisitRequestsTask).ToList()
            };

            return viewModel;
        }
    }
}
