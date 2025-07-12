// site.js

document.addEventListener("DOMContentLoaded", function () {
    console.log("site.js carregado com sucesso");

    // Ativa todos os dropdowns do Bootstrap
    var dropdownTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="dropdown"]'));
    dropdownTriggerList.map(function (dropdownToggleEl) {
        return new bootstrap.Dropdown(dropdownToggleEl);
    });
});
