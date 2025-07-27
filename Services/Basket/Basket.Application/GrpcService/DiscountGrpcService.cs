using Discount.Grpc.Protos;

namespace Basket.Application.GrpcService
{
    public class DiscountGrpcService
    {
        private readonly DiscountProtocoService.DiscountProtocoServiceClient _discountProtocoServiceClient;

        public DiscountGrpcService(DiscountProtocoService.DiscountProtocoServiceClient discountProtocoServiceClient)
        {
            _discountProtocoServiceClient = discountProtocoServiceClient;
        }

        public async Task<CouponModel> GetDiscount(string productName)
        {
            var discountRequest = new GetDiscountRequest { ProductName = productName };

            return await _discountProtocoServiceClient.GetDiscountAsync(discountRequest);
        }
    }
}
