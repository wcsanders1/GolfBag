$(function () {

    var ajaxGetCourse = function () {
        var $course = $(this);

        console.log($course.attr("data-action"));

        var options = {
            url: $course.attr("data-action"),
            type: "GET",
            data: $course.serialize()
        };

        $.ajax(options).done(function (data) {
            var $target = $($course.attr("data-target"));
            var $newHtml = $(data);
            console.log($target);
            $target.html($newHtml);
        });
    };

    $("a[data-ajax='true']").on("click", ajaxGetCourse);
});