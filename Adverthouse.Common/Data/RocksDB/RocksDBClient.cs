using System;
using System.Collections.Generic; 
using System.Net.Http;
using System.Net.Http.Headers;
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
            RocksDBResponse value = null;
            HttpResponseMessage response = await client.GetAsync($"api/get/{key}");
            if (response.IsSuccessStatusCode)
            {
                value = await response.Content.ReadAsAsync<RocksDBResponse>();
            }
            return value;
        }
        public static async Task<RocksDBResponse> GetAsync(string dbName,string key)
        {
            RocksDBResponse value = null;
            HttpResponseMessage response = await client.GetAsync($"api/get/{dbName}/{key}");
            if (response.IsSuccessStatusCode)
            {
                value = await response.Content.ReadAsAsync<RocksDBResponse>();
            }
            return value;
        }
        public static async Task<RocksDBResponse> AddAsync(string key, string value)
        {

            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"api/Add", new KeyValuePair<string, string>(key, value));
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<RocksDBResponse>();
        }
        public static async Task<RocksDBResponse> AddAsync(string dbName,string key, string value)
        {

            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"api/Add/{dbName}", new KeyValuePair<string, string>(key, value));
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<RocksDBResponse>();
        }
        public static async Task<RocksDBResponse> DeleteAsync(string key)
        {
            RocksDBResponse value = null;
            HttpResponseMessage response = await client.DeleteAsync($"api/delete/{key}");
            if (response.IsSuccessStatusCode)
            {
                value = await response.Content.ReadAsAsync<RocksDBResponse>();
            }
            return value;
        }
        public static async Task<RocksDBResponse> DeleteAsync(string dbName,string key)
        {
            RocksDBResponse value = null;
            HttpResponseMessage response = await client.DeleteAsync($"api/delete/{dbName}/{key}");
            if (response.IsSuccessStatusCode)
            {
                value = await response.Content.ReadAsAsync<RocksDBResponse>();
            }
            return value;
        }
    }
}
