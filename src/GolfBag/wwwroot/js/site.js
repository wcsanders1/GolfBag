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
                        SWITCHES ON JQUERY UI DATEPICKER

documentation for the datepicker: http://www.eyecon.ro/bootstrap-datepicker/
***********************************************************************************/

$(function () {
    "use strict";

    $(document).on("focus", ".datepicker", function () {
        $(this).datepicker();
        $(this).on("changeDate", function () {
            $(this).datepicker("hide");
        });
    });
});

// $("#carousel").carousel();  this will activate a carousel