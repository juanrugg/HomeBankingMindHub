namespace HomeBankingMindHub.Services.Interfaces
{
    public interface IPasswordHasher
    {
        string Hash(string password);
        bool VerifyPassword(string passwordHash, string inputPassword);
    }
}
