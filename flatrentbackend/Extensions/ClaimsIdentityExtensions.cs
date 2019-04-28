using System;
using System.Linq;
using System.Security.Claims;

namespace FlatRent.Extensions
{
    public static class ClaimsIdentityExtensions
    {
        public static Guid GetUserId(this ClaimsPrincipal user)
        {
            var claim = user.Claims.FirstOrDefault(x => x.Type == "UserId");
            if (claim?.Value == null) return Guid.Empty;
            return Guid.TryParse(claim.Value, out var userId) ? userId : Guid.Empty;
        }
    }
}