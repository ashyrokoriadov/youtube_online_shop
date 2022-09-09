using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using OnlineShop.Library.Common.Responses;
using OnlineShop.Library.Constants;
using OnlineShop.Library.Options;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace OnlineShop.Library.Clients
{
    public abstract class RepoClientBase<T> : IRepoClient<T>, IHttpClientContainer
    {
        public RepoClientBase(HttpClient client, IOptions<ServiceAdressOptions> options)
        {
            HttpClient = client;
            InitializeClient(options);
            SetControllerName();
        }

        protected abstract void InitializeClient(IOptions<ServiceAdressOptions> options);

        protected abstract void SetControllerName();

        public HttpClient HttpClient { get; init; }

        protected string ControllerName { get; set; }

        public async Task<ServiceResponse<Guid>> Add(T entity) => await SendPostRequest<T, Guid>(entity, $"{ControllerName}/{RepoActions.Add}");

        public async Task<ServiceResponse<IEnumerable<Guid>>> AddRange(IEnumerable<T> entities)
            => await SendPostRequest<IEnumerable<T>, IEnumerable<Guid>>(entities, $"{ControllerName}/{RepoActions.AddRange}");

        public async Task<ServiceResponse<T>> Update(T entity) => await SendPostRequest<T, T>(entity, $"{ControllerName}/{RepoActions.Update}");

        public async Task<ServiceResponse<object>> Remove(Guid entityId) => await SendPostRequest<Guid, object>(entityId, $"{ControllerName}/{RepoActions.Remove}");

        public async Task<ServiceResponse<object>> RemoveRange(IEnumerable<Guid> entityIds)
            => await SendPostRequest<IEnumerable<Guid>, object>(entityIds, $"{ControllerName}/{RepoActions.RemoveRange}");

        public async Task<ServiceResponse<T>> GetOne(Guid entityId) => await SendGetRequest<T>($"{ControllerName}?Id={entityId}");

        public async Task<ServiceResponse<IEnumerable<T>>> GetAll() => await SendGetRequest<IEnumerable<T>>($"{ControllerName}/{RepoActions.GetAll}");

        protected async Task<ServiceResponse<TResponse>> SendPostRequest<TRequest, TResponse>(TRequest request, string path)
        {
            var jsonContent = JsonConvert.SerializeObject(request);
            var httpContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
            var requestResult = await HttpClient.PostAsync(path, httpContent);

            ServiceResponse<TResponse> result;

            if (requestResult.IsSuccessStatusCode)
            {
                var responseJson = await requestResult.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<TResponse>(responseJson);
                result = new ServiceResponse<TResponse>(response);
            }
            else
            {
                result = new ServiceResponse<TResponse>(new[] { $"{nameof(requestResult.StatusCode)}:{requestResult.StatusCode}; " +
                    $"{nameof(requestResult.ReasonPhrase)}:{requestResult.ReasonPhrase}." });
            }

            return result;
        }

        protected async Task<ServiceResponse<TResponse>> SendGetRequest<TResponse>(string path)
        {
            var requestResult = await HttpClient.GetAsync(path);

            ServiceResponse<TResponse> result;

            if (requestResult.IsSuccessStatusCode)
            {
                var responseJson = await requestResult.Content.ReadAsStringAsync();
                var response = JsonConvert.DeserializeObject<TResponse>(responseJson);
                result = new ServiceResponse<TResponse>(response);
            }
            else
            {
                result = new ServiceResponse<TResponse>(new[] { $"{nameof(requestResult.StatusCode)}:{requestResult.StatusCode}; " +
                    $"{nameof(requestResult.ReasonPhrase)}:{requestResult.ReasonPhrase}." });
            }

            return result;
        }
    }
}
