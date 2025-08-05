using MediatR;
using VillaAgency.Application.Common.Interfaces.Repositories;
using VillaAgency.Domain.Entities;

namespace VillaAgency.Application.Handlers.PropertySubmissions.Commands
{
    public class CreatePropertySubmissionCommand : IRequest<bool>
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string TransactionType { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public decimal Price { get; set; }
        public int Area { get; set; }
        public int Bedrooms { get; set; }
        public int Floor { get; set; }
        public int FloorsCount { get; set; }
        public int Unit { get; set; }
        public int UnitsCountInFloor { get; set; }
        public string BuildDate { get; set; } = string.Empty;
        public List<string> ImageUrls { get; set; } = new();
        public List<string> VideoUrls { get; set; } = new();
        public Guid CategoryId { get; set; }

    }
    public class CreatePropertySubmissionCommandHandler : IRequestHandler<CreatePropertySubmissionCommand, bool>
    {
        private readonly IPropertySubmissionRepository _propertySubmissionRepository;
        public CreatePropertySubmissionCommandHandler(IPropertySubmissionRepository propertySubmissionRepository)
        {
            _propertySubmissionRepository=propertySubmissionRepository;
        }

        public async Task<bool> Handle(CreatePropertySubmissionCommand request, CancellationToken cancellationToken)
        {
            var submission = new PropertySubmission
            {
                Id=Guid.NewGuid(),
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                TransactionType = request.TransactionType,
                Title = request.Title,
                Description = request.Description,
                Address = request.Address,
                Price = request.Price,
                Area = request.Area,
                Bedrooms = request.Bedrooms,
                Floor = request.Floor,
                FloorsCount = request.FloorsCount,
                Unit = request.Unit,
                UnitsCountInFloor = request.UnitsCountInFloor,
                BuildDate = request.BuildDate,
                CategoryId = request.CategoryId,
                ImageUrls = request.ImageUrls,
                VideoUrls = request.VideoUrls,


            };


            await _propertySubmissionRepository.AddAsync(submission, cancellationToken);
            return true;


        }
    }
}

