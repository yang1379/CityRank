using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using CityRank.Models;

namespace CityRank
{
    public class WebRequest
    {
        private CityRankContext _context;

        public WebRequest(CityRankContext context)
        {
            _context = context;
        }

        // The second parameter is a function that returns a Dictionary of string keys to object values.
        // If an API returned an array as its top level collection the parameter type would be "Action>"
        public static async Task GetLatLongByCityStateNameAsync(string city, string state, Action<LatLng> Callback)
        {
            // Create a temporary HttpClient connection.
            using (var Client = new HttpClient())
            {
                try
                {
                    string apiKey = "AIzaSyA3ie2G8iuJB2LBO9ZMjUPmNEwQTE3o8z4";

                    string uri = $"https://maps.googleapis.com/maps/api/geocode/json?address={city},+{state}&key={apiKey}";
                    Client.BaseAddress = new Uri(uri);
                    HttpResponseMessage Response = await Client.GetAsync(""); // Make the actual API call.
                    Response.EnsureSuccessStatusCode(); // Throw error if not successful.
                    string StringResponse = await Response.Content.ReadAsStringAsync(); // Read in the response as a string.
                     
                    // Then parse the result into JSON and convert to a dictionary that we can use.
                    // DeserializeObject will only parse the top level object, depending on the API we may need to dig deeper and continue deserializing
                    // Dictionary<string, object> JsonResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(StringResponse);
                     
                    JObject LatLongbject = JsonConvert.DeserializeObject<JObject>(StringResponse);
                    
                    // Console.WriteLine("results: " + LatLongbject["results"]);
                    // Console.WriteLine("geometry: " + LatLongbject["results"][0]["geometry"]);
                    // Console.WriteLine("location: " + LatLongbject["results"][0]["geometry"]["location"]);
                    // Console.WriteLine("lat: " + LatLongbject["results"][0]["geometry"]["location"]["lat"]);
                    // Console.WriteLine("lng: " + LatLongbject["results"][0]["geometry"]["location"]["lng"]);

                    Console.WriteLine("City: " + LatLongbject["results"][0]["address_components"][0]["long_name"]);
                    Console.WriteLine("State: " + LatLongbject["results"][0]["address_components"][2]["short_name"]);

                    LatLng LatLngData = new LatLng{
                        City = LatLongbject["results"][0]["address_components"][0]["long_name"].ToString(),
                        State = LatLongbject["results"][0]["address_components"][2]["short_name"].ToString(),
                        Latitude = LatLongbject["results"][0]["geometry"]["location"]["lat"].Value<double>(),
                        Longitude = LatLongbject["results"][0]["geometry"]["location"]["lng"].Value<double>()
                    };

                    // string lat = LatLongbject["results"][0]["geometry"]["location"]["lat"].ToString();
                    // string lng = LatLongbject["results"][0]["geometry"]["location"]["lng"].ToString();
                    
                    // Finally, execute our callback, passing it the response we got.
                    // Callback(JsonResponse);
                    Callback(LatLngData);
                }
                catch (HttpRequestException e)
                {
                    // If something went wrong, display the error.
                    Console.WriteLine($"Request exception: {e.Message}");
                }
            }
        }


