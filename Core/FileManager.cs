using Microsoft.AspNetCore.Http;

public static class FileManager
{
    private const string BaseUploadsPath = "wwwroot/Image/"; // مسیر پایه برای ذخیره فایل‌ها
    private const string ProductImagesFolder = "ProductImage"; // پوشه تصاویر محصولات

    /// <summary>
    /// ذخیره عکس محصول در سرور
    /// </summary>
    /// <param name="imageFile">فایل عکس</param>
    /// <param name="basePath">مسیر پایه (اختیاری - پیش‌فرض: مسیر جاری)</param>
    /// <returns>مسیر نسبی عکس ذخیره‌شده</returns>
    public static async Task<string> SaveProductImageAsync(IFormFile imageFile, string basePath = null)
    {
        if (imageFile == null || imageFile.Length == 0)
            return null;

        // بررسی پسوند فایل
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();
        if (Array.IndexOf(allowedExtensions, fileExtension) == -1)
            throw new InvalidOperationException("فرمت فایل تصویر نامعتبر است!");

        // بررسی حجم فایل (حداکثر 2 مگابایت)
        if (imageFile.Length > 2 * 1024 * 1024)
            throw new InvalidOperationException("حجم فایل باید کمتر از 2 مگابایت باشد!");

        // تعیین مسیر ذخیره‌سازی
        var uploadsPath = Path.Combine(basePath ?? Directory.GetCurrentDirectory(), BaseUploadsPath, ProductImagesFolder);
        if (!Directory.Exists(uploadsPath))
            Directory.CreateDirectory(uploadsPath);

        // تولید نام یکتا برای فایل
        var fileName = $"{Guid.NewGuid()}{fileExtension}";
        var filePath = Path.Combine(uploadsPath, fileName);

        // ذخیره فایل
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await imageFile.CopyToAsync(stream);
        }

        // برگرداندن مسیر نسبی (مثلاً: /uploads/products/filename.jpg)
        return $"{fileName}";
    }

    /// <summary>
    /// حذف عکس محصول از سرور
    /// </summary>
    /// <param name="imagePath">مسیر نسبی عکس (مثلاً: /uploads/products/filename.jpg)</param>
    /// <param name="basePath">مسیر پایه (اختیاری - پیش‌فرض: مسیر جاری)</param>
    /// <returns>نتیجه عملیات (true/false)</returns>
    public static bool DeleteProductImage(string imagePath, string basePath = null)
    {
        if (string.IsNullOrEmpty(imagePath))
            return false;

        try
        {
            // تبدیل مسیر نسبی به مسیر فیزیکی
            var relativePath = imagePath.TrimStart('/');
            var fullPath = Path.Combine(basePath ?? Directory.GetCurrentDirectory(), "wwwroot", relativePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                return true;
            }
            return false;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// بررسی وجود فایل عکس
    /// </summary>
    /// <param name="imagePath">مسیر نسبی عکس</param>
    /// <param name="basePath">مسیر پایه (اختیاری)</param>
    /// <returns>مسیر نسبی در صورت وجود فایل، در غیر این صورت null</returns>
    public static string GetProductImagePath(string imagePath, string basePath = null)
    {
        if (string.IsNullOrEmpty(imagePath))
            return null;

        var relativePath = imagePath.TrimStart('/');
        var fullPath = Path.Combine(basePath ?? Directory.GetCurrentDirectory(), "wwwroot", relativePath);

        return File.Exists(fullPath) ? imagePath : null;
    }
}