using MediatR;
using Microsoft.AspNetCore.Identity;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Application.Handlers.ViewModels;
using VillaAgency.Domain.Entities.Users;

namespace VillaAgency.Application.Handlers.Properties.Queries
{
    public record GetHomePageDataQuery : IRequest<HomePageViewModel> { }

    public class GetHomePageDataQueryHandler : IRequestHandler<GetHomePageDataQuery, HomePageViewModel>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetHomePageDataQueryHandler(IPropertyRepository propertyRepository, ICategoryRepository categoryRepository, UserManager<ApplicationUser> userManager)
        {
            _propertyRepository=propertyRepository;
            _categoryRepository=categoryRepository;
            _userManager=userManager;
        }

        public async Task<HomePageViewModel> Handle(GetHomePageDataQuery request, CancellationToken cancellationToken)
        {
            var sliderPropertiesTask = _propertyRepository.GetSomeAsync(p => p.IsInSlider, p => p.CreatedAt, 6);
            var featuredPropertyTask = _propertyRepository.FindAsync(p => p.IsFeatured, cancellationToken);
            var totalPropertiesCountTask = _propertyRepository.CountAsync(_ => true);
            var latestPropertiesTask = _propertyRepository.GetSomeAsync(p => true, p => p.CreatedAt, 6);
            var agentsCount = (await _userManager.GetUsersInRoleAsync("Agent")).Count;


            var apartmentDealsTask = _propertyRepository.FindOneAsync(p => p.IsInApartment, cancellationToken);
            var villaDealsTask = _propertyRepository.FindOneAsync(p => p.IsInVilla, cancellationToken);
            var penthouseDealsTask = _propertyRepository.FindOneAsync(p => p.IsInPentHouse, cancellationToken);

            await Task.WhenAll(sliderPropertiesTask, featuredPropertyTask, totalPropertiesCountTask, latestPropertiesTask, apartmentDealsTask, villaDealsTask, penthouseDealsTask);

            return new HomePageViewModel
            {
                SliderProperties=await sliderPropertiesTask,
                FeaturedProperty=(await featuredPropertyTask).FirstOrDefault(),
                TotalPropertiesCount=(int)await totalPropertiesCountTask,
                TotalAgentsCount = agentsCount,
                LatestProperties=await latestPropertiesTask,
                ApartmentDeals=await apartmentDealsTask,
                PenthouseDeals=await penthouseDealsTask,
                VillaDeals=await villaDealsTask,

            };
        }
    }

}
