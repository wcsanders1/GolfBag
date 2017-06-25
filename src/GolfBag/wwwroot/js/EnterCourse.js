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

        $(".course-slope-input").each(function (index) {
            if (index < $numberOfTeeBoxes) {
                $(this).removeClass("hidden");
            } else {
                $(this).addClass("hidden");
            }
        });
        validateOnTeeboxChange();
    };

    var validateOnTeeboxChange = function () {
        $(".custom-submit").off();
        validateForm($(".writable-scorecard"), true, true);
    };

    var backNine = function () {
        var $radioButton = $(this),
            $backNine = $("#back-nine"),
            $target = $($radioButton.attr("data-target")),
            options = {
                async: false,
                url: $radioButton.attr("data-action"),
                type: "GET",
                data: $radioButton.serialize(),
                cache: false
            };

        if (parseInt($radioButton.val()) === 18) {
            $.ajax(options).done(function (data) {
                var $newHtml = $(data);
                $target.html($newHtml);
                $(".custom-submit").off();
                validateForm($(".writable-scorecard"), true, false);
                makeTeeboxNamesCorrespond();
            });
        } else {
            $target.empty();
            $(".custom-submit").off();
            validateForm($(".writable-scorecard"), true, true);
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

    validateForm($(".writable-scorecard"), false, false);

    var validateThis = function () {
        validateCustom($(".writable-scorecard"));
    };

    $(document).on("click", ".custom-submit", validateThis);
});



/*********************************************************************************************
               MAKE TEEBOX NAME LABELS MATCH WHEN ENTERING NEW COURSE
*********************************************************************************************/

$(function () {
    "use strict";

    $(".teebox-name-input").on("keyup keydown click", function () {
        makeTeeboxNamesCorrespond();
    });
});