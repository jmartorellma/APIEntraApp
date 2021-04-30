using System;
using System.Linq;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace Server
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

        public static IEnumerable<ApiResource> GetApis() =>
            new List<ApiResource>
            {
                new ApiResource(Configuration["ApiEntraName"].ToString())
            };

        public static IEnumerable<Client> GetClients() =>
            new List<Client>
            {
                new Client
                {
                    ClientId = Configuration["ClientId"].ToString(),
                    AllowedGrantTypes = GrantTypes.Code,
                    RequirePkce = true,
                    RequireClientSecret = false,
                    RedirectUris = { Configuration["AngularClientEntraAppURL"].ToString() },
                    PostLogoutRedirectUris = { Configuration["AngularClientEntraAppURL"].ToString() },
                    AllowedCorsOrigins = { Configuration["AngularClientEntraAppURL"].ToString() },
                    AllowedScopes = { 
                        Configuration["ApiEntraName"].ToString(),
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    },
                    AllowAccessTokensViaBrowser = true,
                    AllowOfflineAccess = true,
                    RequireConsent = false
                }
            };
    }
}
