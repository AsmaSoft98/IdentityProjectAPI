using System.Security.Claims;
using System.Text.Json;

namespace IdentityWasmProject.Authentication
{
    public static class StringExtension
    {
        public static IEnumerable<Claim> ParseClaimsFromJwt(this string jwt)
        {
            var claims = new List<Claim>();
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            if (keyValuePairs is null) return claims;

            keyValuePairs.TryGetValue(ClaimTypes.Role, out var roles);

            if (!string.IsNullOrWhiteSpace(roles?.ToString()))
            {
                var role = roles.ToString() ?? string.Empty;
                if (role.Trim().StartsWith("["))
                {
                    var parsedRoles = JsonSerializer.Deserialize<string[]>(role) ?? Array.Empty<string>();
                    claims.AddRange(parsedRoles.Select(pr => new Claim(ClaimTypes.Role, pr)).ToList());
                }
                else
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                keyValuePairs.Remove(ClaimTypes.Role);
            }

            claims.AddRange(keyValuePairs.Where(kvp => !string.IsNullOrWhiteSpace(kvp.Value.ToString()))
                                         .Select(kvp => new Claim(kvp.Key, kvp!.Value.ToString()!)));

            return claims;
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
