using Archer.Extension.Models;

namespace Archer.Extension.JwtHelper
{
    public interface IJwtHelper
    {
        string GenerateToken(TokenModel tokenModel, int expireMinutes = 480);

        bool ValidateToken(string authToken);
    }
}
