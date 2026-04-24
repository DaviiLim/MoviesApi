using System.Security.Claims;

namespace Api.Extensions
{
    public static class TokenClaimHelper
    {
        public static int GetUserIdFromToken(this ClaimsPrincipal user)
        {
            var claim = user.FindFirst(ClaimTypes.NameIdentifier);
            if (claim == null)
            {
                return 0;
            }
            return int.Parse(claim.Value);
        }
    }
}
