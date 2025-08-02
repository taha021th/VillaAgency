using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VillaAgency.Domain.Entities
{
    public class PropertyRequest
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string TransactionType { get; set; }
        public List<Guid> CategoryIds { get; set; } = new(); // شناسه دسته‌بندی‌های مورد نظر
        public string DesiredRegions { get; set; } // مناطق مورد نظر

        public decimal? MinBudget { get; set; }
        public decimal? MaxBudget { get; set; }
        public int? MaxArea { get; set; }
        public int? MinArea { get; set; }
        public int? MinBedrooms { get; set; }
        public string? Description { get; set; }
        public List<PropertyRequestReport> Reports { get; set; } = new();

        [BsonDateTimeOptions(Kind = DateTimeKind.Utc)]
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Pending";
    }
}
