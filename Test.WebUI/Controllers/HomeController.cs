using Adverthouse.Common.Data.Caching;
using Adverthouse.Common.NoSQL;
using Adverthouse.Core.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
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

        public IActionResult Fill() {
 
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
            string key = "a18da5868a4e4123bbc22ea2355a1012";

            var result = SecurityUtility.Encrypt(key,"Merhaba yunus");
            var temop = SecurityUtility.Decrypt(key, result);


            List<int> _get() => new List<int> { 1, 3, 4 };


            if (rrs == null)
            {
               rrs = new RefreshableStaticData<List<int>>(TimeSpan.FromSeconds(20), _get, () => DateTime.Now , false);
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
}
