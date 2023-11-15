using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System.Net.Http;
using System.Net.Http.Headers;
using WebApp.Models;

namespace WebApp.Pages
{
    [Authorize(Policy = "Employee")]
    public class EmployeeModel : PageModel
    {
        private readonly IHttpClientFactory _httpClientFactory;

        [BindProperty]
        public List<WeatherForecast> ForecastItens {  get; set; }

        public EmployeeModel(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            ForecastItens = new List<WeatherForecast>();
        }

        public async Task OnGetAsync()
        {
            JwtApi token = JsonConvert.DeserializeObject<JwtApi?>(HttpContext.Session.GetString("accessToken") ?? string.Empty) ?? new JwtApi();

            if (string.IsNullOrEmpty(token.AccessToken) || token.ExpiryTime < DateTime.UtcNow)
                token = await GetAccessToken();

            HttpClient api = _httpClientFactory.CreateClient("WebApi");
           
            api.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token?.AccessToken ?? string.Empty);
            ForecastItens = await api.GetFromJsonAsync<List<WeatherForecast>>("WeatherForecast") 
                ?? throw new Exception("Não foi possível consultar na API");
        }

        private async Task<JwtApi> GetAccessToken()
        {
            HttpClient api = _httpClientFactory.CreateClient("WebApi");

            HttpResponseMessage response = await api.PostAsJsonAsync("Auth", new Credential() { UserName = "admin", Password = "123" });
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();
            HttpContext.Session.SetString("accessToken", content);

            return JsonConvert.DeserializeObject<JwtApi>(content) ?? new JwtApi();
        }
    }
}
