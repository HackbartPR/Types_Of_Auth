using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Net.Http;
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
            HttpClient api = _httpClientFactory.CreateClient("WebApi");

            ForecastItens = await api.GetFromJsonAsync<List<WeatherForecast>>("WeatherForecast") 
                ?? throw new Exception("Não foi possível consultar na API");
        }
    }
}
