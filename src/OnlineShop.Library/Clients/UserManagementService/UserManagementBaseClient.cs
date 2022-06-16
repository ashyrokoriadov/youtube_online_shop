using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OnlineShop.Library.Authentification.Responses;
using OnlineShop.Library.Options;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Library.Clients.UserManagementService
{
    public abstract class UserManagementBaseClient : IDisposable
    {
        public UserManagementBaseClient(HttpClient client, IOptions<ServiceAdressOptions> options)
        {
            HttpClient = client;
            HttpClient.BaseAddress = new Uri(options.Value.UserManagementService);
        }

        public HttpClient HttpClient { get; init; }

        protected async Task<IdentityResult> SendPostRequest<TRequest>(TRequest request, string path)
        {
            var jsonContent = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var requestResult = await HttpClient.PostAsync(path, httpContent);

            IdentityResult result;

            if (requestResult.IsSuccessStatusCode)
            {
                var response = await requestResult.Content.ReadAsStringAsync();
                result = IdentityResult.Success;
            }
            else
            {
                result = IdentityResult.Failed(
                    new IdentityError()
                    {
                        Code = requestResult.StatusCode.ToString(),
                        Description = requestResult.ReasonPhrase
                    }
                );
            }

            return result;
        }

        protected async Task<UserManagementServiceResponse<TResult>> SendGetRequest<TResult>(string request)
        {
            var requestResult = await HttpClient.GetAsync(request);

            UserManagementServiceResponse<TResult> result;

            if (requestResult.IsSuccessStatusCode)
            {
                var response = await requestResult.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(response))
                {
                    result = new UserManagementServiceResponse<TResult>()
                    {
                        Code = requestResult.StatusCode.ToString(),
                        Description = requestResult.ReasonPhrase
                    };
                }
                else
                {
                    var payload = JsonConvert.DeserializeObject<TResult>(response);
                    result = new UserManagementServiceResponse<TResult>()
                    {
                        Code = requestResult.StatusCode.ToString(),
                        Description = requestResult.ReasonPhrase,
                        Payload = payload
                    };
                }
            }
            else
            {
                result = new UserManagementServiceResponse<TResult>()
                {
                    Code = requestResult.StatusCode.ToString(),
                    Description = requestResult.ReasonPhrase
                };
            }

            return result;
        }

        public void Dispose()
        {
            HttpClient.Dispose();
        }
    }
}
