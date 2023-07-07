using Adverthouse.Core.TcpPooling;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Adverthouse.Common.Data.RocksDB
{
    public class RocksDBClient
    {  
        private static HttpClient client = new HttpClient(new HttpClientHandler()
        {            
            Proxy = null,
            UseProxy = false,
            MaxConnectionsPerServer = int.MaxValue,
            AllowAutoRedirect = false,
            UseCookies = false
        });          
       
        private static string _tcpHostAddress = "";

        public RocksDBClient(string tcpHostAdress = "127.0.0.1")
        {
            _tcpHostAddress = tcpHostAdress;

            if (client.BaseAddress == null)
            {
                client.BaseAddress = new Uri("http://" + _tcpHostAddress + ":3800/");
                client.DefaultRequestHeaders.Accept.Clear();
            } 
        }
        private static async Task<byte[]> SerializeAndCompressAsync<T>(T obj, CancellationToken cancel = default(CancellationToken))
        {
            using (var outputStream = new MemoryStream())
            {
                using (var compressionStream = new GZipStream(outputStream, CompressionMode.Compress, true))
                {
                    var bytes = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(obj));

                    await compressionStream.WriteAsync(bytes, 0, bytes.Length, cancel);
                }
                return outputStream.ToArray();
            }
        }

        private static async Task<T> DecompressAndDeserializeAsync<T>(byte[] bytes, CancellationToken cancel = default(CancellationToken))
        {
            using (var inputStream = new MemoryStream(bytes))
            {
                using (var outputStream = new MemoryStream())
                {
                    using (var compressionStream = new GZipStream(inputStream, CompressionMode.Decompress))
                    {
                        await compressionStream.CopyToAsync(outputStream, cancel);
                        var bytesOut = outputStream.ToArray();

                        return JsonConvert.DeserializeObject<T>(Encoding.UTF8.GetString(bytesOut));
                    }
                }
            }
        }

        public static async Task<RocksDBResponse<string>> AddAsync(string dbName, string key, string value)
        {
            HttpResponseMessage response = await client.PutAsJsonAsync(
                $"api/Add/{dbName}", new KeyValuePair<string, string>(key, value));
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<RocksDBResponse<string>>();
        }

        public static async Task<RocksDBResponse<string>> GetAsync(string dbName, string key)
        {
            var value = new RocksDBResponse<string>();
            HttpResponseMessage response = await client.GetAsync($"api/get/{dbName}/{key}");
            if (response.IsSuccessStatusCode)
            {
                value = await response.Content.ReadAsAsync<RocksDBResponse<string>>() ?? new RocksDBResponse<string>();
            }
            return value;
        }

        public static async Task<RocksDBResponse<string>> AddAsByteAsync<T>(string dbName, string key, T value)
        {
            var data = await SerializeAndCompressAsync<T>(value);

            HttpResponseMessage response = await client.PutAsJsonAsync("api/AddAsByte/" + dbName,
                    new KeyValuePair<string, byte[]>(key, data));

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<RocksDBResponse<string>>();
        }

        public static async Task<RocksDBResponse<T>> GetFromByteAsAsync<T>(string dbName, string key) where T : class
        {
           var value = new RocksDBResponse<T>();
            HttpResponseMessage response = await client.GetAsync($"api/GetAsByte/{dbName}/{key}");
            if (response.IsSuccessStatusCode)
            {
                RocksDBResponse<byte[]> tempValue = await response.Content.ReadAsAsync<RocksDBResponse<byte[]>>() ?? new RocksDBResponse<byte[]>();

                value.Data = await DecompressAndDeserializeAsync<T>(tempValue.Data);
                
            }
            return value;
        }

        public static async Task<RocksDBResponse<string>> DeleteAsync(string key)
        {

            HttpResponseMessage response = await client.DeleteAsync($"api/delete/{key}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<RocksDBResponse<string>>();
        }

        public static async Task<RocksDBResponse<string>> DeleteAsync(string dbName, string key)
        {

            HttpResponseMessage response = await client.DeleteAsync($"api/delete/{dbName}/{key}");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsAsync<RocksDBResponse<string>>();
        }
    }
}
