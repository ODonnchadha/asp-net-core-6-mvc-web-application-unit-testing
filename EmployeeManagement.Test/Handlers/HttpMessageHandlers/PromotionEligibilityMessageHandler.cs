using EmployeeManagement.Business;
using System.Net;
using System.Text;
using System.Text.Json;

namespace EmployeeManagement.Test.Handlers.HttpMessageHandlers
{
    public class PromotionEligibilityMessageHandler : HttpMessageHandler
    {
        private readonly bool isEligible;

        public PromotionEligibilityMessageHandler(bool isEligible) 
            => this.isEligible = isEligible;

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var promotion = 
                new PromotionEligibility { EligibleForPromotion =  isEligible };

            var response = new HttpResponseMessage(HttpStatusCode.OK)
            {                
               Content = new StringContent(JsonSerializer.Serialize(promotion, 
                new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                }), 
                Encoding.ASCII, "application/json")
            };

            return Task.FromResult(response);
        }
    }
}
