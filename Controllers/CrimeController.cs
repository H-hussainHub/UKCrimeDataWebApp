using Microsoft.AspNetCore.Http;
using UKCrimeDataWebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace UKCrimeDataWebApp.Controllers
{
    public class CrimeController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<CrimeController> _logger;

        public CrimeController(IHttpClientFactory httpClientFactory, ILogger<CrimeController> logger)
        {
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
        }

        public IActionResult Crime()
        {
            return View(new CrimeSummary());
        }

        [HttpPost]
        public async Task<IActionResult> Crime(CrimeSummary summary)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var response = await _httpClient.GetStringAsync($"https://data.police.uk/api/crimes-street/all-crime?lat={summary.Latitude}&lng={summary.Longitude}&date={summary.Date}");
                    var crimes = JsonConvert.DeserializeObject<List<Crime>>(response);

                    foreach (var crime in crimes)
                    {
                        if (summary.CrimesByCategory.ContainsKey(crime.Category))
                        {
                            summary.CrimesByCategory[crime.Category]++;
                        }
                        else
                        {
                            summary.CrimesByCategory.Add(crime.Category, 1);
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while fetching crime data.");

                    ModelState.AddModelError(string.Empty, "An error occurred while processing your request. Please try again later.");
                }
            }

            return View(summary);
        }
    }
}
