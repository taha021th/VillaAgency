using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace VillaAgency.Domain.Entities
{

    public class Property
    {
        [BsonId]
        [BsonRepresentation(BsonType.String)]
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public Guid CategoryId { get; set; }
        public string CategoryName { get; set; }
        public Guid AgentId { get; set; }
        public string AgentName { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; }
        public string Address { get; set; }
        public string TransactionType { get; set; } // e.g., "خرید و فروش", "رهن و اجاره"

        /// <summary>
        /// متراژ
        /// </summary>
        public int Area { get; set; }

        /// <summary>
        /// تعداد اتاق خواب
        /// </summary>
        public int Bedrooms { get; set; }

        /// <summary>
        /// طبقه
        /// </summary>
        public int Floor { get; set; }

        /// <summary>
        /// تعداد طبقات
        /// </summary>
        public int FloorsCount { get; set; }
        /// <summary>
        /// تعداد واحد در هر طبقه
        /// </summary>
        public int UnitsCountInFloor { get; set; }
        /// <summary>
        /// شماره واحد
        /// </summary>
        public int Unit { get; set; }
        /// <summary>
        /// سال ساخت
        /// </summary>
        public string BuildDate { get; set; } = string.Empty;
        public List<string> ImageUrls { get; set; }
        public List<string> VideoUrls { get; set; }
        public bool IsInSlider { get; set; } = false;
        public bool IsFeatured { get; set; } = false;
        public bool IsInVilla { get; set; } = false;
        public bool IsInApartment { get; set; } = false;
        public bool IsInPentHouse { get; set; } = false;
    }
}
