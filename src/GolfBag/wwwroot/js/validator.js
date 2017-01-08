/**********************************************************************************
            GLOBAL VALIDATION FUNCTIONS
**********************************************************************************/

var validateForm = function ($form) {
    "use strict";

    $form.validate();
    $(".error-container").empty();

    $(".score").each(function () {
        var $holeNumber = $(this).attr("data-hole-number");
        $(this).rules("add", {
            required: true,
            range: [1, 99],
            messages: {
                required: "Hole " + $holeNumber + ": You must enter a score",
                range: "Hole " + $holeNumber + ": Score must be between 1 and 99"
            }
        })
    });

    $form.data("validator").settings.onfocusout = function (element) { $(element).valid(); };
    $form.data("validator").settings.showErrors = function (errorMap, errorList) {
        this.defaultShowErrors();
        $(".error-container").empty();
        $(".field-validation-error span", $form)
            .clone()
            .appendTo(".error-container")
            .wrap("<li>");

    };
};