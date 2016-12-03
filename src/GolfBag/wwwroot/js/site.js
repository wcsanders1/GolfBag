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

$(function () {
    "use strict";

    $("body").tooltip({
        selector: "[data-toggle='tooltip']"
    });
});