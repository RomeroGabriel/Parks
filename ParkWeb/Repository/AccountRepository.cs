using Newtonsoft.Json;
using ParkWeb.Models;
using ParkWeb.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ParkWeb.Repository
{
    public class AccountRepository : Repository<User>, IAccountRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public AccountRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<User> LoginAsync(string url, User user)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (user != null)
            {
                request.Content =
                    new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            }
            else
            {
                return new User();
            }

            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(jsonString);
            }
            else
            {
                return new User();
            }
        }

        public async Task<bool> RegisterAsync(string url, User objCreate)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (objCreate != null)
            {
                request.Content =
                    new StringContent(JsonConvert.SerializeObject(objCreate), Encoding.UTF8, "application/json");
            }
            else
            {
                return false;
            }

            var client = _clientFactory.CreateClient();
            HttpResponseMessage response = await client.SendAsync(request);

            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
