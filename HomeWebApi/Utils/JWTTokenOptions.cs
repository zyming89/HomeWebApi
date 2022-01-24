namespace HomeWebApi.Utils
{
    public class JWTTokenOptions
    {
        public string? Audience { get; set; }
        public string SecurityKey { get; set; } = null!;

        public string? Issuer { get; set; }

    }
}
