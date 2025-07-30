using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VillaAgency.Domain.Entities
{
    public class VisitRequest
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public Guid PropertyId { get; set; }
        public string PropertyName { get; set; }
        public Guid AgentId { get; set; }
        public string UserName { get; set; }
        public string UserPhone { get; set; }
        public string UserEmail { get; set; }
        public DateTime RequestDate { get; set; }
        public string Status { get; set; }
        public List<AgentReport> Reports { get; set; } = new List<AgentReport>();
    }
}
