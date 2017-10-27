using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using CityRank.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading;

namespace CityRank.Controllers
{
    public class DashboardController : Controller
    {
        private const string BAR = "bar";
        private const string BICYCLE_STORE = "bicycle_store";
        private const string CAFE = "cafe";
        private const string CONVENIENCE_STORE = "convenience_store";
        private const string PARK = "park";
        private const string RESTAURANT = "restaurant";
        private const string STARBUCKS = "starbucks";

        private CityRankContext _context;

        public DashboardController(CityRankContext context)
        {
            _context = context;
        }

        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.Latitude = HttpContext.Session.GetString("Latitude");
            ViewBag.Longitude = HttpContext.Session.GetString("Longitude");
            ViewBag.CityAttractions = new CityAttractions();
            ViewBag.Attractions = new List<Attraction>();
            ViewBag.Cities = new List<City>();

            string city = HttpContext.Session.GetString("City");
            string state = HttpContext.Session.GetString("State");
            string attraction_name = HttpContext.Session.GetString("Attraction");

            string error = HttpContext.Session.GetString("Error");

            if(error != null && error != "") {
                ViewBag.Error = error;
            }
            else if (city != null && state != null && attraction_name != null) {
                ViewBag.City = city;
                ViewBag.State = state;
                ViewBag.AttractionName = attraction_name.Replace('_', ' ');
                
                City newCity = _context.Cities.SingleOrDefault(x => x.Name == city && x.State == state);
                
                if (attraction_name == PARK) {
                    ViewBag.Attractions = _context.Parks.Where(x => x.CityId == newCity.Id);
                }
                else if (attraction_name == BAR) {
                    ViewBag.Attractions = _context.Bars.Where(x => x.CityId == newCity.Id);
                }
                else if (attraction_name == BICYCLE_STORE) {
                    ViewBag.Attractions = _context.BicycleStores.Where(x => x.CityId == newCity.Id);
                }
                else if (attraction_name == CAFE) {
                    ViewBag.Attractions = _context.Cafes.Where(x => x.CityId == newCity.Id);
                }
                else if (attraction_name == CONVENIENCE_STORE) {
                    ViewBag.Attractions = _context.ConvenienceStores.Where(x => x.CityId == newCity.Id);
                }
                else if (attraction_name == RESTAURANT) {
                    ViewBag.Attractions = _context.Restaurants.Where(x => x.CityId == newCity.Id);
                }else if (attraction_name == STARBUCKS) {
                    ViewBag.Attractions = _context.Starbucks.Where(x => x.CityId == newCity.Id);
                }
                
            }

            // ViewBag.Cities = _context.Cities.Include(x => x.Parks).OrderByDescending(x => x.Parks.Count);

            return View("Dashboard");
        }


        [HttpGet]
        [Route("current_rankings")]
        public IActionResult CurrentRankings()
        {
            ViewBag.Bars = _context.Cities.Include(x => x.Bars).Where(x => x.Bars.Count > 0).OrderByDescending(x => x.Bars.Count);
            ViewBag.BicycleStores = _context.Cities.Include(x => x.BicycleStores).Where(x => x.BicycleStores.Count > 0).OrderByDescending(x => x.BicycleStores.Count);
            ViewBag.Cafes = _context.Cities.Include(x => x.Cafes).Where(x => x.Cafes.Count > 0).OrderByDescending(x => x.Cafes.Count);
            ViewBag.ConvenienceStores = _context.Cities.Include(x => x.ConvenienceStores).Where(x => x.ConvenienceStores.Count > 0).OrderByDescending(x => x.ConvenienceStores.Count);
            ViewBag.Parks = _context.Cities.Include(x => x.Parks).Where(x => x.Parks.Count > 0).OrderByDescending(x => x.Parks.Count);
            ViewBag.Restaurants = _context.Cities.Include(x => x.Restaurants).Where(x => x.Restaurants.Count > 0).OrderByDescending(x => x.Restaurants.Count);
            ViewBag.Starbucks = _context.Cities.Include(x => x.Starbucks).Where(x => x.Starbucks.Count > 0).OrderByDescending(x => x.Starbucks.Count);
            return View("CurrentRankings");
        }

