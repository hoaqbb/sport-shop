using Microsoft.Extensions.Logging;
using Ordering.Core.Entities;

namespace Ordering.Infrastructure.Data
{
    public class OrderContextSeed
    {
        public static async Task SeedAsync(OrderContext orderContext, ILogger<OrderContextSeed> logger)
        {
            if(!orderContext.Orders.Any())
            {
                orderContext.Orders.AddRange(GetOrders());
                await orderContext.SaveChangesAsync();
                logger.LogInformation($"Ordering Database: {typeof(OrderContext).Name} seeded!!!");
            }
        }

        private static IEnumerable<Order> GetOrders()
        {
            return new List<Order>
            {
                new ()
                {
                    UserName = "hoang",
                    FirstName = "Dinh",
                    LastName = "Hoang",
                    EmailAddress = "hoang@sport-shop.net",
                    AddressLine = "Ho Chi Minh",
                    Country = "VietNam",
                    TotalPrice = 750,
                    State = "HCM",
                    ZipCode = "510000",

                    CardName = "Visa",
                    CardNumber = "1234567890123456",
                    CreatedBy = "Hoang",
                    Expiration = "12/25",
                    Cvv = "123",
                    PaymentMethod = 1,
                    LastModifiedBy = "Hoang",
                    LastModifiedDate = new DateTime(),
                }
            };
        }
    }
}
