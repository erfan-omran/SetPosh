const productImage = document.querySelector('.product-image');
const mainImage = document.querySelector('.main-image');

// مدیریت زوم با حرکت موس
productImage.addEventListener('mousemove', (e) => {
    const rect = productImage.getBoundingClientRect();
    const x = e.clientX - rect.left;
    const y = e.clientY - rect.top;

    const xPercent = (x / rect.width) * 100;
    const yPercent = (y / rect.height) * 100;

    mainImage.style.transformOrigin = `${xPercent}% ${yPercent}%`;
    mainImage.style.transform = 'scale(1.8)';
});

// بازگرداندن زوم به حالت عادی
productImage.addEventListener('mouseleave', () => {
    mainImage.style.transform = 'scale(1)';
    mainImage.style.transformOrigin = 'center center';
});

// باز کردن عکس در حالت تمام صفحه
productImage.addEventListener('click', () => {
    if (mainImage.requestFullscreen) {
        mainImage.requestFullscreen(); // برای مرورگرهای مدرن
    } else if (mainImage.webkitRequestFullscreen) {
        mainImage.webkitRequestFullscreen(); // برای مرورگرهای WebKit
    } else if (mainImage.msRequestFullscreen) {
        mainImage.msRequestFullscreen(); // برای مرورگرهای قدیمی مایکروسافت
    }
});

//----------------------------------بخش کامنت----------------------------------
//پر کردن ستاره ها------

const stars = document.querySelectorAll('.star');
let ratingValue = 0; // ذخیره امتیاز انتخاب شده

// برای هر ستاره رویداد hover و کلیک اضافه می‌کنیم
stars.forEach((star, index) => {
    // رویداد hover (زمانی که موس روی ستاره میره)
    star.addEventListener('mouseover', () => {
        updateStars(index + 1);  // رنگ ستاره‌ها رو به روز می‌کنیم
    });

    // رویداد click (زمانی که کاربر روی ستاره کلیک می‌کنه)
    star.addEventListener('click', () => {
        ratingValue = index + 1;  // امتیاز ستاره‌ای که روی آن کلیک شده است
        updateStars(ratingValue);  // ستاره‌ها رو بر اساس امتیاز به روز می‌کنیم
    });

    // رویداد mouseout (زمانی که موس از ستاره خارج میشه)
    star.addEventListener('mouseout', () => {
        updateStars(ratingValue);  // زمانی که موس از ستاره خارج میشه، ستاره‌ها به وضعیت انتخاب شده برمی‌گردن
    });
});

// تابع برای تغییر رنگ ستاره‌ها به ازای امتیاز
function updateStars(rating) {
    stars.forEach((star, index) => {
        if (index < rating) {
            star.classList.add('filled');  // ستاره‌هایی که کمتر از امتیاز هستند پر میشن
        } else {
            star.classList.remove('filled');  // ستاره‌هایی که بیشتر از امتیاز هستند خالی می‌مونن
        }
    });
}


//ارسال نظر----------

const commentForm = document.querySelector('.comment-form');
const commentsList = document.querySelector('.comments-list');

commentForm.addEventListener('submit', function (e) {
    e.preventDefault();

    const commentText = e.target.comment.value;
    const rating = ratingValue;  // استفاده از مقدار امتیاز ذخیره‌شده

    // شبیه‌سازی نام کاربر
    const userName = "کاربر تست";

    const commentHTML = `
        <div class="comment-item">
            <div class="comment-author">
                ${userName}
                <div class="comment-rating">${'⭐'.repeat(rating)}</div>
            </div>
            <div class="comment-text">${commentText}</div>
        </div>
    `;

    // حذف متن "هنوز نظری ثبت نشده است" در صورت وجود
    if (commentsList.querySelector('p')) {
        commentsList.innerHTML = '';
    }

    commentsList.insertAdjacentHTML('beforeend', commentHTML);

    // پاک‌سازی فرم
    e.target.reset();

    // پاک کردن مقدار امتیاز پس از ارسال نظر
    ratingValue = 0;
    updateStars(ratingValue);  // ریست کردن رنگ ستاره‌ها پس از ارسال
});
