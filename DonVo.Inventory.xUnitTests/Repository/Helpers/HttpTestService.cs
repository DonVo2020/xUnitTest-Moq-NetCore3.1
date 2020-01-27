using DonVo.Inventory.Domains.Repositories;
using System.Net.Http;
using System.Threading.Tasks;

namespace DonVo.Inventory.xUnitTests.Repository.Helpers
{
    public class HttpTestService : IHttpServiceRepository
    {
        public static string Token;

        public Task<HttpResponseMessage> PutAsync(string url, HttpContent content)
        {
            return Task.Run(() => new HttpResponseMessage());
        }
        public Task<HttpResponseMessage> GetAsync(string url)
        {
            return Task.Run(() => new HttpResponseMessage() { Content = new StringContent("{\"data\" : [{'Id':1, 'Id':1, 'Unit':'Unit', 'Code':'Code', 'Name':'Name'}]}") });
        }

        public Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            return Task.Run(() => new HttpResponseMessage());
        }
    }
}
