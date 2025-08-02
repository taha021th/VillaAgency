using MongoDB.Bson.Serialization.Attributes;

namespace VillaAgency.Domain.Entities
{
    public class PropertyRequestReport
    {
        public string Content { get; set; }
        [BsonDateTimeOptions]
        public DateTime ReportDate { get; set; } = DateTime.UtcNow;
        public string AgentName { get; set; }
    }
}
