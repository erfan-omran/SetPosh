//using Microsoft.AspNetCore.Http;
//using System.Globalization;

//namespace Core
//{
//    public class PersianCultureMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public PersianCultureMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task InvokeAsync(HttpContext context)
//        {
//            CultureInfo persianCulture = new CultureInfo("fa-IR");
//            persianCulture.DateTimeFormat.ShortDatePattern = "yyyy/MM/dd";
//            persianCulture.DateTimeFormat.LongDatePattern = "dddd d MMMM yyyy";
//            persianCulture.DateTimeFormat.AMDesignator = "ق.ظ";
//            persianCulture.DateTimeFormat.PMDesignator = "ب.ظ";

//            Thread.CurrentThread.CurrentCulture = persianCulture;
//            Thread.CurrentThread.CurrentUICulture = persianCulture;

//            await _next(context);
//        }
//    }
//}