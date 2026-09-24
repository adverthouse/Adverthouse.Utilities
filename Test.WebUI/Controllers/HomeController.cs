using Adverthouse.Common.Data.Caching;
using Adverthouse.Common.NoSQL;
using Adverthouse.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Test.WebUI.Models;
using Test.WebUI.Models.Services;
using Test.WebUI.Validators;

namespace Test.WebUI.Controllers
{
    public static class AdminDefaults
    {
        public static NoSQLKey RoleByIDCacheKey => new NoSQLKey("Mem.RolesByID-{0}");
        public static NoSQLKey RefreshRoleByIDCacheKey => new NoSQLKey("Mem.Refresh.RolesByID-{0}");

        public static NoSQLKey LastUpdateDateTIC => new NoSQLKey("Mem.TotalItemCount");
    }
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ICacheManager<MemoryCacheManager> _cacheManager;
        private readonly ICategoryService _categoryService;
        private readonly IMemoryCache _memoryCache;

        public HomeController(ILogger<HomeController> logger,
            ICacheManager<MemoryCacheManager> cacheManager, ICategoryService categoryService, IMemoryCache memoryCache)
        {
            _logger = logger;
            _cacheManager = cacheManager;
            _categoryService = categoryService;
            _memoryCache = memoryCache;
        }

        public IActionResult Fill()
        {

            /*
                 _categoryService.Create(new Category()
                 {
                     CategoryID = 1,
                     CategoryName = "Elektronik",
                     TotalDownloadCount = 10,
                     TotalViewCount = 20
                 });
                 _categoryService.Create(new Category()
                 {
                     CategoryID = 2,
                     CategoryName = "Ev & Yaşam",
                     TotalDownloadCount = 3,
                     TotalViewCount = 4
                 }); */
            string temp = "Data set";
            _categoryService.AllZero();
            temp = "All zero";

            /*
        var lst = new List<CategoryStat>();
        lst.Add(new CategoryStat()
        {
            CategoryID = 1, 
            TotalDownloadCount = 100,
            TotalViewCount = 200
        });
        lst.Add(new CategoryStat()
        {
            CategoryID = 2, 
            TotalDownloadCount = 300,
            TotalViewCount = 400
        });


        _categoryService.UpdateAllElastic(lst);
*/

            return Ok("");
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

        private static long _lockFlag = DateTime.Now.Ticks; // 0 - free

        private static string abc = "deneme";


        private static RefreshableStaticData<List<int>> rrs;

        public IActionResult Index()
        {
            string key = "a18da5868a4e4133bbc22ea2355a1012";


            var secret = "test-secret";

            var data = new TesetData()
            {
                MyProperty = 123,
                Name = @"orem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since 1966, when designers at Letraset and James Mosley, the librarian at St Bride Printing Library in London, took a 1914 Cicero translation and scrambled it to make dummy text for Letraset's Body Type sheets. It has survived not only many decades, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised thanks to these sheets and more recently with desktop publishing software like Aldus PageMaker and Microsoft Word including versions of Lorem Ipsum.

Why do we use it?
It is a long established fact that a reader will be distracted by the readable content of a page when looking at its layout. The point of using Lorem Ipsum is that it has a more-or-less normal distribution of letters, as opposed to using 'Content here, content here', making it look like readable English. Many desktop publishing packages and web page editors now use Lorem Ipsum as their default model text, and a search for 'lorem ipsum' will uncover many web sites still in their infancy. Various versions have evolved over the years, sometimes by accident, sometimes on purpose (injected humour and the like).


Where does it come from?
Contrary to popular belief, Lorem Ipsum is not simply random text. It has roots in a piece of classical Latin literature from 45 BC, making it over 2000 years old. Richard McClintock, a Latin professor at Hampden-Sydney College in Virginia, looked up one of the more obscure Latin words, consectetur, from a Lorem Ipsum passage, and going through the cites of the word in classical literature, discovered the undoubtable source. Lorem Ipsum comes from sections 1.10.32 and 1.10.33 of de Finibus Bonorum et Malorum (The Extremes of Good and Evil) by Cicero, written in 45 BC. This book is a treatise on the theory of ethics, very popular during the Renaissance. The first line of Lorem Ipsum, Lorem ipsum dolor sit amet.., comes from a line in section 1.10.32.The standard chunk of Lorem Ipsum used since 1966 is reproduced below for those interested. Sections 1.10.32 and 1.10.33 from de Finibus Bonorum et Malorum by Cicero are also reproduced in their exact original form, accompanied by English versions from the 1914 translation by H. Rackham.

",
                Url = "https://www.esigen.com/google/integration-checkpoint/cXpWd2swR2RDSzBVOGtMc1dxeDZ3dEJSOWRvVE41dkFCVWRKeC8wTkVFQUhDRlRDTkpwOHNmVnJvWmttZndXSS92QS95aktjMnVJd3ZlQTV2c2FNSWFaTkF6MWxlVCtuYnVoVm1IQXlOMGNvNXpnL3F1SCtTWVBjeUNoWjZTM21GallXclUybEJoL3RVais2YjJLbHVvOXZvbllPMFZrN2tRZ2ZWNFNYUGZ6V3FHdlE0aUkyUDhTSzlpTlQ2Tk05NHlSSEF3azdLWE1yMXNRM05URzNXZz09"
            };

            var encrypted = SecureURL.Encrypt(data, secret);

            Console.WriteLine($"Encrypted: {encrypted}");
            Console.WriteLine($"Length: {encrypted.Length}");

            var decrypted = SecureURL.Decrypt<TesetData>(encrypted, secret);





            var pass = PBKDF2Hasher.HashPassword("Yunus872.");
            pass = PBKDF2Hasher.HashPassword("Yunus872.");

            bool isValid = PBKDF2Hasher.VerifyPassword("Yunus872.", pass);

            List<int> _get() => new List<int> { 1, 3, 4 };


            if (rrs == null)
            {
                rrs = new RefreshableStaticData<List<int>>(TimeSpan.FromSeconds(20), _get, () => DateTime.Now, false);
            }
            return View(rrs.GetFreshData());
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }

    public class TesetData
    {
        public int MyProperty { get; set; }
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
