using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using FlatRent.Entities;
using Microsoft.EntityFrameworkCore.Design;
using Serilog;

namespace FlatRent.Models
{
    public class JwtPayload
    {
        public string UserId { get; set; }
        public UserType UserType { get; set; }

        public static JwtPayload CreateFromUser(User user)
        {
            return new JwtPayload
            {
                UserId = user.Id.ToString(),
                UserType = user.Type,
            };
        }

        public Claim[] GetAsClaims()
        {
//            var claims = GetType().GetProperties()
//                .Select(property => property.GetValue(this) != null
//                    ? new Claim(property.Name, property.GetValue(this).ToString())
//                    : null).Where(claim => claim != null).Concat(new[]
//                {
//                    new Claim(ClaimTypes.Role, UserType.Id.ToString()),
//                }).ToArray();
            var claims = new[]
            {
                new Claim(nameof(UserId), UserId),
                new Claim(ClaimTypes.Role, UserType.Id.ToString()),
            };
            return claims;
        }

        public static JwtPayload CreateFromClaims(IEnumerable<Claim> claims)
        {
            var payload = new JwtPayload();
//            foreach (var claim in claims)
//            {
//                var propertyName = claim.Type.Substring(3);
//                var property = typeof(JwtPayload).GetProperties().FirstOrDefault(x => x.Name == propertyName);
//                if (property == null) continue;
//                try
//                {
//                    property.SetValue(payload, claim.Value);
//                }
//                catch (Exception e)
//                {
//                    Log.Error(e, $"Exception thrown while creating JWT Payload from claims at type {claim.Type} with value {claim.Value}");
//                }
//            }

            var userId = claims.First(c => c.Type.Substring(3) == nameof(UserId)).Value;
            payload.UserId = userId;
            var userTypeId = claims.First(c => c.Type == ClaimTypes.Role).Value;
            payload.UserType = UserType.ExistingTypes.First(us => us.Id == int.Parse(userTypeId));

            return payload;
        }
    }
}