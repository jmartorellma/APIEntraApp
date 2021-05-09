using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace IdentityServer.ServerConfiguration
{
    public static class IdentityServerConfiguration
    {
        public static IConfiguration Configuration { get; private set; }

        public static IEnumerable<IdentityResource> GetIdentityResources() =>
           new IdentityResource[]
           {
                 new IdentityResources.OpenId(),
                 new IdentityResources.Profile()
           };

        public static IEnumerable<ApiResource> GetApis(IConfiguration configuration) =>
            new List<ApiResource>
            {
                new ApiResource(configuration["ApiEntraName"].ToString(), configuration["ApiEntraName"].ToString())
                { 
                    Scopes = { configuration["ApiEntraName"].ToString() }
                }
            };

        public static IEnumerable<ApiScope> GetScopes(IConfiguration configuration) =>
           new List<ApiScope>
           {
                new ApiScope(configuration["ApiEntraName"].ToString())
           };

        public static IEnumerable<Client> GetClients(IConfiguration configuration) =>
            new List<Client>
            {
                new Client
                {
                    ClientId = configuration["ClientId"].ToString(),
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,                    
                    RedirectUris = { configuration["AngularClientEntraAppURL"].ToString() },
                    PostLogoutRedirectUris = { configuration["AngularClientEntraAppURL"].ToString() },
                    AllowedCorsOrigins = { configuration["AngularClientEntraAppURL"].ToString() },
                    AllowedScopes = { 
                        configuration["ApiEntraName"].ToString(),
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    AccessTokenLifetime = 3600,
                    AllowAccessTokensViaBrowser = true,
                    AllowOfflineAccess = true,
                    RequireConsent = false
                }
            };
    }
}
