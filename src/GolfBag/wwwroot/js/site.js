/***********************************************************************************
                        AJAX
 **********************************************************************************/

$(function () {
    "use strict";

    var ajaxGetCourse = function () {
        var $course = $(this),
            options = {
                url: $course.attr("data-action"),
                type: "GET",
                data: $course.serialize()
            };

        $.ajax(options).done(function (data) {
            var $target = $($course.attr("data-target"));
            var $newHtml = $(data);
            $target.html($newHtml);
        });
    };

    $(document).on("click", "a[data-ajax='true']", ajaxGetCourse);
});


/***********************************************************************************
                        SWITCHES ON TOOLTIPS
 **********************************************************************************/

$(function () {
    "use strict";

    $("body").tooltip({
        selector: "[data-toggle='tooltip']"
    });
});


/***********************************************************************************
                        TOGGLES FLOAT ON NAVBAR
 **********************************************************************************/

$(function () {
    "use strict";

    var navbarCollapse = function () {
        if ($("#navbar-toggler").is(":visible")) {
            $("#title-and-welcome").css("float", "none");
        } else {
            $("#title-and-welcome").css("float", "left");
        }
    };

    $(document).ready(navbarCollapse);
    $(window).on('resize', navbarCollapse);
});


// $("#carousel").carousel();  this will activate a carousel