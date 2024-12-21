using Album.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Diagnostics;

namespace Album.Controllers
{
    public class HomeController : Controller
    {
        private AlbumDbContext _context;
        private string _path;

        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger,AlbumDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _logger = logger;
            _context = context;
            _path = $"{hostEnvironment.WebRootPath}\\Album";
        }

        public IActionResult Index()
        {
            var hotAlbums = _context.TAlbums
               .OrderByDescending(m => m.FReleaseTime)
               .Take(12)
               .ToList();

            return View(hotAlbums);
        }

        //Home/AlbumCategory，依Cid分類編號取得照片記錄
        public IActionResult AlbumCategory(int Cid = 1)
        {
            //依Cid分類編號取得相簿分類名稱
            ViewBag.CategoryName = _context.TCategories
                .FirstOrDefault(m => m.FCid == Cid)
                .FCname;
            //依Cid分類編號取得該分類的所有照片，並依發佈時間進行遞減排序
            var albums = _context.TAlbums
                .Where(m => m.FCid == Cid)
                .OrderByDescending(m => m.FReleaseTime)
                .ToList();
            return View(albums);

        }

        //Home/AlbumUpload，顯示照片上傳的頁面
        public IActionResult AlbumUpload()
        {
            return View();
        }

        //照片上傳頁面按下Submit鈕執行
        [HttpPost]
        public async Task<IActionResult> AlbumUpload(TAlbum album, IFormFile formFile)
        {
            TempData["Error"] = "資料無法上傳，請記得上傳照片並檢視資料";
            if (ModelState.IsValid)
            {
                if (formFile != null)
                {
                    if (formFile.Length > 0)
                    {
                        //照片上傳
                        string fileName = $"{Guid.NewGuid().ToString()}.jpg";
                        string savePath = $"{_path}\\{fileName}";
                        using (var stream = new FileStream(savePath, FileMode.Create))
                        {
                            await formFile.CopyToAsync(stream);
                        }
                        //照片記錄寫入
                        album.FAlbum = fileName;
                        album.FReleaseTime = DateTime.Now;
                        _context.TAlbums.Add(album);
                        _context.SaveChanges();
                        TempData["Success"] = "照片上傳成功";
                        return RedirectToAction("AlbumCategory", new { Cid = album.FCid });
                    }
                }
            }
            return View(album);
        }

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string uid, string pwd)
        {
            // 依uid和pwd取得會員
            var member = _context.TMembers
                .FirstOrDefault(m => m.FUid == uid && m.FPwd == pwd);
            // 若member非null，表示有該位會員
            if (member != null)
            {
                IList<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, member.FUid),
                        new Claim(ClaimTypes.Role, member.FRole)
                    };
                var claimsIndentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = true };
                HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                    new ClaimsPrincipal(claimsIndentity),
                    authProperties);
                TempData["Success"] = "登入成功";
                return RedirectToAction("Index", member.FRole);   //前往會員對應的控制器
            }
            TempData["Error"] = "帳密錯誤，請重新檢查";
            return View();
        }

        //Home/Logout，登出作業
        public IActionResult Logout()
        {
            HttpContext.SignOutAsync();
            return RedirectToAction("Index");
        }

        //權限不足會執行此處
        public IActionResult NoAuthorization()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            var exceptionDetails = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            ViewBag.ExceptionPath = exceptionDetails?.Path;
            ViewBag.ErrorMessage = exceptionDetails?.Error?.Message;
            //ViewBag.StackTrace = exceptionDetails?.Error?.StackTrace;
            _logger.LogError($"路徑{ViewBag.ExceptionPath}，錯誤訊息{ViewBag.ErrorMessage}");
            return View();
        }
    }
}
