$(function () {

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

    $("a[data-ajax='true']").on("click", ajaxGetCourse);
});

$(function () {

    var toggleNines = function () {
        var $checkbox = $(this),
            $frontNineCheckbox = $("#front-nine-checkbox"),
            $backNineCheckbox = $("#back-nine-checkbox"),
            $target = $($checkbox.attr("data-target")),
            options = {
                url: $checkbox.attr("data-action"),
                type: "GET",
                data: $checkbox.serialize()
            };

        if ($checkbox.prop("checked")) {
            $.ajax(options).done(function (data) {
                var $newHtml = $(data);
                $target.html($newHtml);
            });
        } else {
            $target.empty();
        }

        if (!$frontNineCheckbox.prop("checked")) {
            $backNineCheckbox.attr("disabled", true);
        } else {
            $backNineCheckbox.attr("disabled", false);
        }

        if (!$backNineCheckbox.prop("checked")) {
            $frontNineCheckbox.attr("disabled", true);
        } else {
            $frontNineCheckbox.attr("disabled", false);
        }
    };

    $(document).on("change", "#front-nine-checkbox", toggleNines);
    $(document).on("change", "#back-nine-checkbox", toggleNines);
});