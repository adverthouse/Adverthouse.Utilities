using Adverthouse.Core.TcpPooling;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Sockets;
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

        private static JsonSerializer serializer = new JsonSerializer(); 

        public RocksDBClient(string tcpHostAdress = "127.0.0.1",int port = 38670, int minPoolSize = 20, int maxPoolSize = 5000, int socketRecycleAgeAsMinute = 15)
        { 
            if (client.BaseAddress == null)
            {
                client.BaseAddress = new Uri("http://" + tcpHostAdress + ":3800/");
                client.DefaultRequestHeaders.Accept.Clear();
            }

            TcpConnectionPool.InitializeConnectionPool(tcpHostAdress, port, minPoolSize, maxPoolSize, socketRecycleAgeAsMinute);
        }

        public static T GetDataOverTCP<T>(string dbName, string key)
        {
            var CustomSocket = TcpConnectionPool.GetSocket();
            
            try
            {
                byte[] sendData = Encoding.ASCII.GetBytes($"get {dbName} {key}");
                string response;
                using (NetworkStream stream = CustomSocket.GetStream())
                {
                    stream.Write(sendData, 0, sendData.Length);
 
                    using(MemoryStream buffer = new MemoryStream())
                    {
                        int b; 
                        while ((b = stream.ReadByte()) != -1)
                        {
                            buffer.WriteByte((byte)b);
                        } 
                        response = Encoding.UTF8.GetString(buffer.GetBuffer());
                    }
                }
                return JsonConvert.DeserializeObject<T>(response);
            }
            finally
            {
                TcpConnectionPool.PutSocket(CustomSocket);
            }
        }
        public static async Task<T> GetDataOverTCPAsync<T>(string dbName, string key)
        {
            var CustomSocket = TcpConnectionPool.GetSocket();

            try
            {
                byte[] sendData = Encoding.ASCII.GetBytes($"get {dbName} {key}");
                string response;
                using (NetworkStream stream = CustomSocket.GetStream())
                {
                    await stream.WriteAsync(sendData, 0, sendData.Length);
 
                    using(MemoryStream buffer = new MemoryStream())
                    {
                        int b; 
                        while ((b = stream.ReadByte()) != -1)
                        {
                            buffer.WriteByte((byte)b);
                        } 
                        response = Encoding.UTF8.GetString(buffer.GetBuffer());
                    }
                }
                return JsonConvert.DeserializeObject<T>(response);
            }
            finally
            {
                TcpConnectionPool.PutSocket(CustomSocket);
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
            string value = "";
            HttpResponseMessage response = await client.GetAsync($"api/GetAsString/{dbName}/{key}");
            if (response.IsSuccessStatusCode)
            {
                value = await response.Content.ReadAsStringAsync();
            }
            return value;
        }

        public static async Task<T> GetStringAsObjectAsync<T>(string dbName, string key)
        {            
            using (Stream s = await client.GetStreamAsync($"api/GetAsString/{dbName}/{key}"))
            using (StreamReader sr = new StreamReader(s))
            using (JsonReader reader = new JsonTextReader(sr))
            {
                return serializer.Deserialize<T>(reader);
            }
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
