namespace VillaAgency.Infrastructure.Settings
{
    public record DatabaseSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }

    }
}
