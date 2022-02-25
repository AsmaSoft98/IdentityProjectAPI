namespace identityProjectAPI.Infrastructure
{
    public class AppConfiguration
    {
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Key { get; set; }
        public int ExpiryInDays { get; set; }
    }
}
