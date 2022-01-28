using Newtonsoft.Json;
using System;
using System.Collections.Generic; 
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;  

namespace Adverthouse.Common.Data.RocksDB
{
    public class RocksDBClient
    {  private static HttpClient client = new HttpClient(new HttpClientHandler()
        {            
            Proxy = null,
            UseProxy = false,
            MaxConnectionsPerServer = int.MaxValue,
            AllowAutoRedirect = false,
            UseCookies = false
        });

        public RocksDBClient(string serverUrlBase = "http://localhost:3800/")
        {
            if (client.BaseAddress == null)
            { 
                client.BaseAddress = new Uri(serverUrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        public static async Task<RocksDBResponse> GetAsync(string key)
        {
            RocksDBResponse value = new RocksDBResponse();
            HttpResponseMessage response = await client.GetAsync($"api/get/{key}");
            if (response.IsSuccessStatusCode)
            {
                value = await response.Content.ReadAsAsync<RocksDBResponse>() ?? new RocksDBResponse();
            }
            return value;
        }
        public static async Task<RocksDBResponse> GetAsync(string dbName, string key)
        {
            RocksDBResponse value = new RocksDBResponse();
            HttpResponseMessage response = await client.GetAsync($"api/get/{dbName}/{key}");
            if (response.IsSuccessStatusCode)
            {
                value = await response.Content.ReadAsAsync<RocksDBResponse>() ?? new RocksDBResponse();
            }
            return value;
        }
    
        public static async Task<string> GetStringAsync(string dbName, string key)
        {
            RocksDBResponse value = new RocksDBResponse();
            var response = await client.GetStringAsync($"api/GetAsString/{dbName}/{key}");

            return response;
        }
        public static async Task<T> GetAsByteAsync<T>(string dbName, string key) where T : class
        {
            var response = await client.GetStringAsync($"api/GetAsByte/{dbName}/{key}");

            return JsonConvert.DeserializeObject<T>(response);
        }

        public static async Task<RocksDBResponse> AddAsync(string dbName, string key, string value)
        {

            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"api/Add/{dbName}", new KeyValuePair<string, string>(key, value));
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<RocksDBResponse>();
        }

        public static async Task<RocksDBResponse> AddAsByteAsync<T>(string dbName, string key, T value)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"api/AddAsByte/{dbName}", new KeyValuePair<string, byte[]>(key, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value))));
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<RocksDBResponse>();
        }

        public static async Task<RocksDBResponse> DeleteAsync(string key)
        {

            HttpResponseMessage response = await client.DeleteAsync($"api/delete/{key}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<RocksDBResponse>();
        }

        public static async Task<RocksDBResponse> DeleteAsync(string dbName, string key)
        {

            HttpResponseMessage response = await client.DeleteAsync($"api/delete/{dbName}/{key}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<RocksDBResponse>();
        }
    }
}