        [HttpGet]
        [Route("city_attraction/{attractionName}/{cityId}")]
        public IActionResult CityAttraction(string attractionName, int cityId)
        {
            City city = _context.Cities.SingleOrDefault(x => x.Id == cityId);
            ViewBag.City = city;
            string markers = "[";

            if (attractionName == PARK) {
                ViewBag.AttractionName = "Parks";
                // ViewBag.Attractions = _context.Parks.Where(x => x.CityId == cityId);
                List<Park> parks = _context.Parks.Where(x => x.CityId == cityId).ToList();

                foreach (Park park in parks) {
                    markers += "{";
                    markers += string.Format("'title': \"{0}\",", park.Name);
                    markers += string.Format("'lat': '{0}',", park.Latitude);
                    markers += string.Format("'lng': '{0}',", park.Longitude);
                    markers += "},";

                }

                ViewBag.Attractions = parks;
            }
            else if (attractionName == BAR) {
                ViewBag.AttractionName = "Bars";
                List<Bar> bars = _context.Bars.Where(x => x.CityId == cityId).ToList();
                
                foreach (Bar bar in bars) {
                    markers += "{";
                    markers += string.Format("'title': \"{0}\",", bar.Name);
                    markers += string.Format("'lat': '{0}',", bar.Latitude);
                    markers += string.Format("'lng': '{0}',", bar.Longitude);
                    markers += "},";

                }

                ViewBag.Attractions = bars;
            }
            else if (attractionName == BICYCLE_STORE) {
                ViewBag.AttractionName = "Bicycle Stores";
                List<BicycleStore> bicycleStores = _context.BicycleStores.Where(x => x.CityId == cityId).ToList();
                
                foreach(BicycleStore bicycleStore in bicycleStores) {
                    markers += "{";
                    markers += string.Format("'title': \"{0}\",", bicycleStore.Name);
                    markers += string.Format("'lat': '{0}',", bicycleStore.Latitude);
                    markers += string.Format("'lng': '{0}',", bicycleStore.Longitude);
                    markers += "},";

                }

                ViewBag.Attractions = bicycleStores;
            }
            else if (attractionName == CAFE) {
                ViewBag.AttractionName = "Cafes";
                List<Cafe> cafes = _context.Cafes.Where(x => x.CityId == cityId).ToList();
                
                foreach(Cafe cafe in cafes) {
                    markers += "{";
                    markers += string.Format("'title': \"{0}\",", cafe.Name);
                    markers += string.Format("'lat': '{0}',", cafe.Latitude);
                    markers += string.Format("'lng': '{0}',", cafe.Longitude);
                    markers += "},";

                }

                ViewBag.Attractions = cafes;
            }
            else if (attractionName == CONVENIENCE_STORE) {
                ViewBag.AttractionName = "Convenience Stores";
                List<ConvenienceStore> convenienceStores = _context.ConvenienceStores.Where(x => x.CityId == cityId).ToList();
                
                foreach(ConvenienceStore convenienceStore in convenienceStores) {
                    markers += "{";
                    markers += string.Format("'title': \"{0}\",", convenienceStore.Name);
                    markers += string.Format("'lat': '{0}',", convenienceStore.Latitude);
                    markers += string.Format("'lng': '{0}',", convenienceStore.Longitude);
                    markers += "},";

                }

                ViewBag.Attractions = convenienceStores;
            }
            else if (attractionName == RESTAURANT) {
                ViewBag.AttractionName = "Restaurants";
                List<Restaurant> restaurants = _context.Restaurants.Where(x => x.CityId == cityId).ToList();
                
                foreach(Restaurant restaurant in restaurants) {
                    markers += "{";
                    markers += string.Format("'title': \"{0}\",", restaurant.Name);
                    markers += string.Format("'lat': '{0}',", restaurant.Latitude);
                    markers += string.Format("'lng': '{0}',", restaurant.Longitude);
                    markers += "},";

                }

                ViewBag.Attractions = restaurants;
            }
            else if (attractionName == STARBUCKS) {
                ViewBag.AttractionName = "Starbucks";
                List<Starbuck> starbucks = _context.Starbucks.Where(x => x.CityId == cityId).ToList();
                
                foreach(Starbuck starbuck in starbucks) {
                    markers += "{";
                    markers += string.Format("'title': \"{0}\",", starbuck.Name);
                    markers += string.Format("'lat': '{0}',", starbuck.Latitude);
                    markers += string.Format("'lng': '{0}',", starbuck.Longitude);
                    markers += "},";

                }

                ViewBag.Attractions = starbucks;
            }

            markers += "];";
            ViewBag.Markers = markers;
            return View("SpecificCityAttraction");
        }


