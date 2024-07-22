using Microsoft.AspNetCore.Identity;

namespace FirstWebAPI_Application.Repository
{
    public interface ITokenRepository
    {
        string CreateJWTToken(IdentityUser user, List<string> roles);
    }
}
