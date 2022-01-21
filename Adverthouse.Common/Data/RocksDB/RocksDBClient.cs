using System;
using System.Collections.Generic; 
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;  

namespace Adverthouse.Common.Data.RocksDB
{
    public class RocksDBClient
    {
        private static HttpClient client = new HttpClient();

        public RocksDBClient(string serverUrlBase = "https://localhost:3800/")
        {
            if (client.BaseAddress == null)
            {
                client.BaseAddress = new Uri(serverUrlBase);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(
                    new MediaTypeWithQualityHeaderValue("application/json"));
            }
        }

        public static async Task<StatusInfo> GetAsync(string key)
        {
            StatusInfo value = null;
            HttpResponseMessage response = await client.GetAsync($"get/{key}");
            if (response.IsSuccessStatusCode)
            {
                value = await response.Content.ReadAsAsync<StatusInfo>();
            }
            return value;
        }
        public static async Task<StatusInfo> AddAsync(string key, string value)
        {

            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"Add", new KeyValuePair<string, string>(key, value));
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<StatusInfo>();
        }

        public static async Task<StatusInfo> DeleteAsync(string key)
        {
            StatusInfo value = null;
            HttpResponseMessage response = await client.DeleteAsync($"delete/{key}");
            if (response.IsSuccessStatusCode)
            {
                value = await response.Content.ReadAsAsync<StatusInfo>();
            }
            return value;
        }
    }
}