        [HttpGet]
        [Route("show_all_cities/{attractionName}")]
        public IActionResult ShowAllCities(string attractionName)
        {
            ViewBag.Cities = ViewBag.Parks = new List<City>();
            ViewBag.Cities = ViewBag.Bars = new List<City>();
            ViewBag.Cities = ViewBag.BicycleStores = new List<City>();
            ViewBag.Cities = ViewBag.Cafes = new List<City>();
            ViewBag.Cities = ViewBag.ConvenienceStores = new List<City>();
            ViewBag.Cities = ViewBag.Restaurants = new List<City>();
            ViewBag.Cities = ViewBag.Starbucks = new List<City>();

            if (attractionName == PARK) {
                ViewBag.AttractionName = "Parks";
                ViewBag.Parks = _context.Cities.Include(x => x.Parks).Where(x => x.Parks.Count > 0).OrderByDescending(x => x.Parks.Count);
            }
            else if (attractionName == BAR) {
                ViewBag.AttractionName = "Bars";
                ViewBag.Bars = _context.Cities.Include(x => x.Bars).Where(x => x.Bars.Count > 0).OrderByDescending(x => x.Bars.Count);
            }
            else if (attractionName == BICYCLE_STORE) {
                ViewBag.AttractionName = "Bicycle Stores";
                ViewBag.BicycleStores = _context.Cities.Include(x => x.BicycleStores).Where(x => x.BicycleStores.Count > 0).OrderByDescending(x => x.BicycleStores.Count);
            }
            else if (attractionName == CAFE) {
                ViewBag.AttractionName = "Cafes";
                ViewBag.Cafes = _context.Cities.Include(x => x.Cafes).Where(x => x.Cafes.Count > 0).OrderByDescending(x => x.Cafes.Count);
            }
            else if (attractionName == CONVENIENCE_STORE) {
                ViewBag.AttractionName = "Convenience Stores";
                ViewBag.ConvenienceStores = _context.Cities.Include(x => x.ConvenienceStores).Where(x => x.ConvenienceStores.Count > 0).OrderByDescending(x => x.ConvenienceStores.Count);
            }
            else if (attractionName == RESTAURANT) {
                ViewBag.AttractionName = "Restaurants";
                ViewBag.Restaurants = _context.Cities.Include(x => x.Restaurants).Where(x => x.Restaurants.Count > 0).OrderByDescending(x => x.Restaurants.Count);
            }
            else if (attractionName == STARBUCKS) {
                ViewBag.AttractionName = "Starbucks";
                ViewBag.Starbucks = _context.Cities.Include(x => x.Starbucks).Where(x => x.Starbucks.Count > 0).OrderByDescending(x => x.Starbucks.Count);
            }

            return View("ShowAllCities");
        }


        // [HttpPost]
        // [Route("select_city")]
        // public IActionResult SelectCity(string city, string state, string attraction_name)
        // {
        //     HttpContext.Session.SetString("City", city);
        //     HttpContext.Session.SetString("State", state);
        //     HttpContext.Session.SetString("Attraction", attraction_name);
        //     HttpContext.Session.SetString("Error", "");

