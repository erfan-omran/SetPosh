//slider
let currentSlide = 0;
const slides = document.querySelectorAll('.slide');
const dots = document.querySelectorAll('.dot');
const totalSlides = slides.length;
let slideInterval;
let touchStartX = 0; // موقعیت شروع لمسی
let touchEndX = 0; // موقعیت پایان لمسی
let isDragging = false; // وضعیت کشیدن ماوس یا انگشت

// نمایش اسلاید خاص
function showSlide(index) {
    if (index >= totalSlides) currentSlide = 0;
    if (index < 0) currentSlide = totalSlides - 1;

    slides.forEach((slide, i) => {
        slide.style.transform = `translateX(${currentSlide * 100}%)`;
    });

    dots.forEach((dot, i) => {
        dot.classList.toggle('active', i === currentSlide);
    });
}

// تغییر اسلاید با دکمه‌ها
function changeSlide(step) {
    currentSlide += step;
    showSlide(currentSlide);
    resetSlideInterval(); // ریست کردن تایمر
}

// رفتن به اسلاید خاص با دایره‌های ناوبری
function goToSlide(index) {
    currentSlide = index;
    showSlide(currentSlide);
    resetSlideInterval(); // ریست کردن تایمر
}

// تابع ریست کردن تایمر
function resetSlideInterval() {
    clearInterval(slideInterval); // متوقف کردن تایمر قبلی
    slideInterval = setInterval(() => {
        changeSlide(1); // رفتن به اسلاید بعدی
    }, 5000); // 5000 میلی‌ثانیه = 5 ثانیه
}

// مدیریت رویداد لمسی
function handleTouchStart(event) {
    touchStartX = event.changedTouches[0].clientX; // ذخیره موقعیت لمسی در شروع
    isDragging = true; // شروع کشیدن
}

function handleTouchEnd(event) {
    if (!isDragging) return; // اگر در حال کشیدن نیست، هیچ‌کاری نکنید
    touchEndX = event.changedTouches[0].clientX; // ذخیره موقعیت لمسی در پایان
    handleGesture(); // پردازش کشش انگشت
    isDragging = false; // پایان کشیدن
}

// مدیریت رویداد ماوس
function handleMouseDown(event) {
    touchStartX = event.clientX; // ذخیره موقعیت ماوس در شروع
    isDragging = true; // شروع کشیدن
}

function handleMouseUp(event) {
    if (!isDragging) return; // اگر در حال کشیدن نیست، هیچ‌کاری نکنید
    touchEndX = event.clientX; // ذخیره موقعیت ماوس در پایان
    handleGesture(); // پردازش کشش ماوس
    isDragging = false; // پایان کشیدن
}

function handleMouseMove(event) {
    if (!isDragging) return; // اگر در حال کشیدن نیست، هیچ‌کاری نکنید
    // می‌توان اینجا قابلیت کشیدن را با حرکات اضافی اضافه کرد (اختیاری)
}

// پردازش کشش انگشت یا ماوس
function handleGesture() {
    if (touchStartX - touchEndX > 50) {
        // کشش به سمت چپ
        changeSlide(-1);
    } else if (touchEndX - touchStartX > 50) {
        // کشش به سمت راست
        changeSlide(1);
    }
}

// افزودن رویدادهای لمسی
slides.forEach(slide => {
    slide.addEventListener('touchstart', handleTouchStart);
    slide.addEventListener('touchend', handleTouchEnd);
});

// افزودن رویدادهای ماوس
slides.forEach(slide => {
    slide.addEventListener('mousedown', handleMouseDown);
    slide.addEventListener('mouseup', handleMouseUp);
    slide.addEventListener('mousemove', handleMouseMove);
});

// نمایش اسلاید اولیه
showSlide(currentSlide);

// تنظیم تغییر خودکار هر ۵ ثانیه
resetSlideInterval(); // شروع تایمر

//