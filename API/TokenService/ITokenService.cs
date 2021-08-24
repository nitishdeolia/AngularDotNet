using API.Models;

namespace API.TokenServices
{
    public interface ITokenService
    {
        string CreateToken(AppUser user);
    }
}