        //     ViewBag.LatLong = new LatLng();
        //     var LatLngObject = new LatLng();
        //     var CityAttractionsObject = new CityAttractions();
        //     if(!string.IsNullOrWhiteSpace(city) && !string.IsNullOrWhiteSpace(state)) {

        //         WebRequest.GetLatLongByCityStateNameAsync(city, state, LatLngResponse => {
        //             LatLngObject = LatLngResponse;
        //         }).Wait();
        //         ViewBag.LatLong = LatLngObject;

        //         if(city == LatLngObject.City && state == LatLngObject.State) {

        //             City newCity = _context.Cities.FirstOrDefault(x => x.Name == city && x.State == state);

        //             if(newCity == null || newCity.Name == null) {
        //                 newCity = new City {
        //                     Name = city,
        //                     State = state
        //                 };

        //                 _context.Cities.Add(newCity);
        //                 _context.SaveChanges();
        //                 newCity = _context.Cities.SingleOrDefault(x => x.Name == city && x.State == state);
        //             }

        //             WebRequest.GetCityAttractionAsync(LatLngObject, attraction_name, null, CityAttractionsResponse => {
        //                 CityAttractionsObject = CityAttractionsResponse;
        //             }).Wait();

        //             if(CityAttractionsObject.Attraction.ToLower() == PARK) {
        //                 AddParks(newCity, CityAttractionsObject);
        //             }

        //             if(CityAttractionsObject.Attraction.ToLower() == BICYCLE_STORE) {
        //                 AddBicycleStores(newCity, CityAttractionsObject);
        //             }


        //         } else {
        //             String error = $"No information for city: {city} in state: {state}";
        //             HttpContext.Session.SetString("Error", error);
        //         }

                
        //         ViewBag.CityAttractions = CityAttractionsObject;

        //     }

        //     return RedirectToAction("Index");
        //     // return View("Dashboard");
        // }


        [HttpPost]
        [Route("select_city")]
        public IActionResult SelectCity(string city, string state, string attraction_name)
        {
            string state_upcase = state.ToUpper();

            HttpContext.Session.SetString("City", city);
            HttpContext.Session.SetString("State", state_upcase);
            HttpContext.Session.SetString("Attraction", attraction_name);
            HttpContext.Session.SetString("Error", "");

            LatLng LatLngObject = new LatLng();
            CityAttractions CityAttractionsObject = new CityAttractions();
            if(!string.IsNullOrWhiteSpace(city) && !string.IsNullOrWhiteSpace(state_upcase)) {

                WebRequest.GetLatLongByCityStateNameAsync(city, state_upcase, LatLngResponse => {
                    LatLngObject = LatLngResponse;
                }).Wait();
                ViewBag.LatLong = LatLngObject;
                HttpContext.Session.SetString("Latitude", LatLngObject.Latitude.ToString());
                HttpContext.Session.SetString("Longitude", LatLngObject.Longitude.ToString());

                if(city.ToUpper() == LatLngObject.City.ToUpper() && state_upcase == LatLngObject.State) {

                    City newCity = _context.Cities.FirstOrDefault(x => x.Name == city && x.State == state_upcase);

                    if(newCity == null || newCity.Name == null) {
                        newCity = new City {
                            Name = city,
                            State = state_upcase
                        };

                        _context.Cities.Add(newCity);
                        _context.SaveChanges();
                        newCity = _context.Cities.SingleOrDefault(x => x.Name == city && x.State == state);
                    }

                    AddAttractions(LatLngObject, attraction_name, CityAttractionsObject, newCity);

                } else {
                    String error = $"No information for city: {city} in state: {state_upcase}";
                    HttpContext.Session.SetString("Error", error);
                }

                
                ViewBag.CityAttractions = CityAttractionsObject;

            }

            return RedirectToAction("Index");
        }

