using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;

namespace VillaAgency.Application.Handlers.Properties.Commands
{
    public record UpdatePropertyCommand : IRequest
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public int Area { get; set; }
        public int Bedrooms { get; set; }
        public int Floor { get; set; }
        public int FloorsCount { get; set; }
        public int UnitsCountInFloor { get; set; }
        public int Unit { get; set; }
        public string BuildDate { get; set; }
        public List<string>? ImageUrls { get; set; } = null;
        public List<string>? VideoUrls { get; set; } = null;
        public Guid CategoryId { get; set; }
        public string TransactionType { get; set; }


    }
    public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly ICategoryRepository _categoryRepository;

        public UpdatePropertyCommandHandler(IPropertyRepository propertyRepository, ICategoryRepository categoryRepository)
        {
            _propertyRepository = propertyRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
        {

            var propertyToUpdate = await _propertyRepository.GetByIdAsync(request.Id, cancellationToken);
            if (propertyToUpdate == null)
            {
                throw new Exception("Property not found");
            }


            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null)
            {
                throw new Exception("Category not found");
            }

            propertyToUpdate.FullName = request.FullName;
            propertyToUpdate.PhoneNumber = request.PhoneNumber;
            propertyToUpdate.Title = request.Title;
            propertyToUpdate.Description = request.Description;
            propertyToUpdate.Price = request.Price;
            propertyToUpdate.Area = request.Area;
            propertyToUpdate.Bedrooms = request.Bedrooms;
            propertyToUpdate.Floor = request.Floor;
            propertyToUpdate.FloorsCount = request.FloorsCount;
            propertyToUpdate.UnitsCountInFloor = request.UnitsCountInFloor;
            propertyToUpdate.Unit = request.Unit;
            propertyToUpdate.BuildDate = request.BuildDate;
            propertyToUpdate.Address = request.Address;
            propertyToUpdate.CategoryId = request.CategoryId;
            propertyToUpdate.CategoryName = category.Name;
            propertyToUpdate.ImageUrls = request.ImageUrls??new List<string>();
            propertyToUpdate.VideoUrls=request.VideoUrls??new List<string>();
            propertyToUpdate.TransactionType = request.TransactionType;


            await _propertyRepository.UpdateAsync(propertyToUpdate, cancellationToken);


        }


    }
}
