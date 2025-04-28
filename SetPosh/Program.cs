using Core;
using Core.Enum;
using DataBase;
using Service;
using SetPosh;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("SetPoshConnection") ?? "";
DBConnection.InitConnectionStr(connectionString);  // Set ConnectionStr

builder.Services.AddSingleton<CommentService>();
builder.Services.AddSingleton<DemandService>();
builder.Services.AddSingleton<DemandDetailService>();
builder.Services.AddSingleton<DemandStatusService>();
builder.Services.AddSingleton<ProductService>();
builder.Services.AddSingleton<ProductCategoryService>();
builder.Services.AddSingleton<ShoppingCartService>();
builder.Services.AddSingleton<ShoppingCartDetailService>();
builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<Enum_UserTypeService>();

builder.Services.AddControllersWithViews();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

builder.Services.AddAuthentication(Settings.AuthCookieName)
    .AddCookie(
    Settings.AuthCookieName,
    options =>
    {
        //options.Cookie.SameSite = SameSiteMode.None;
        //options.Cookie.SecurePolicy = CookieSecurePolicy.None;
        options.ExpireTimeSpan = TimeSpan.FromDays(14);
        options.LoginPath = Settings.LoginPath;
        options.AccessDeniedPath = Settings.AccessDeniedPath;
        options.Cookie.Name = Settings.AuthCookieName;
        //options.Cookie.HttpOnly = false;
    }
);
builder.Services.AddAuthorization(
    option =>
    {
        int AdminID = (int)UserTypeEnum.Admin;
        option.AddPolicy(
            UserTypeEnum.Admin.ConvertToString(),
            policy => policy.RequireRole(AdminID.ConvertToString())
        );
    }
);

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler(Settings.ErrorPath);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(name: "default", pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
