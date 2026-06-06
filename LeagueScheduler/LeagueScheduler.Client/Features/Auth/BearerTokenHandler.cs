using System.Net.Http.Headers;

namespace LeagueScheduler.Client.Features.Auth
{
    public class BearerTokenHandler : DelegatingHandler
    {
        private readonly AppAuthStateProvider _auth;

        public BearerTokenHandler(AppAuthStateProvider auth) => _auth = auth;

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken ct)
        {
            if (!string.IsNullOrEmpty(_auth.Token))
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _auth.Token);

            return base.SendAsync(request, ct);
        }
    }
}
