using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;

namespace LeagueScheduler.Client.Features.Auth
{
    public class AppAuthStateProvider : AuthenticationStateProvider
    {
        private const string StorageKey = "auth_token";
        private const string OriginalStorageKey = "auth_token_original";
        private static readonly AuthenticationState _anonymous =
            new(new ClaimsPrincipal(new ClaimsIdentity()));

        private readonly IJSRuntime _js;

        public string? Token { get; private set; }
        public bool IsImpersonating { get; private set; }
        public string? ImpersonatedDisplayName { get; private set; }

        public AppAuthStateProvider(IJSRuntime js) => _js = js;

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            Token = await _js.InvokeAsync<string?>("localStorage.getItem", StorageKey);

            if (string.IsNullOrWhiteSpace(Token))
            {
                IsImpersonating = false;
                ImpersonatedDisplayName = null;
                return _anonymous;
            }

            var principal = ParseToken(Token);
            if (principal is null)
            {
                IsImpersonating = false;
                ImpersonatedDisplayName = null;
                return _anonymous;
            }

            IsImpersonating = principal.HasClaim(c => c.Type == "imp");
            ImpersonatedDisplayName = principal.FindFirst("displayName")?.Value;
            return new AuthenticationState(principal);
        }

        public async Task NotifyUserLoggedIn(string token)
        {
            Token = token;
            await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, token);
            await _js.InvokeVoidAsync("localStorage.removeItem", OriginalStorageKey);
            var principal = ParseToken(token)!;
            IsImpersonating = false;
            ImpersonatedDisplayName = null;
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        public async Task NotifyUserLoggedOut()
        {
            Token = null;
            IsImpersonating = false;
            ImpersonatedDisplayName = null;
            await _js.InvokeVoidAsync("localStorage.removeItem", StorageKey);
            await _js.InvokeVoidAsync("localStorage.removeItem", OriginalStorageKey);
            NotifyAuthenticationStateChanged(Task.FromResult(_anonymous));
        }

        public async Task BeginImpersonation(string impersonationToken)
        {
            // Preserve the current admin token so impersonation can be exited.
            if (!string.IsNullOrWhiteSpace(Token))
                await _js.InvokeVoidAsync("localStorage.setItem", OriginalStorageKey, Token);

            Token = impersonationToken;
            await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, impersonationToken);

            var principal = ParseToken(impersonationToken)!;
            IsImpersonating = true;
            ImpersonatedDisplayName = principal.FindFirst("displayName")?.Value;
            NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
        }

        public async Task EndImpersonation()
        {
            var originalToken = await _js.InvokeAsync<string?>("localStorage.getItem", OriginalStorageKey);
            await _js.InvokeVoidAsync("localStorage.removeItem", OriginalStorageKey);

            if (!string.IsNullOrWhiteSpace(originalToken))
            {
                Token = originalToken;
                await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, originalToken);
                var principal = ParseToken(originalToken)!;
                IsImpersonating = false;
                ImpersonatedDisplayName = null;
                NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(principal)));
            }
            else
            {
                await NotifyUserLoggedOut();
            }
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
