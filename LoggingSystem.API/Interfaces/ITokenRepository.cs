using Microsoft.AspNetCore.Identity;

namespace LoggingSystem.API.Interfaces
{
    public interface ITokenRepository
    {
        string CreateJwtToken(IdentityUser user, List<string> roles);
    }
}
