using System.Net.Http;
using System.Threading.Tasks;

namespace DonVo.Inventory.Domains.Repositories
{
    public interface IHttpServiceRepository
    {
        Task<HttpResponseMessage> PutAsync(string url, HttpContent content);
        Task<HttpResponseMessage> GetAsync(string url);
        Task<HttpResponseMessage> PostAsync(string url, HttpContent content);
    }
}
