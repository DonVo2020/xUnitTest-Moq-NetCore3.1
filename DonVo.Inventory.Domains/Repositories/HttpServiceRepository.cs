using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace DonVo.Inventory.Domains.Repositories
{
    public class HttpServiceRepository : IHttpServiceRepository
    {
        private readonly HttpClient _client = new HttpClient();

        public HttpServiceRepository(IIdentityInterface identityService)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, identityService.Token);
        }

        public async Task<HttpResponseMessage> PutAsync(string url, HttpContent content)
        {
            return await _client.PutAsync(url, content);
        }

        public async Task<HttpResponseMessage> GetAsync(string url)
        {
            return await _client.GetAsync(url);
        }

        public async Task<HttpResponseMessage> PostAsync(string url, HttpContent content)
        {
            return await _client.PostAsync(url, content);
        }
    }
}
