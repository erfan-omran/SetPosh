const productImage = document.querySelector('.product_image');
const mainImage = document.querySelector('.main_image');

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
let ratingValue = 1;

stars.forEach((star, index) => {
    star.addEventListener('mouseover', () => {
        UpdateStars(index + 1);
    });
    star.addEventListener('click', () => {
        ratingValue = index + 1;
        UpdateStars(ratingValue);
    });
    star.addEventListener('mouseout', () => {
        UpdateStars(ratingValue); 
    });
});

function UpdateStars(rating) {
    stars.forEach((star, index) => {
        if (index < rating) {
            star.classList.add('star_filled');
        } else {
            star.classList.remove('star_filled');
        }
    });
}