using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OnlineShop.Library.Authentification.Models;
using OnlineShop.Library.Authentification.Requests;
using OnlineShop.Library.Authentification.Responses;
using OnlineShop.Library.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Library.Clients.AuthenticationService
{
    public class UsersClient : IUsersClient, IDisposable
    {       
        public UsersClient(HttpClient client, IOptions<ServiceAdressOptions> options)
        {
            HttpClient = client;
            HttpClient.BaseAddress = new Uri(options.Value.AuthentificationService);
        }

        public HttpClient HttpClient { get; init; }

        public async Task<IdentityResult> Add(CreateUserRequest request) => await SendPostRequest(request, "/users/add");

        public async Task<IdentityResult> ChangePassword(UserPasswordChangeRequest request) => await SendPostRequest(request, "/users/changepassword");

        public void Dispose() =>HttpClient.Dispose();

        public async Task<AuthenticationServiceResponse<ApplicationUser>> Get(string name) => await SendGetRequest<ApplicationUser>($"users?name={name}");

        public async Task<AuthenticationServiceResponse<IEnumerable<ApplicationUser>>> GetAll() => await SendGetRequest<IEnumerable<ApplicationUser>>("/users/all");

        public async Task<IdentityResult> Remove(ApplicationUser user) => await SendPostRequest(user, "/users/remove");       

        public async Task<IdentityResult> Update(ApplicationUser user) => await SendPostRequest(user, "/users/update");

        private async Task<IdentityResult> SendPostRequest<TRequest>(TRequest request, string path)
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

        private  async Task<AuthenticationServiceResponse<TResult>> SendGetRequest<TResult>(string request)
        {
            var requestResult = await HttpClient.GetAsync(request);

            AuthenticationServiceResponse<TResult> result;

            if (requestResult.IsSuccessStatusCode)
            {
                var response = await requestResult.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(response))
                {
                    result = new AuthenticationServiceResponse<TResult>()
                    {
                        Code = requestResult.StatusCode.ToString(),
                        Description = requestResult.ReasonPhrase
                    };
                }
                else
                {
                    var payload = JsonConvert.DeserializeObject<TResult>(response);
                    result = new AuthenticationServiceResponse<TResult>()
                    {
                        Code = requestResult.StatusCode.ToString(),
                        Description = requestResult.ReasonPhrase,
                        Payload = payload
                    };
                }
            }
            else
            {
                result = new AuthenticationServiceResponse<TResult>()
                {
                    Code = requestResult.StatusCode.ToString(),
                    Description = requestResult.ReasonPhrase
                };
            }

            return result;
        }
    }
}
