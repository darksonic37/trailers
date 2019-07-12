using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;


namespace trailers.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AutocompleteController : ControllerBase
    {
        private static readonly String API_KEY = "fcddb7069db544977f0b9737e9230666";
        private static readonly String API_URL = "https://api.themoviedb.org/3";
        private static readonly HttpClient client = new HttpClient();

        // GET /api/autocomplete/{query}
        [HttpGet("{query}")]
        public async Task<ActionResult<string>> GetAutocomplete(string query)
        {
            // Request movies matching query
            var searchResponseString = await client.GetStringAsync($"{API_URL}/search/movie?api_key={API_KEY}&query={query}");
            var searchResponseJson = JsonConvert.DeserializeObject<JObject>(searchResponseString);
            
            // Build list of movies
            var titles = new List<string>();
            foreach(var result in searchResponseJson["results"])
            {
                // Save this specific movie's basic details
                var title = result["title"].ToObject<string>();
                titles.Add(title);
            }

            // Build JSON string manually
            string response = "[\"";
            response = String.Concat(response, string.Join("\",\"", titles));
            response = String.Concat(response, "\"]");
            return response;
        }
    }
}
