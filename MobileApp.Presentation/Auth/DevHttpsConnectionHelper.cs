namespace MobileApp.Presentation.Auth
{
    public class DevHttpsConnectionHelper
    {
        public DevHttpsConnectionHelper()
        {
            LazyHttpClient = new Lazy<HttpClient>(() => new HttpClient(GetPlatformMessageHandler()));
        }

        public string DevServerRootUrl { get; }

        private Lazy<HttpClient> LazyHttpClient;
        public HttpClient HttpClient => LazyHttpClient.Value;

        public HttpMessageHandler? GetPlatformMessageHandler()
        {
            var httpsClientHandlerService = new HttpsClientHandlerService();
            return httpsClientHandlerService.GetPlatformMessageHandler();
        }
    }
}
