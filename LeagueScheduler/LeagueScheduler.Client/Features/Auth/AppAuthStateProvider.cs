using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace LeagueScheduler.Client.Features.Auth
{
    public class AppAuthStateProvider : AuthenticationStateProvider
    {
        private const string StorageKey = "auth_token";
        private readonly IJSRuntime _js;
        private static readonly AuthenticationState _anonymous =
            new(new ClaimsPrincipal(new ClaimsIdentity()));

        public string? Token { get; private set; }

        public AppAuthStateProvider(IJSRuntime js) => _js = js;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            Token = await _js.InvokeAsync<string?>("localStorage.getItem", StorageKey);

            if (string.IsNullOrWhiteSpace(Token))
                return _anonymous;

            var principal = ParseToken(Token);
            return principal is null ? _anonymous : new AuthenticationState(principal);
        }

        public async Task NotifyUserLoggedIn(string token)
        {
            Token = token;
            await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, token);
            var principal = ParseToken(token)!;
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        public async Task NotifyUserLoggedOut()
        {
            Token = null;
            await _js.InvokeVoidAsync("localStorage.removeItem", StorageKey);
            NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
        }

        private static ClaimsPrincipal? ParseToken(string token)
        {
            try
            {
                var parts = token.Split('.');
                if (parts.Length != 3) return null;

                var payload = parts[1];
                switch (payload.Length % 4)
                {
                    case 2: payload += "=="; break;
                    case 3: payload += "="; break;
                }
                var jsonBytes = Convert.FromBase64String(payload.Replace('-', '+').Replace('_', '/'));
                var claims = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(jsonBytes);

                if (claims is null) return null;

                if (claims.TryGetValue("exp", out var expEl))
                {
                    var expiry = DateTimeOffset.FromUnixTimeSeconds(expEl.GetInt64());
                    if (expiry < DateTimeOffset.UtcNow) return null;
                }

                var claimsList = claims.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
                return new ClaimsPrincipal(new ClaimsIdentity(claimsList, "jwt"));
            }
            catch
            {
                return null;
            }
        }
    }
}
