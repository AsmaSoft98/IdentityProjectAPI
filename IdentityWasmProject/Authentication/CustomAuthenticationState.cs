using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace IdentityWasmProject.Authentication
{
    public class CustomAuthenticationState : AuthenticationStateProvider
    {
        private readonly HttpClient _httpClient;
        private readonly ILocalStorageService _localStorage;
        public CustomAuthenticationState(HttpClient httpClient, ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _localStorage = localStorage;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (await _localStorage.ContainKeyAsync("token"))
            {
                // user logged in 
                var tokenAsString = await _localStorage.GetItemAsStringAsync("token");
                var tokenHandler = new JwtSecurityTokenHandler();

                var token1 = tokenHandler.ReadJwtToken(tokenAsString);

                var identity1 = new ClaimsIdentity(token1.Claims, "Bearer");
                var user1 = new ClaimsPrincipal(identity1);

                var authState1 = new AuthenticationState(user1);

                NotifyAuthenticationStateChanged(Task.FromResult(authState1)); // broadcast auth state - blazor components can subscribe to this

                return authState1;
            }
            return new AuthenticationState(new ClaimsPrincipal()); // empty claimsPrincipal means user not logged in
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("token");

            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }
    }
}
