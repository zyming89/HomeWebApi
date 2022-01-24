namespace HomeWebApi.Utils
{
    public interface IConstomJWTService
    {
        string GetToken(string Code, string Password);
    }
}
