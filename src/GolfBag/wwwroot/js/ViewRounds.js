$(function () {
    "use strict";

    if (selectedRound >= 0) {
        var $course = $(this),
            options = {
                url: "/RoundOfGolf/DisplayRound/" + selectedRound,
                type: "GET",
                data: $course.serialize(),
                cache: false
            };

        $.ajax(options).done(function (data) {
            var $target = $("#display-round");
            var $newHtml = $(data);
            $target.html($newHtml);
        });
    }

});