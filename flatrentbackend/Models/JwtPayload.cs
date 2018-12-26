using System;
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
        public string EmployeeId { get; set; }
        public string ClientId { get; set; }
        public string UserType { get; set; }
        public string Department { get; set; }
        public string Position { get; set; }

        public static JwtPayload CreateFromUser(User user)
        {
            return new JwtPayload
            {
                UserId = user.Id.ToString(),
                UserType = user.Type?.Name,
                ClientId = user.ClientInformation?.Id.ToString(),
                EmployeeId = user.EmployeeInformation?.Id.ToString(),
                Department = user.EmployeeInformation?.Department,
                Position = user.EmployeeInformation?.Position,
            };
        }

        public Claim[] GetAsClaims()
        {
            var claims = GetType().GetProperties()
                .Select(property => property.GetValue(this) != null
                    ? new Claim(property.Name, property.GetValue(this).ToString())
                    : null).Where(claim => claim != null).Concat(new[]
                {
                    new Claim(ClaimTypes.Role, UserType),
                }).ToArray();
            if (Department != null)
            {
                claims = claims.Concat(new[] {new Claim(ClaimTypes.Role, Department)}).ToArray();
            }
            return claims;
        }

        public static JwtPayload CreateFromClaims(Claim[] claims)
        {
            var payload = new JwtPayload();
            foreach (var claim in claims)
            {
                var propertyName = claim.Type.Substring(3);
                var property = typeof(JwtPayload).GetProperties().FirstOrDefault(x => x.Name == propertyName);
                if (property == null) continue;
                try
                {
                    property.SetValue(payload, claim.Value);
                }
                catch (Exception e)
                {
                    Log.Error(e, $"Exception thrown while creating JWT Payload from claims at type {claim.Type} with value {claim.Value}");
                }
            }

            return payload;
        }
    }
}