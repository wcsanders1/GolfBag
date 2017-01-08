/******************************************************************************
            TOGGLES FRONT AND BACK NINES
*******************************************************************************/

$(function () {
    "use strict";

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

        var _validate = function () {
            $.validator.unobtrusive.parse(".writable-scorecard");
            validateForm($(".writable-scorecard"));
        };

        if ($checkbox.prop("checked")) {
            $.ajax(options).done(function (data) {
                var $newHtml = $(data);
                $target.html($newHtml);
                _validate();
                stripeRow($(".teebox-selector"));
            });
        } else {
            $target.empty();
        }

        if (!$frontNineCheckbox.prop("checked")) {
            $backNineCheckbox.attr("disabled", true);
        } else {
            $backNineCheckbox.attr("disabled", false);
            _validate();
        }

        if (!$backNineCheckbox.prop("checked")) {
            $frontNineCheckbox.attr("disabled", true);
        } else {
            $frontNineCheckbox.attr("disabled", false);
            _validate();
        }
    };

    $(document).on("change", "#front-nine-checkbox", toggleNines);
    $(document).on("change", "#back-nine-checkbox", toggleNines);
});



/******************************************************************************
            TEEBOX TOGGLING
*******************************************************************************/

$(function () {
    "use strict";

    var stripe = function () {
        stripeRow($(this));
    }

    $(document).on("change", ".teebox-selector", stripe);
});