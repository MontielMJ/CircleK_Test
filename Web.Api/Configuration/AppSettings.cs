namespace Web.Api.Configuration
{
    public class JwtSettings
    {
        public string SecretKey { get; set; } = string.Empty;
        public string Issuer { get; set; } = string.Empty;
        public string Audience { get; set; } = string.Empty;
        public int ExpirationHours { get; set; } = 24;
    }

    public class ApiSettings
    {
        public int MaxPageSize { get; set; } = 100;
        public int DefaultPageSize { get; set; } = 10;
        public int MaxItemsPerSale { get; set; } = 50;
        public int MaxPaymentsPerTransaction { get; set; } = 10;
    }
}