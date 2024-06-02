using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;

namespace Diploma.UI.Services.Http
{
    public abstract class HttpServiceBase
    {
        protected readonly HttpClient _client;
        private IConfiguration _appSettings { get; set; }
        protected abstract string _apiControllerName { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="appSettings">Application configuration</param>
        /// <param name="localStorage">Local storage service.</param>
        protected HttpServiceBase(IConfiguration appSettings)
        {
            _appSettings = appSettings;

            if (string.IsNullOrEmpty(_appSettings["ApiUrl"]))
                return;

            _client = new HttpClient();
            _client.BaseAddress = new Uri(_appSettings["ApiUrl"]);
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.Timeout = TimeSpan.FromMinutes(5);
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        protected string Url()
        {
            return $"/api/{_apiControllerName}";
        }

        protected string Url(string action)
        {
            return $"/{_apiControllerName}/{action}";
        }

        protected async Task EnsureSuccessStatusCodeAsync(HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            var content = await response.Content.ReadAsStringAsync();

            string s = response.StatusCode.ToString() + content;
            throw new Exception(s);
        }
    }
}
