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
            str = String.Concat(str, $"\"id\": {Id}, \"title\": \"{Title}\", \"Youtube\": \"{Youtube}\", \"Release\": \"{Release}\"");
            str = String.Concat(str, "}");
            return str;
        }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private static readonly String API_KEY = "fcddb7069db544977f0b9737e9230666";
        private static readonly HttpClient client = new HttpClient();

        // GET api/values
        [HttpGet]
        public async Task<ActionResult<string>> Get()
        {
            var query = "007";
            var responseString = await client.GetStringAsync($"https://api.themoviedb.org/3/search/movie?api_key={API_KEY}&query={query}");
            var responseJson = JsonConvert.DeserializeObject<JObject>(responseString);

            var movies = new List<Movie>();
            var results = responseJson["results"];
            foreach(var result in results)
            {
                var m = new Movie(result["id"].ToObject<int>(), result["title"].ToObject<string>(), "", result["release_date"].ToObject<string>());
                movies.Add(m);
            }

            // Build JSON array as a string
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
