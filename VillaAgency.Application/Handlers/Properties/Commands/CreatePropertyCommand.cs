using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Handlers.Properties.Commands
{
    public record CreatePropertyCommand : IRequest<Guid>
    {
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
        public Guid CategoryId { get; set; }
        public Guid AgentId { get; set; }
        public string? AgentName { get; set; }
        public List<string>? ImageUrls { get; set; }
        public List<string>? VideoUrls { get; set; }
        public string BuildDate { get; set; } = string.Empty;
        public string TransactionType { get; set; }


    }
    public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, Guid>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly ICategoryRepository _categoryRepository;


        public CreatePropertyCommandHandler(IPropertyRepository propertyRepository, ICategoryRepository categoryRepository)
        {
            _propertyRepository = propertyRepository;
            _categoryRepository = categoryRepository;

        }

        public async Task<Guid> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);
            if (category == null) throw new Exception("Category not found");
            var property = new Property
            {
                Id = Guid.NewGuid(), // ایجاد یک شناسه جدید
                FullName=request.FullName,
                PhoneNumber=request.PhoneNumber,
                Title = request.Title,
                Price = request.Price,
                Description = request.Description,
                Address = request.Address,
                Area = request.Area,
                Bedrooms = request.Bedrooms,
                Floor = request.Floor,
                FloorsCount = request.FloorsCount,
                UnitsCountInFloor = request.UnitsCountInFloor,
                Unit = request.Unit,
                BuildDate =request.BuildDate,
                CategoryId = request.CategoryId,
                CategoryName = category.Name,
                AgentId=request.AgentId,
                AgentName=request.AgentName,
                ImageUrls = request.ImageUrls??new List<string>(),
                VideoUrls=request.VideoUrls??new List<string>(),
                TransactionType =request.TransactionType

            };

            await _propertyRepository.AddAsync(property, cancellationToken);
            return property.Id;
        }
    }
}
