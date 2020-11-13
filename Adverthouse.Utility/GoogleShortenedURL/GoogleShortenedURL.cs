using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Adverthouse.Utility.GoogleShortenedURL
{
    public class GoogleShortenedURL
    {
        private string _key;
        public string GetShortURL(string longUrl)
        {
            string googReturnedJson = string.Empty;

            GoogleShortenedURLRequest googSentJson = new GoogleShortenedURLRequest();
            googSentJson.longUrl = longUrl;
            string jsonData = JsonConvert.SerializeObject(googSentJson, Formatting.Indented);

            byte[] bytebuffer = Encoding.UTF8.GetBytes(jsonData);
            try
            {
                WebRequest webreq = WebRequest.Create("https://www.googleapis.com/urlshortener/v1/url?key=" + _key);
                webreq.Method = WebRequestMethods.Http.Post;
                webreq.ContentLength = bytebuffer.Length;
                webreq.ContentType = "application/json";

                using (Stream stream = webreq.GetRequestStream())
                {
                    stream.Write(bytebuffer, 0, bytebuffer.Length);
                    stream.Close();
                }

                using (HttpWebResponse webresp = (HttpWebResponse)webreq.GetResponse())
                {
                    using (Stream dataStream = webresp.GetResponseStream())
                    {
                        using (StreamReader reader = new StreamReader(dataStream))
                        {
                            googReturnedJson = reader.ReadToEnd();
                        }
                    }
                }

                GoogleShortenedURLResponse googUrl = JsonConvert.DeserializeObject<GoogleShortenedURLResponse>(googReturnedJson);

                return googUrl.id;
            }
            catch
            {
                return longUrl;
            }
        }

        public GoogleShortenedURL(string key)
        {
            _key = key;
        }
    }
}
    