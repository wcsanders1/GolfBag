$(function () {
    "use strict";

    var backNine = function () {
        var $radioButton = $(this),
            $backNine = $("#back-nine"),
            $target = $($radioButton.attr("data-target")),
            options = {
                url: $radioButton.attr("data-action"),
                type: "GET",
                data: $radioButton.serialize()
            };

        if ($radioButton.val() == 18) {
            $.ajax(options).done(function (data) {
                var $newHtml = $(data);
                $target.html($newHtml);
            });
        } else {
            $target.empty();
        }
    };

    $("#input-nine").on("change", backNine);
    $("#input-eighteen").on("change", backNine);
});