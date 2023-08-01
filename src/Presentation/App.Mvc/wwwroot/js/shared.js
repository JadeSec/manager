//This function is responsible for remove "layout-fluid" in specify pages
(function () {
    if ($('div.page-center').length > 0) {
        $('body').removeClass('layout-fluid')
                 .addClass('page-center-muted');
    }
})();

// This function is responsible for adding/removing effect in the menu
(function () {
    // Obtém a URL atual
    var pathname = window.location.pathname;

    // Get all menu items
    var navItems = document.querySelectorAll('.nav-item');

    // Iterate through each menu item
    for (var i = 0; i < navItems.length; i++) {
        var navItem = navItems[i];

        // Get the link of the menu item
        var link = navItem.querySelector('a');

        // Get the value of the "href" attribute of the link
        var href = link.getAttribute('href');

        // Check if the URL contains the value of the "href" attribute of the link
        if (new RegExp(`(${href}).*`, 'gm').test(pathname)) {
            // Add the "active" class to the corresponding menu item
            navItem.classList.add('active');
            //navItem.classList.remove('text-muted');
            $(link).toggleClass("text-muted");
        }
    }
})();

//This function is responsible per show/hidden popover
(function () {
    var popoverTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="popover"]'))
    popoverTriggerList.map(function (popoverTriggerEl) {
        return new bootstrap.Popover(popoverTriggerEl)
    });
})();

//This function is responsible per enable input mask
(function () {
    document.addEventListener("DOMContentLoaded", function () {
        const inputs = document.querySelectorAll("[data-mask]");
        inputs.forEach(input => {
            const mask = input.getAttribute("data-mask");
            const maskVisible = input.getAttribute("data-mask-visible") === "true";

            const imask = IMask(input, {
                mask: mask,
                lazy: maskVisible
            });
        });
    });
})();