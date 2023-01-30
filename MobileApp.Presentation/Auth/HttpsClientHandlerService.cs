namespace MobileApp.Presentation.Auth
{
    public class HttpsClientHandlerService
    {
        public HttpMessageHandler GetPlatformMessageHandler()
        {
#if ANDROID
            var handler = new CustomAndroidMessageHandler(); //Xamarin.Android.Net.AndroidMessageHandler();
            handler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) =>
            {
                //return true;
                //if (cert != null && cert.Issuer.Equals("CN=localhost"))
                //    return true;
                var res = errors == System.Net.Security.SslPolicyErrors.None;
                return true;
            };
            return handler;
#elif IOS
            var handler = new NSUrlSessionHandler
            {
                TrustOverrideForUrl = IsHttpsLocalhost
            };
            return handler;
#else

            //throw new PlatformNotSupportedException("Only Android and iOS supported.");
            var handler = new HttpClientHandler
            {
            };
            return handler;
#endif
        }

#if IOS
        public bool IsHttpsLocalhost(NSUrlSessionHandler sender, string url, Security.SecTrust trust)
        {
            if (url.StartsWith("https://localhost"))
                return true;
            return false;
        }
#endif
    }


#if ANDROID
    public sealed class CustomAndroidMessageHandler : Xamarin.Android.Net.AndroidMessageHandler
    {
        protected override Javax.Net.Ssl.IHostnameVerifier GetSSLHostnameVerifier(Javax.Net.Ssl.HttpsURLConnection connection)
            => new CustomHostnameVerifier();

        private sealed class CustomHostnameVerifier : Java.Lang.Object, Javax.Net.Ssl.IHostnameVerifier
        {
            public bool Verify(string? hostname, Javax.Net.Ssl.ISSLSession? session)
            {
                return true;
                //    Javax.Net.Ssl.HttpsURLConnection.DefaultHostnameVerifier.Verify(hostname, session)
                //|| hostname == "10.0.2.2" && session.PeerPrincipal?.Name == "CN=localhost";
            }
        }
    }
#endif
}
