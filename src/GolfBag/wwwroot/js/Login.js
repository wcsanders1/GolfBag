/***********************************************************************************
                        FLOATING LABELS
***********************************************************************************/

$(function () {
    "use strict";

    $(".field-input").on("input", function () {
        configureFloatingInputs($(this));
    });

    $(".field-input").each(function () {
        configureFloatingInputs($(this));
    });
});