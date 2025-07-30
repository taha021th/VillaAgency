namespace VillaAgency.Domain.Entities
{
    public class AgentReport
    {
        public Guid Id { get; set; }
        public string Notes { get; set; }
        public DateTime ReportDate { get; set; }
    }
}
