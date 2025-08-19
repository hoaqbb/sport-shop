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
            new ApiScope("catalogapi.read"),
            new ApiScope("catalogapi.write"),
            new ApiScope("basketapi"),
            new ApiScope("sportshopgateway")
        };

    public static IEnumerable<ApiResource> ApiResources => new
    ApiResource[]
    {
        //List of Microservices
        new ApiResource("Catalog", "Catalog.API")
        {
            Scopes = {"catalogapi.read", "catalogapi.write"}
        },
        new ApiResource("Basket", "Basket.API")
        {
            Scopes = {"basketapi"}
        },
        new ApiResource("SportShopGateway", "Sport Shop Gateway")
        {
            Scopes = {"sportshopgateway", "basketapi"}
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
                AllowedScopes = {"catalogapi.read", "catalogapi.write"}
            },
            new Client
            {
                ClientName = "Basket API Client",
                ClientId = "BasketAPIClient",
                ClientSecrets = {new Secret("5cf3dc71-7d1a-4ffd-8720-217165b4f29a".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = {"basketapi"}
            },
            new Client
            {
                ClientName = "Sport Shop Gateway Client",
                ClientId = "SportShopGatewayClient",
                ClientSecrets = {new Secret("1gk6dc71-7d1a-4ffd-8720-217165b4f29a".Sha256())},
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = {"sportshopgateway", "basketapi"}
            }
        };
}