        private void AddAttractions(LatLng LatLngObject, string attraction_name, CityAttractions CityAttractionsObject, City newCity) {
            string placesApiKey = "AIzaSyAgsubmt3p8asFrV-x_x_cEInQokzf-X9s";
            string uri = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={LatLngObject.Latitude},{LatLngObject.Longitude}&types={attraction_name}&radius=3200&key={placesApiKey}";
            
            // Check if the attraction has already been retrieved, and has been stored in the database.
            if(attraction_name == PARK) {
                List<Park> parks = _context.Parks.Where(x => x.CityId == newCity.Id).ToList();
                if (parks.Count() != 0) {
                    return;
                }
            }
            else if(attraction_name == BAR) {
                List<Bar> bars = _context.Bars.Where(x => x.CityId == newCity.Id).ToList();
                if (bars.Count() != 0) {
                    return;
                }
            }
            else if(attraction_name == BICYCLE_STORE) {
                List<BicycleStore> bicycleStores = _context.BicycleStores.Where(x => x.CityId == newCity.Id).ToList();
                if (bicycleStores.Count() != 0) {
                    return;
                }
            }
            else if(attraction_name == CAFE) {
                List<Cafe> cafes = _context.Cafes.Where(x => x.CityId == newCity.Id).ToList();
                if (cafes.Count() != 0) {
                    return;
                }
            }
            else if(attraction_name == CONVENIENCE_STORE) {
                List<ConvenienceStore> ConvenienceStores = _context.ConvenienceStores.Where(x => x.CityId == newCity.Id).ToList();
                if (ConvenienceStores.Count() != 0) {
                    return;
                }
            }
            else if(attraction_name == RESTAURANT) {
                List<Restaurant> Restaurants = _context.Restaurants.Where(x => x.CityId == newCity.Id).ToList();
                if (Restaurants.Count() != 0) {
                    return;
                }
            }
            else if(attraction_name == STARBUCKS) {
                uri = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={LatLngObject.Latitude},{LatLngObject.Longitude}&name=starbucks&radius=3200&key={placesApiKey}";
                List<Starbuck> Starbucks = _context.Starbucks.Where(x => x.CityId == newCity.Id).ToList();
                if (Starbucks.Count() != 0) {
                    return;
                }
            }

            bool nextPage = true;

            while(nextPage) {
                WebRequest.GetCityAttractionAsync(LatLngObject, attraction_name, uri, CityAttractionsResponse => {
                    CityAttractionsObject = CityAttractionsResponse;
                }).Wait();

                if(CityAttractionsObject.Attraction.ToLower() == PARK) {
                    AddParks(newCity, CityAttractionsObject);
                }

                else if(CityAttractionsObject.Attraction.ToLower() == BAR) {
                    AddBars(newCity, CityAttractionsObject);
                }

                else if(CityAttractionsObject.Attraction.ToLower() == BICYCLE_STORE) {
                    AddBicycleStores(newCity, CityAttractionsObject);
                }

                else if(CityAttractionsObject.Attraction.ToLower() == CAFE) {
                    AddCafes(newCity, CityAttractionsObject);
                }

                else if(CityAttractionsObject.Attraction.ToLower() == CONVENIENCE_STORE) {
                    AddConvenienceStores(newCity, CityAttractionsObject);
                }

                else if(CityAttractionsObject.Attraction.ToLower() == RESTAURANT) {
                    AddRestaurants(newCity, CityAttractionsObject);
                }

                else if(CityAttractionsObject.Attraction.ToLower() == STARBUCKS) {
                    AddStarbucks(newCity, CityAttractionsObject);
                }

                string nextPageToken = CityAttractionsObject.NextPageToken;
                if(nextPageToken != null) {
                    Thread.Sleep(2000);
                    uri = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={LatLngObject.Latitude},{LatLngObject.Longitude}&types={attraction_name}&radius=3200&pagetoken={nextPageToken}&key={placesApiKey}";

                    if(CityAttractionsObject.Attraction.ToLower() == STARBUCKS) {
                        uri = $"https://maps.googleapis.com/maps/api/place/nearbysearch/json?location={LatLngObject.Latitude},{LatLngObject.Longitude}&name=starbucks&radius=3200&pagetoken={nextPageToken}&key={placesApiKey}";
                    }
                }
                else {
                    nextPage = false;
                }
            }
        }

