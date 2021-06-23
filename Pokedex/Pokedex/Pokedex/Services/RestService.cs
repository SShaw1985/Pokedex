using Pokedex.Interfaces;
using Pokedex.Models;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using ModernHttpClient;


namespace Pokedex.Services
{
    public class RestService : IRestService
    {
        private  string BaseUri {get {return "https://pokeapi.co/api/v2/"; } }
        public async Task<ResponseWrapper> GetAsync(string uri)
        {
            try
            {
                if (uri != null)
                {
                    if (uri.Contains(BaseUri))
                        uri = uri.Replace(BaseUri, "");

                    var httpClient = CreateHttpClient();
                    HttpResponseMessage response = await httpClient.GetAsync(uri);
                    return await HandleResponse(response);    
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return null;
        }


        private HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(BaseUri);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }

        private async Task<ResponseWrapper> HandleResponse(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            return new ResponseWrapper
            {
                ReasonPhrase = response.ReasonPhrase,
                StatusCode = response.StatusCode,
                Content = content,
            };
        }
    }
}
