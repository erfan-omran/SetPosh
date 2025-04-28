
function IsNullOrEmpty(value) {
    return value === null || value === undefined || value.trim() === '';
}

function ShowMessage(MessageTagElm, MessageText, Success = true) {
    MessageTagElm.classList.remove("hidden")
    MessageTagElm.innerText = MessageText;
    if (Success) {
        MessageTagElm.classList.remove("alert_danger")
        MessageTagElm.classList.add("alert_success")
    }
    else {
        MessageTagElm.classList.remove("alert_success")
        MessageTagElm.classList.add("alert_danger")
    }
}
function HideMessage(MessageTagElm) {
    MessageTagElm.classList.add("hidden")
}

function ShowLoader(LoadingContainer, LoadingMessage = "") {

    let ResTxt = `
                <div class="loading_state">
                    <div class="dots_loader">
                        <span>.</span>
                        <span>.</span>
                        <span>.</span>
                    </div>`

    if (true)
        ResTxt += `<p>${LoadingMessage}</p>`;
    ResTxt += "</div>"

    $("#" + LoadingContainer).html(ResTxt);
}

function ShowFloatingMessage(message, type) {
    const NotificationContainer = document.getElementById('NotificationsContainer');
    const Notification = document.createElement('div');
    Notification.className = `notification ${type}`;

    // 1. ابتدا المنت را با opacity:0 به DOM اضافه می‌کنیم
    Notification.style.opacity = '0';
    Notification.style.transform = 'translateX(-100%)';

    Notification.innerHTML = `
        <div class="notification_content">${message}</div>
        <div class="notification_progress">
            <div class="notification_progressLine"></div>
        </div>
    `;

    // 2. اضافه کردن به DOM قبل از هر انیمیشنی
    if (NotificationContainer.firstChild) {
        NotificationContainer.insertBefore(Notification, NotificationContainer.firstChild);
    } else {
        NotificationContainer.appendChild(Notification);
    }


    const duration = type === 'error' ? 5000 : 3000;

    // 3. استفاده از double requestAnimationFrame برای اطمینان از رندر
    requestAnimationFrame(() => {
        requestAnimationFrame(() => {
            // 4. فعال کردن transition پس از رندر
            Notification.style.transition = 'all 0.3s ease';
            Notification.style.opacity = '1';
            Notification.style.transform = 'translateX(0)';

            // 5. شروع انیمیشن progress bar با تأخیر
            setTimeout(() => {
                const progressBarLine = Notification.querySelector('.notification_progressLine');
                progressBarLine.style.transition = `transform ${duration}ms linear`;
                progressBarLine.style.transform = 'scaleX(0)';
            }, 50);
        });
    });

    // 6. حذف پیام
    setTimeout(() => {
        Notification.style.transition = 'all 0.3s ease';
        Notification.style.opacity = '0';
        Notification.style.transform = 'translateX(-100%)';

        setTimeout(() => {
            Notification.remove();
        }, 300);
    }, duration);
}