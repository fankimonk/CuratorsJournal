function resizeTextAreas() {
    const textareas = document.querySelectorAll("textarea");

    textareas.forEach(textarea => {
        textarea.style.height = "auto";
        textarea.style.height = (textarea.scrollHeight) + "px";
    });
}