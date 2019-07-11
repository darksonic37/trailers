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
    public class Movie
    {
        private int Id;
        private string Title;
        private string Youtube;
        private string Release;

        public Movie(int id, string title, string youtube, string release)
        {
            Id = id;
            Title = title;
            Youtube = youtube;
            Release = release;
        }

        public override string ToString()
        {
            string str = "";
            str = String.Concat(str, "{");
            str = String.Concat(str, $"\"id\": {Id}, \"title\": \"{Title}\", \"youtube\": \"{Youtube}\", \"release\": \"{Release}\"");
            str = String.Concat(str, "}");
            return str;
        }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static readonly String API_KEY = "fcddb7069db544977f0b9737e9230666";
        private static readonly String API_URL = "https://api.themoviedb.org/3";
        private static readonly HttpClient client = new HttpClient();

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var query = "007";

            // Request movies matching query
            var searchResponseString = await client.GetStringAsync($"{API_URL}/search/movie?api_key={API_KEY}&query={query}&append_to_response=videos");
            var searchResponseJson = JsonConvert.DeserializeObject<JObject>(searchResponseString);
            Console.WriteLine(searchResponseJson);
            
            // Build list of movies
            var movies = new List<Movie>();
            foreach(var result in searchResponseJson["results"])
            {
                // Save this specific movie's basic details
                var id = result["id"].ToObject<int>();
                var title = result["title"].ToObject<string>();
                var release = result["release_date"].ToObject<string>();
                
                // Request this specific movie's trailer URL
                var trailerResponseString = await client.GetStringAsync($"{API_URL}/movie/{id}/videos?api_key={API_KEY}");
                var trailerResponseJson = JsonConvert.DeserializeObject<JObject>(trailerResponseString);
                var trailer = "";
                if(trailerResponseJson["results"].Count() != 0)
                    trailer = String.Concat("https://www.youtube.com/watch?v=", trailerResponseJson["results"][0]["key"].ToObject<string>());

                // Build movie object and append to list of movies
                var m = new Movie(
                    id, 
                    title,
                    trailer, 
                    release
                );
                movies.Add(m);
            }

            // Build JSON response as a string
            var str = "[";
            str = String.Concat(str, movies[0].ToString());
            for(int i = 1; i < movies.Count; i++) 
            {
                str = String.Concat(str, ",");
                str = String.Concat(str, movies[i].ToString());
            }
            str = String.Concat(str, "]");
            return str;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
