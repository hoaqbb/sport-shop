using Duende.IdentityServer.Models;

namespace SportShop.Identity;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("catalogapi"),
            new ApiScope("basketapi")
        };

    public static IEnumerable<ApiResource> ApiResources => new
    ApiResource[]
    {
        //List of Microservices
        new ApiResource("Catalog", "Catalog.API")
        {
            Scopes = {"catalogapi"}
        },
        new ApiResource("Basket", "Basket.API")
        {
            Scopes = {"basketapi"}
        }
    };

    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            //m2m flow
            new Client
            {
                ClientName = "Catalog API Client",
                ClientId = "CatalogAPIClient",
                ClientSecrets = {new Secret("9de2dc71-7d1a-4ffd-8720-217165b4f29a".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = {"catalogapi", "basketapi"}
            }
        };
}
