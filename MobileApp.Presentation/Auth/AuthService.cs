using IdentityModel.Client;
using IdentityModel.OidcClient;
using System.Security.Claims;
using System.Text.Json;

namespace MobileApp.Presentation.Auth
{
    public class AuthService
    {
        private readonly OidcClient _client;

        public AuthService(OidcClient client)
        {
            _client = client;
        }

        public ClaimsPrincipal User { get; private set; }
        public string RefreshToken { get; private set; }
        public string AccessToken { get; private set; }

        public bool IsAuthenticated => User != null;
        public async Task Login()
        {
            if (!IsAuthenticated)
            {
                var result = await _client.LoginAsync();
                HandleError(result);

                User = result.User;
                if (!string.IsNullOrWhiteSpace(result.AccessToken))
                {
                    AccessToken = result.AccessToken;
                }

                if (!string.IsNullOrWhiteSpace(result.RefreshToken))
                {
                    RefreshToken = result.RefreshToken;
                }
            }
        }

        public async Task Logout()
        {
            var result = await _client.LogoutAsync();
            HandleError(result);
        }

        public async Task<string> ApiCall()
        {
            if (AccessToken != null)
            {
                var client = new HttpClient();
                client.SetBearerToken(AccessToken);

                var response = await client.GetAsync("https://demo.duendesoftware.com/api/test");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var doc = JsonDocument.Parse(content).RootElement;
                    return JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });
                }
                else
                {
                    throw new Exception(response.ReasonPhrase);
                }
            }

            return null;
        }

        private static void HandleError(Result result)
        {
            if (result.IsError) throw new Exception(result.Error);
        }
    }
}
