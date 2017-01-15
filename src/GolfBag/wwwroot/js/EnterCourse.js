$(function () {
    "use strict";

    var configureTeeBoxes = function () {
        var $numberOfTeeBoxes = $("#tee-box-number-selector").val();

        $(".tee-box-input").each(function (index) {
            if (index < $numberOfTeeBoxes) {
                $(this).removeClass("hidden");
            } else {
                $(this).addClass("hidden");
            }
        });

        $(".back-nine-tee-box-input").each(function (index) {
            if (index < $numberOfTeeBoxes) {
                $(this).removeClass("hidden");
            } else {
                $(this).addClass("hidden");
            }
        });
    };

    var backNine = function () {
        var $radioButton = $(this),
            $backNine = $("#back-nine"),
            $target = $($radioButton.attr("data-target")),
            options = {
                async: false,
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

        configureTeeBoxes();
    };

    $("#input-nine").on("change", backNine);
    $("#input-eighteen").on("change", backNine);
    $("#tee-box-number-selector").on("change", configureTeeBoxes);
});



/*********************************************************************************************
               JQUERY VALIDATOR ON FORM
*********************************************************************************************/

$(function () {
    "use strict";

    validateForm($(".writable-scorecard"), false);
});