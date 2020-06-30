using System.Collections.Generic;
using System.Runtime.InteropServices.ComTypes;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;
using WebApplication.DataAccess.SQL.DataModels;
using WebApplication.DataAccess.SQL.Providers;

namespace WebApplication.IdentityServer
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
               new IdentityResource(
                     name: "profile",
                     displayName: "Your profile data",
                     userClaims: new[] { "email", "nome" , "ruolo"}
                     )
            };
        }

        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                 new ApiScope(name: "api1.get",   displayName: "my api scope"),
            };
        }


        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
           {
              new ApiResource("api1", "My api")
              {
                    Scopes = { "api1.get"}
              }
           };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
           {
               new Client
               {
                  ClientId = "client",
                  AllowedGrantTypes = GrantTypes.ClientCredentials,

                  ClientSecrets =
                  {
                     new Secret("secret".Sha256())
                  },
                  AllowedScopes = { "api1.get","profile" },

               },
               new Client
               {
                   ClientId = "ro.client",
                   AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                   AllowAccessTokensViaBrowser = true,
                   ClientSecrets =
                   {
                      new Secret("secret".Sha256())
                   },
                  AllowedScopes = { "api1.get","profile" }
               },
               new Client
        {
            ClientId = "mvc",
            ClientSecrets = { new Secret("secret".Sha256()) },

            AllowedGrantTypes = GrantTypes.Code,
            RequireConsent = false,

            RedirectUris = { "https://localhost:44330/signin-oidc" },

            PostLogoutRedirectUris = { "https://localhost:44330/Home/Index" },

            AllowedScopes = { "api1.get","profile", IdentityServerConstants.StandardScopes.OpenId }
        }

            };
        }

    }


}
