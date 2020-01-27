using DonVo.Inventory.Domains.Repositories;
using System.Net.Http;
using System.Threading.Tasks;

namespace DonVo.Inventory.xUnitTests.Repository.Helpers
{
    public class HttpFailTestService : IHttpServiceRepository
    {
        public static string Token;

        public Task<HttpResponseMessage> PutAsync(string url, HttpContent content)
        {
            return Task.Run(() => new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError));
        }
        public Task<HttpResponseMessage> GetAsync(string url)
        {
            return Task.Run(() => new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError));
        }

        public Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            return Task.Run(() => new HttpResponseMessage(System.Net.HttpStatusCode.InternalServerError));
        }
    }
}
