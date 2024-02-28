## .NET MAUI Blazor app Authentication with Idntityserver4 / duende


This sample shows how to use the [IdentityModel.OidcClient FOSS library](https://github.com/IdentityModel/IdentityModel.OidcClient). to connect a .NET MAUI app to IdentityServer


start browser-based authentication flows, which listen for a callback to a specific URL registered to the app.

### IdntityServer Client Configuration
```
    new ClientEntity
    {
        ClientId = "mobile-app",
        AllowedGrantTypes = GrantTypes.Code,
        RequirePkce = true,
        RequireClientSecret = false,
        AllowedScopes =
        {
            IdentityServerConstants.StandardScopes.OpenId,
            IdentityServerConstants.StandardScopes.Profile
        },
        RedirectUris = { "myapp://" },
        PostLogoutRedirectUris = { "myapp://" }
    }
```

