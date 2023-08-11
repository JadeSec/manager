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
        
        // Get the match of the menu item
        var match = navItem.getAttribute("data-url-match");

        // Check if the URL contains the value of the "href" attribute of the link
        if (new RegExp(match, 'gm').test(pathname)) {
            // Add the "active" class to the corresponding menu item
            navItem.classList.add('active');            
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

//This function is responsible per show/hidden tooltip

(function () {
    var tooltipTriggerList = [].slice.call(document.querySelectorAll('[data-bs-toggle="tooltip"]'));
    tooltipTriggerList.map(function (tooltipTriggerEl) {
        return new bootstrap.Tooltip(tooltipTriggerEl);
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


//This function is responsible per change colors badge findings
(function () {
    document.addEventListener("DOMContentLoaded", function () {
        const divs = document.querySelectorAll("[data-severity]");
        divs.forEach(item => {
            const severity = item.getAttribute("data-severity");
            const span = $(item).children("span");
            if (severity == "low") {
                span.addClass("bg-indigo");
            } else if (severity == "medium") {
                span.addClass("bg-yellow");
            } else if (severity == "high") {
                span.addClass("bg-warning");
            } else if (severity == "critical") {
                span.addClass("bg-red");
            }              
        });
    });
})();