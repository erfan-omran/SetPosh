
function IsNullOrEmpty(value) {
    return !value || value.trim() === '';
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