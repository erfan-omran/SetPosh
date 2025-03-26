namespace SetPosh
{
    public class Settings
    {
        public const string AuthCookieName = "SetPoshCookie";
        public static DateTime CookieExpierTime = DateTime.UtcNow.AddDays(12);

        public const string LoginPath = "/Auth/Login";
        public const string AccessDeniedPath = "/Auth/Error";
        public const string ErrorPath = "/Home/Error";
    }
}
