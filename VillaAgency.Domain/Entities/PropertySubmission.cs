using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VillaAgency.Domain.Entities
{

    public class PropertySubmission
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }

        // اطلاعات تماس مالک
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }

        // اطلاعات معامله
        public string TransactionType { get; set; } // e.g., "خرید و فروش", "رهن و اجاره"

        // **فیلدهای جدید اضافه شده از انتیتی Property**
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
        public string BuildDate { get; set; }
        public Guid CategoryId { get; set; }
        public List<string> ImageUrls { get; set; } = new();
        public List<string> VideoUrls { get; set; } = new();
        // اطلاعات سیستمی
        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending";
    }
}
