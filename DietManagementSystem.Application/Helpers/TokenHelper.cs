using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DietManagementSystem.Application.Helpers;

internal class TokenHelper
{
    public static Guid GetUserIdFromToken(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);

        // Common claim types for user id: "sub" or "nameid"
        var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier || c.Type == "sub");

        if (userIdClaim == null)
        {
            throw new Exception("UserId claim not found in token");
        }

        return Guid.Parse(userIdClaim.Value.ToString());
    }

}