        public static async Task GetCityAttractionAsync(LatLng latLng, string attraction, string uri, Action<CityAttractions> Callback)
        {
            string nextPageToken = null;
            // Create a temporary HttpClient connection.
            using (var Client = new HttpClient())
            {
                try
                {
                    // string placesApiKey = "AIzaSyAgsubmt3p8asFrV-x_x_cEInQokzf-X9s";

                    // https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=-33.8670,151.1957&radius=500&types=food&name=cruise&key=YOUR_API_KEY
                    // string uri = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={latLng.Latitude},{latLng.Longitude}&radius=100000&types={attraction}&key={placesApiKey}";
                    // https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=47.6101497,-122.2015159&radius=50000&types=park&rankby=prominence&key=AIzaSyAgsubmt3p8asFrV-x_x_cEInQokzf-X9s
                    // string uri = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={latLng.Latitude},{latLng.Longitude}&types={attraction}&rankby=distance&key={placesApiKey}";
                    // string uri = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={latLng.Latitude},{latLng.Longitude}&types={attraction}&radius=3200&key={placesApiKey}";
                    Client.BaseAddress = new Uri(uri);
                    HttpResponseMessage Response = await Client.GetAsync(""); // Make the actual API call.
                    Response.EnsureSuccessStatusCode(); // Throw error if not successful.
                    string StringResponse = await Response.Content.ReadAsStringAsync(); // Read in the response as a string.
                     
                    JObject CityAttractionsObject = JsonConvert.DeserializeObject<JObject>(StringResponse);
                    
                    // Console.WriteLine("results: " + CityAttractionsObject["results"]);
                    // Console.WriteLine("next_page_token: " + CityAttractionsObject["next_page_token"]);
                    if(CityAttractionsObject["next_page_token"] != null) {
                        nextPageToken = CityAttractionsObject["next_page_token"].Value<string>();
                    }
                    
                    JArray AttractionList = CityAttractionsObject["results"].Value<JArray>();

                    List<Attraction> Attractions = new List<Attraction>();

                    foreach(JObject AttractionObject in AttractionList) {

                        Attraction newAttraction = new Attraction {
                            Name = AttractionObject["name"].Value<string>(),
                            Vicinity = AttractionObject["vicinity"].Value<string>(),
                            Latitude = AttractionObject["geometry"]["location"]["lat"].Value<double>(),
                            Longitude = AttractionObject["geometry"]["location"]["lng"].Value<double>()
                        };

                        Attractions.Add(newAttraction);
                    }

                    // Console.WriteLine("results: " + CityAttractionsObject["results"]);
                    // Console.WriteLine("name: " + CityAttractionsObject["results"][0]["name"]);
                    // Console.WriteLine("name: " + CityAttractionsObject["results"][1]["name"]);
                    // Console.WriteLine("name: " + CityAttractionsObject["results"][2]["name"]);


                    CityAttractions CityAttractions = new CityAttractions {
                        Attraction = attraction,
                        Attractions = Attractions,
                        NextPageToken = nextPageToken
                    };

                    // Finally, execute our callback, passing it the response we got.
                    // Callback(JsonResponse);
                    Callback(CityAttractions);
                }
                catch (HttpRequestException e)
                {
                    // If something went wrong, display the error.
                    Console.WriteLine($"Request exception: {e.Message}");
                }
            }
        }


        // public static async Task GetCityAttractionAsync(LatLng latLng, string attraction, Action<CityAttractions> Callback)
        // {
        //     // Create a temporary HttpClient connection.
        //     using (var Client = new HttpClient())
        //     {
        //         try
        //         {
        //             string placesApiKey = "AIzaSyAgsubmt3p8asFrV-x_x_cEInQokzf-X9s";

        //             // https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=-33.8670,151.1957&radius=500&types=food&name=cruise&key=YOUR_API_KEY
        //             // string uri = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={latLng.Latitude},{latLng.Longitude}&radius=100000&types={attraction}&key={placesApiKey}";
        //             // https://maps.googleapis.com/maps/api/place/nearbysearch/json?location=47.6101497,-122.2015159&radius=50000&types=park&rankby=prominence&key=AIzaSyAgsubmt3p8asFrV-x_x_cEInQokzf-X9s
        //             // string uri = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={latLng.Latitude},{latLng.Longitude}&types={attraction}&rankby=distance&key={placesApiKey}";
        //             string uri = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={latLng.Latitude},{latLng.Longitude}&types={attraction}&radius=3200&key={placesApiKey}";
        //             Client.BaseAddress = new Uri(uri);
        //             HttpResponseMessage Response = await Client.GetAsync(""); // Make the actual API call.
        //             Response.EnsureSuccessStatusCode(); // Throw error if not successful.
        //             string StringResponse = await Response.Content.ReadAsStringAsync(); // Read in the response as a string.
                     
        //             JObject CityAttractionsObject = JsonConvert.DeserializeObject<JObject>(StringResponse);
                    
        //             JArray AttractionList = CityAttractionsObject["results"].Value<JArray>();

        //             List<Attraction> Attractions = new List<Attraction>();

        //             foreach(JObject AttractionObject in AttractionList) {

        //                 Attraction newAttraction = new Attraction {
        //                     Name = AttractionObject["name"].Value<string>(),
        //                     Vicinity = AttractionObject["vicinity"].Value<string>()
        //                 };

        //                 Attractions.Add(newAttraction);
        //             }

        //             // Console.WriteLine("results: " + CityAttractionsObject["results"]);
        //             // Console.WriteLine("name: " + CityAttractionsObject["results"][0]["name"]);
        //             // Console.WriteLine("name: " + CityAttractionsObject["results"][1]["name"]);
        //             // Console.WriteLine("name: " + CityAttractionsObject["results"][2]["name"]);


        //             CityAttractions CityAttractions = new CityAttractions {
        //                 Attraction = attraction,
        //                 Attractions = Attractions
        //             };

        //             // Finally, execute our callback, passing it the response we got.
        //             // Callback(JsonResponse);
        //             Callback(CityAttractions);
        //         }
        //         catch (HttpRequestException e)
        //         {
        //             // If something went wrong, display the error.
        //             Console.WriteLine($"Request exception: {e.Message}");
        //         }
        //     }
        // }






    }
}
