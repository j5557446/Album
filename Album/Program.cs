using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Album.Models;


var builder = WebApplication.CreateBuilder(args);

var cnstr = $"Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename={builder.Environment.ContentRootPath}App_Data\\dbAlbum.mdf;Integrated Security=True;Trusted_Connection=True;";

builder.Services.AddMvc();
builder.Services.AddDbContext<AlbumDbContext>
    (optons => optons.UseSqlServer(cnstr));

//�W�[���Ҥ覡�A�ϥ� cookie ����
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options => {
    //�s��������cookie �u��g��HTTP(S) ��w�Ӧs��
    options.Cookie.HttpOnly = true;
    //���n�J�ɷ|�۰ʾɨ�n�J��
    options.LoginPath = new PathString("/Home/Login");
    //���v�������ڵ��X�ݷ|�۰ʾɨ즹���|
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

app.UseCookiePolicy();      //�ϥ�Cookie Policy
app.UseAuthentication();    //�ϥΨ������� 
app.UseAuthorization();

app.UseExceptionHandler("/Home/Error");


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