        private void AddParks(City newCity, CityAttractions CityAttractionsObject) {
            // List<Park> parks = _context.Parks.Where(x => x.CityId == newCity.Id).ToList();
            // if (parks.Count() == 0) {
                foreach(Attraction attraction in CityAttractionsObject.Attractions) {
                    Park newPark = new Park {
                        CityId = newCity.Id,
                        Name = attraction.Name,
                        Vicinity = attraction.Vicinity,
                        Latitude = attraction.Latitude,
                        Longitude = attraction.Longitude
                    };

                    _context.Parks.Add(newPark);
                }
                _context.SaveChanges();
            // }
        }

        private void AddBars(City newCity, CityAttractions CityAttractionsObject) {
                foreach(Attraction attraction in CityAttractionsObject.Attractions) {
                    Bar newBar = new Bar {
                        CityId = newCity.Id,
                        Name = attraction.Name,
                        Vicinity = attraction.Vicinity,
                        Latitude = attraction.Latitude,
                        Longitude = attraction.Longitude
                    };

                    _context.Bars.Add(newBar);
                }
                _context.SaveChanges();
        }
        private void AddBicycleStores(City newCity, CityAttractions CityAttractionsObject) {
            // List<BicycleStore> bicycleStores = _context.BicycleStores.Where(x => x.CityId == newCity.Id).ToList();
            // if (bicycleStores.Count() == 0) {
                foreach(Attraction attraction in CityAttractionsObject.Attractions) {
                    BicycleStore newBicycleStore = new BicycleStore {
                        CityId = newCity.Id,
                        Name = attraction.Name,
                        Vicinity = attraction.Vicinity,
                        Latitude = attraction.Latitude,
                        Longitude = attraction.Longitude
                    };

                    _context.BicycleStores.Add(newBicycleStore);
                }
                _context.SaveChanges();
            // }
        }

        private void AddCafes(City newCity, CityAttractions CityAttractionsObject) {
                foreach(Attraction attraction in CityAttractionsObject.Attractions) {
                    Cafe newCafe = new Cafe {
                        CityId = newCity.Id,
                        Name = attraction.Name,
                        Vicinity = attraction.Vicinity,
                        Latitude = attraction.Latitude,
                        Longitude = attraction.Longitude
                    };

                    _context.Cafes.Add(newCafe);
                }
                _context.SaveChanges();
        }

        private void AddConvenienceStores(City newCity, CityAttractions CityAttractionsObject) {
            foreach(Attraction attraction in CityAttractionsObject.Attractions) {
                ConvenienceStore newConvenienceStore = new ConvenienceStore {
                    CityId = newCity.Id,
                    Name = attraction.Name,
                    Vicinity = attraction.Vicinity,
                    Latitude = attraction.Latitude,
                    Longitude = attraction.Longitude
                };

                _context.ConvenienceStores.Add(newConvenienceStore);
            }
            _context.SaveChanges();
        }

        private void AddRestaurants(City newCity, CityAttractions CityAttractionsObject) {
            foreach(Attraction attraction in CityAttractionsObject.Attractions) {
                Restaurant newRestaurant = new Restaurant {
                    CityId = newCity.Id,
                    Name = attraction.Name,
                    Vicinity = attraction.Vicinity,
                    Latitude = attraction.Latitude,
                    Longitude = attraction.Longitude
                };

                _context.Restaurants.Add(newRestaurant);
            }
            _context.SaveChanges();
        }

        private void AddStarbucks(City newCity, CityAttractions CityAttractionsObject) {
            foreach(Attraction attraction in CityAttractionsObject.Attractions) {

                if (attraction.Name.Contains("Starbucks")) {
                    Starbuck newStarbuck = new Starbuck {
                        CityId = newCity.Id,
                        Name = attraction.Name,
                        Vicinity = attraction.Vicinity,
                        Latitude = attraction.Latitude,
                        Longitude = attraction.Longitude
                    };

                    _context.Starbucks.Add(newStarbuck);
                }

            }
            _context.SaveChanges();
        }


    }
}
