﻿using System;
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
        private float Rating;
        private bool Adult;

        public Movie(int id, string title, string youtube, string release, float rating, bool adult)
        {
            Id = id;
            Title = title;
            Youtube = youtube;
            Release = release;
            Rating = rating;
            Adult = adult;
        }

        public override string ToString()
        {
            string str = "";
            str = String.Concat(str, "{");
            str = String.Concat(str, $"\"id\": {Id}, \"title\": \"{Title}\", \"youtube\": \"{Youtube}\", \"release\": \"{Release}\", \"rating\": {Rating}, \"adult\": {Adult.ToString().ToLower()}");
            str = String.Concat(str, "}");
            return str;
        }

    }

    [Route("api/[controller]")]
    [ApiController]
    public class SearchController : ControllerBase
    {
        private static readonly String API_KEY = "fcddb7069db544977f0b9737e9230666";
        private static readonly String API_URL = "https://api.themoviedb.org/3";
        private static readonly HttpClient client = new HttpClient();

        // GET /api/search/{query}
        [HttpGet("{query}")]
        public async Task<ActionResult<string>> GetSearch(string query)
        {
            // Request movies matching query
            var searchResponseString = await client.GetStringAsync($"{API_URL}/search/movie?api_key={API_KEY}&query={query}");
            var searchResponseJson = JsonConvert.DeserializeObject<JObject>(searchResponseString);
            
            // Build list of movies
            var movies = new List<Movie>();
            foreach(var result in searchResponseJson["results"])
            {
                // Save this specific movie's basic details
                var id = result["id"].ToObject<int>();
                var title = result["title"].ToObject<string>();
                var release = result["release_date"].ToObject<string>();
                var rating = result["vote_average"].ToObject<float>();
                var adult = result["adult"].ToObject<bool>();
                
                try
                {
                    // Request this specific movie's trailer URL
                    var trailerResponseString = await client.GetStringAsync($"{API_URL}/movie/{id}/videos?api_key={API_KEY}");
                    var trailerResponseJson = JsonConvert.DeserializeObject<JObject>(trailerResponseString);
                    var trailer = String.Concat("https://www.youtube.com/embed/", trailerResponseJson["results"][0]["key"].ToObject<string>());

                    // Build movie object and append to list of movies
                    var m = new Movie(id, title, trailer, release, rating, adult);
                    movies.Add(m);
                } catch {

                }
            }

            // Build JSON response as a string
            var limit = movies.Count > 5 ? 5 : movies.Count;
            var str = "[";
            str = String.Concat(str, movies[0].ToString());
            for(int i = 1; i < limit; i++) 
            {
                str = String.Concat(str, ",");
                str = String.Concat(str, movies[i].ToString());
            }
            str = String.Concat(str, "]");
            return str;
        }
    }
}
