using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Album.Models;

namespace Album.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {

        private AlbumDbContext _context;
        private string _path;

        public AdminController(AlbumDbContext context, IWebHostEnvironment hostEnvironment)
        {

            _context = context;
            _path = $"{hostEnvironment.WebRootPath}\\Album";
            
        }

        //Admin/Index，取得所有分類記錄
        public IActionResult Index()
        {
            var categories = _context.TCategories.OrderByDescending(m => m.FCid).ToList();
            return View(categories);
        }

        //Admin/CategoryDelete，依所選取的Cid類別編號，刪除該分類與該分類的所有照片
        public IActionResult CategoryDelete(int Cid)
        {
            //取得分類
            var category = _context.TCategories.FirstOrDefault(m => m.FCid == Cid);
            //取得該分類的所有照片
            var albums = _context.TAlbums.Where(m => m.FCid == Cid);

            //刪除該分類的所有照片圖檔
            foreach (var item in albums)
            {
                System.IO.File.Delete($"{_path}\\{item.FAlbum}");
            }
            //刪除該分類及該分類的所有照片
            _context.TAlbums.RemoveRange(albums);
            _context.TCategories.Remove(category);
            _context.SaveChanges();
            TempData["Success"] = "相簿分類刪除成功";
            return RedirectToAction("Index");
        }

        //Admin/CategoryCreate，顯示新增類別畫面
        public IActionResult CategoryCreate()
        {
            return View();
        }

        //新增類別作業
        [HttpPost]
        public IActionResult CategoryCreate(TCategory category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.TCategories.Add(category);
                    _context.SaveChanges();
                    TempData["Success"] = "相簿分類新增成功";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "相簿分類新增失敗";
                }
            }
            return View(category);
        }

        //Admin/CategoryEdit，依Cid分類編號取得要修改的相簿分類記錄
        public IActionResult CategoryEdit(int Cid)
        {
            var category = _context.TCategories.FirstOrDefault(m => m.FCid == Cid);
            return View(category);
        }

        //修改相簿分類
        [HttpPost]
        public IActionResult CategoryEdit(TCategory category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    int Cid = category.FCid;
                    var temp = _context.TCategories.FirstOrDefault(m => m.FCid == Cid);
                    temp.FCname = category.FCname;
                    _context.SaveChanges();
                    TempData["Success"] = "相簿分類修改成功";
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "相簿分類名稱無法修改，請重新檢視修改資料";
                }
            }
            return View(category);
        }

        //Admin/AlbumCategory，依相簿分類編號取得該分類的所有照片
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

        //Admin/AlbumDelete，依相片編號刪除照片
        public IActionResult AlbumDelete(int AlbumId)
        {
            //依AlbumId相片編號找出要刪除的照片
            var album = _context.TAlbums.FirstOrDefault(m => m.FAlbumId == AlbumId);

            //刪除照片
            System.IO.File.Delete($"{_path}\\{album.FAlbum}");

            //將符合的照片刪除
            _context.TAlbums.Remove(album);
            _context.SaveChanges();
            TempData["Success"] = "照片刪除成功";
            return RedirectToAction("AlbumCategory", new { Cid = album.FCid });
        }

        //Admin/MemberList，取得所有會員記錄
        public IActionResult MemberList()
        {
            var members = _context.TMembers.Where(m => m.FRole == "Member").ToList();
            return View(members);
        }

        //Admin/MemberCreate，顯示新增會員畫面
        public IActionResult MemberCreate()
        {
            return View();
        }

        //會員新增畫面按下Sumbit執行
        [HttpPost]
        public IActionResult MemberCreate(TMember member)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    member.FRole = "Member";
                    _context.TMembers.Add(member);
                    _context.SaveChanges();
                    TempData["Success"] = "會員新增成功";
                    return RedirectToAction("MemberList");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "會員新增失敗，帳號可能重複";
                }
            }
            return View(member);
        }

        //Admin/MemberEdit，依所選取的Uid，修改會員記錄
        public IActionResult MemberEdit(string Uid)
        {
            //取得欲修改的會員
            var member = _context.TMembers.FirstOrDefault(m => m.FUid == Uid);
            return View(member);
        }

        //在編輯會員頁面按Submit執行
        [HttpPost]
        public IActionResult MemberEdit(TMember member)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string uid = member.FUid;
                    var temp = _context.TMembers.FirstOrDefault(m => m.FUid == uid);
                    temp.FName = member.FName;
                    temp.FPwd = member.FPwd;
                    temp.FMail = member.FMail;
                    _context.SaveChanges();
                    TempData["Success"] = "會員修改成功";
                    return RedirectToAction("MemberList");
                }
                catch (Exception ex)
                {
                    TempData["Error"] = "會員資料無法修改，請重新檢視修改資料";
                }
            }
            return View(member);
        }

        //Admin/MemberDelete，依所選取的Uid，刪除該會員
        public IActionResult MemberDelete(string Uid)
        {
            //取得會員
            var member = _context.TMembers.FirstOrDefault(m => m.FUid == Uid);
            //刪除該位會員記錄
            _context.TMembers.Remove(member);
            _context.SaveChanges();
            TempData["Success"] = "會員刪除成功";
            return RedirectToAction("MemberList");
        }
    }
}
