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
        private readonly IPropertyRequestRepository _propertyRequestRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IVisitRequestRepository _visitRequestRepository;
        private readonly IContactMessageRepository _messageRepository;
        private readonly IPropertySubmissionRepository _propertySubmissionRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetDashboardStatsQueryHandler(
            IPropertyRepository propertyRepository,
            ICategoryRepository categoryRepository,
            IVisitRequestRepository visitRequestRepository,
            IContactMessageRepository messageRepository,
            IPropertySubmissionRepository propertySubmissionRepository,
            IPropertyRequestRepository propertyRequestRepository,
            UserManager<ApplicationUser> userManager
            )
        {
            _propertyRepository = propertyRepository;
            _categoryRepository = categoryRepository;
            _visitRequestRepository = visitRequestRepository;
            _messageRepository = messageRepository;
            _propertySubmissionRepository = propertySubmissionRepository;
            _propertyRequestRepository = propertyRequestRepository;
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

            var totalSubmitPropertiesTask = _propertySubmissionRepository.CountAsync(p => true);
            var pendingSubmitPropertiesTask = _propertySubmissionRepository.CountAsync(p => p.Status=="Pending");
            var recentPendingSubmissionPropertiesTask = _propertySubmissionRepository.GetSomeAsync(p => p.Status=="Pending", p => p.CreatedAt, 5);

            var totalRequestPropertiesTask = _propertyRequestRepository.CountAsync(p => true);
            var pendingRequestPropertiesTask = _propertyRequestRepository.CountAsync(p => p.Status=="Pending");
            var recentPendingRequestPropertiesTask = _propertyRequestRepository.GetSomeAsync(p => p.Status=="Pending", p => p.CreatedAt, 5);


            var totalCategoriesTask = _categoryRepository.CountAsync(c => true);
            var unreadMessagesTask = _messageRepository.CountAsync(m => !m.IsRead);
            var recentPendingVisitRequestsTask = _visitRequestRepository.GetSomeAsync(v => v.Status=="Pending", v => v.RequestDate, 5);



            await Task.WhenAll(
                totalPropertiesTask, propertiesInPeriodTask, totalVisitRequestsTask,
                visitRequestsInPeriodTask, pendingVisitRequestsTask, totalCategoriesTask,
                unreadMessagesTask, recentPendingVisitRequestsTask, totalSubmitPropertiesTask, pendingSubmitPropertiesTask,
                totalRequestPropertiesTask, pendingRequestPropertiesTask

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
                RecentVisitRequests = (await recentPendingVisitRequestsTask).ToList(),
                TotalSubmitProperties = (int)await totalSubmitPropertiesTask,
                PendingSubmitProperties = (int)await pendingSubmitPropertiesTask,
                TotalRequestProperties = (int)await totalRequestPropertiesTask,
                PendingRequestProperties = (int)await pendingRequestPropertiesTask,
                PropertyRequests=(await recentPendingRequestPropertiesTask).ToList(),
                PropertySubmissions=(await recentPendingSubmissionPropertiesTask).ToList()
            };

            return viewModel;
        }
    }
}
