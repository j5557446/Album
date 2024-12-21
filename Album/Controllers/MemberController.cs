using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Album.Models;

namespace Album.Controllers
{
    [Authorize(Roles = "Member")]
    public class MemberController : Controller
    {
        private AlbumDbContext _context;
        private string _path;
        public MemberController(AlbumDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _path = $"{hostEnvironment.WebRootPath}\\Album";
        }
        //Member/Index，取得所有分類記錄
        public IActionResult Index()
        {
            var categories = _context.TCategories.OrderByDescending(m => m.FCid).ToList();
            return View(categories);
        }



        public IActionResult AlbumCategory(int Cid = 1)
        {
            //取得相簿分類名稱
            ViewBag.CategoryName = _context.TCategories
                .FirstOrDefault(m => m.FCid == Cid)
                .FCname;

            var albums = _context.TAlbums
                .Where(m => m.FCid == Cid)
                .OrderByDescending(m => m.FReleaseTime)
                .ToList();
            return View(albums);
        }


        //Member/AlbumDelete，依相片編號刪除相片
        public IActionResult AlbumDelete(int AlbumId)
        {
            //依AlbumId相片編號找出要刪除的照片
            var album = _context.TAlbums.FirstOrDefault(m => m.FAlbumId == AlbumId);

            //刪除照片
            System.IO.File.Delete($"{_path}\\{album.FAlbum}");

            //將符合的相片刪除
            _context.TAlbums.Remove(album);
            _context.SaveChanges();
            TempData["Success"] = "照片刪除成功";
            return RedirectToAction("AlbumCategory", new { Cid = album.FCid });
        }

    }
}
