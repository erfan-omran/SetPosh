using DataBase;
using Microsoft.AspNetCore.Authentication.Cookies;
using Service.Service;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("SetPoshConnection") ?? "";
DBConnection.InitConnectionStr(connectionString);  // Set ConnectionStr

builder.Services.AddSingleton<UserService>();

builder.Services.AddControllersWithViews();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.ExpireTimeSpan = TimeSpan.FromDays(14);
        options.LoginPath = "/Home/Login"; // مسیر صفحه لاگین
        options.AccessDeniedPath = "/Home/Error"; // مسیر دسترسی غیرمجاز
    });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();// فعال کردن Middleware برای احراز هویت
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Login}/{id?}");

app.Run();
