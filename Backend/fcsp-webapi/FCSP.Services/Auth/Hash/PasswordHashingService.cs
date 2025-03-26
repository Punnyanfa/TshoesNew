using Microsoft.AspNet.Identity;

namespace FCSP.Services.Auth.Hash;

public class PasswordHashingService : IPasswordHashingService
{
    private readonly IPasswordHasher _passwordHasher;

    public PasswordHashingService(IPasswordHasher passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    public string GetHashedPassword(string password)
    {
        return _passwordHasher.HashPassword(password);
    }

    public bool VerifyHashedPassword(string password, string hashedPassword)
    {
        var result = _passwordHasher.VerifyHashedPassword(hashedPassword, password);

        return result == PasswordVerificationResult.Success;
    }
}
