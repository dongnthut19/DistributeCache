using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DistributeCache.Data.Repositories;

namespace SampleApp.Pages
{
    #region snippet_IndexModel
    public class IndexModel : PageModel
    {
        private readonly IDistributeCacheRepository _cache;

        public IndexModel(IDistributeCacheRepository cache)
        {
            _cache = cache;
        }

        public string CachedTimeUTC { get; set; }

        public void OnGetAsync()
        {
            CachedTimeUTC = "Cached Time Expired";
            var encodedCachedTimeUTC = _cache.Get("cachedTimeUTC");

            if (encodedCachedTimeUTC != null)
            {
                CachedTimeUTC = encodedCachedTimeUTC;
            }
        }

        public IActionResult OnPostResetCachedTime()
        {
            var currentTimeUTC = DateTime.UtcNow.ToString();

            _cache.UpSert("cachedTimeUTC", currentTimeUTC);

            return RedirectToPage();
        }
    }
    #endregion
}
