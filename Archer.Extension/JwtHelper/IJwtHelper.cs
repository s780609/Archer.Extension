using Archer.Extension.Models;
using Microsoft.IdentityModel.Tokens;

namespace Archer.Extension.JwtHelper
{
    public interface IJwtHelper
    {
        public string GenerateToken(TokenModel tokenModel, int expireMinutes = 480);

        public bool ValidateToken(string authToken);
    }
}
