using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace Test.WebUI.Models.Services
{
    public class ShutterClient
    {
        private string client_id = "288bead9acc9c5473168"; // Insert your CLIENT ID here.
        private string client_secret = "42ced04868311eabb52eb45147bd1d474ef5baa6"; // Insert your CLIENT SECRET here.

        public ImageLst GetSponsoredImages(int page = 1, int itemPerPage = 20, string q = null, int? categoryid = null, string image_type = "")
        {
            ImageLst ilst = new ImageLst();

            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.shutterstock.com/");

                var username = client_id;
                var password = client_secret;

                string url = "v2/images/search?page=" + page.ToString() + "&per_page=" + itemPerPage.ToString() + "&query=" + q + "&view=minimal"; //&region=tr for region
                url += (categoryid != null ? "&category=" + categoryid.ToString() : "");
                url += (!String.IsNullOrWhiteSpace(image_type) ? "&image_type=" + image_type : "");

                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
                client.DefaultRequestHeaders.Add(HttpRequestHeader.Authorization.ToString(), new List<string> { string.Format("Basic {0}", credentials) });
                client.DefaultRequestHeaders.UserAgent.Clear();
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApp", "1.0"));

                var objText = client.GetStringAsync(url).Result;
                ilst = JsonConvert.DeserializeObject<ImageLst>(objText);
            }
            return ilst;
        }

        public string GetSuggestions(string wholeSeachQuery)
        {            
            using (HttpClient client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://api.shutterstock.com/");

                var username = client_id;
                var password = client_secret;

                string url = "v2/images/search/suggestions";

                string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
                client.DefaultRequestHeaders.Add(HttpRequestHeader.Authorization.ToString(), new List<string> { string.Format("Basic {0}", credentials) });
                client.DefaultRequestHeaders.UserAgent.Clear();
                client.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("MyApp", "1.0"));
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                
                var data = new StringContent(JsonConvert.SerializeObject(new { text = wholeSeachQuery.ToLower() }), Encoding.UTF8, "application/json");


                var objText = client.PostAsync(url, data).Result;
                var suggestions = JsonConvert.DeserializeObject<Suggest>(objText.Content.ReadAsStringAsync().Result);
                return String.Join(" ",suggestions.keywords);
            } 
        }


        public CategoryList GetCategories()
        {

            CategoryList ilst = new CategoryList();

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://api.shutterstock.com/");

                    var username = client_id;
                    var password = client_secret;

                    string credentials = Convert.ToBase64String(Encoding.ASCII.GetBytes(username + ":" + password));
                    client.DefaultRequestHeaders.Add(HttpRequestHeader.Authorization.ToString(), new List<string> { string.Format("Basic {0}", credentials) });

                    var objText = client.GetStringAsync("v2/images/categories").Result;
                    ilst = JsonConvert.DeserializeObject<CategoryList>(objText);
                }
            }
            catch { }
            return ilst;
        }
        public ShutterClient()
        {

        }
    }
}