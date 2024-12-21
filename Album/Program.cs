using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Album.Models;


var builder = WebApplication.CreateBuilder(args);

var cnstr = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={builder.Environment.ContentRootPath}App_Data\\dbAlbum.mdf;Integrated Security=True;Trusted_Connection=True;";

builder.Services.AddMvc();
builder.Services.AddDbContext<AlbumDbContext>
    (optons => optons.UseSqlServer(cnstr));

//增加驗證方式，使用 cookie 驗證
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
    //瀏覽器限制cookie 只能經由HTTP(S) 協定來存取
    options.Cookie.HttpOnly = true;
    //未登入時會自動導到登入頁
    options.LoginPath = new PathString("/Home/Login");
    //當權限不夠拒絕訪問會自動導到此路徑
    options.AccessDeniedPath = new PathString("/Home/NoAuthorization");
});

var app = builder.Build();






// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseCookiePolicy();      //使用Cookie Policy
app.UseAuthentication();    //使用身份驗證 
app.UseAuthorization();

app.UseExceptionHandler("/Home/Error");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
