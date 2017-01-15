/**********************************************************************************
            GLOBAL VALIDATION OBJECTS AND FUNCTIONS
**********************************************************************************/

var customValidations = {
    validateElement: function ($element, validationClass) {
        "use strict";

        if (!($element.valid())) {
            $element.addClass(validationClass);
        } else {
            $element.removeClass(validationClass);
        }
    },
    turnArrayToMessage: function (array) {
        "use strict";

        var message = "";

        for (var i = 0; i < array.length; i++) {
            message += " " + array[i];
            if (i < array.length - 1) {
                message += ","
            }
        }
        return message;
    },
    showMessages: function (messages) {
        "use strict";

        var $message = "";

        for (var i = 0; i < messages.length; i++) {
            $message += "<li>" + messages[i] + "</li>"
        }

        $(".error-container").empty().append($message);
    }
};

var scoreValidator = {
    makeAndShowErrorMessages: function () {
        var requiredArray = [],
            rangeArray = [],
            messageArray = [],
            requiredMessage = "",
            rangeMessage = "",
            $errorContainer = $(".error-container");

        $errorContainer.empty();

        $(".score").each(function () {
            if ($(this).hasClass("invalid-score")) {
                var error = $(this).siblings(".field-validation-error").find("span").text(),
                    holeNumber = $(this).data("hole-number");
                if (error == "required") {
                    requiredArray.push(holeNumber);
                } else if (error == "range") {
                    rangeArray.push(holeNumber);
                }               
            }
        });

        if (requiredArray.length > 0) {
            if (requiredArray.length == 1) {
                requiredMessage = "Enter a score for the following hole:" + customValidations.turnArrayToMessage(requiredArray);
            } else {
                requiredMessage = "Enter a score for the following holes:" + customValidations.turnArrayToMessage(requiredArray);
            }
            messageArray.push(requiredMessage);
        }

        if (rangeArray.length > 0) {
            if (rangeArray.length == 1) {
                rangeMessage = "The score for the following hole must be between 1 and 99:" + customValidations.turnArrayToMessage(rangeArray);
            } else {
                rangeMessage = "Scores for the following holes must be between 1 and 99:" + customValidations.turnArrayToMessage(rangeArray);
            } 
            messageArray.push(rangeMessage);
        }
        
        if (messageArray.length > 0) {
            customValidations.showMessages(messageArray);
        }
    },
    validateScores: function ($form, makeMessagesNow) {
        "use strict";
         
        var bindValidationToElement = function ($element) {
            $element.on("focusout keyup", function (e) {
                var code = e.keyCode || e.which;
                if (code != 9) {                        //RETURNS FALSE IF TAB KEY
                    customValidations.validateElement($element, "invalid-score");
                    scoreValidator.makeAndShowErrorMessages();
                }
            });
        };

        var bindValidationToSubmit = function ($form) {
            $form.on("invalid-form.validate", function () {
                $(".score").each(function () {
                    customValidations.validateElement($(this), "invalid-score");
                    scoreValidator.makeAndShowErrorMessages();
                    
                });
            });
        };

        var makeRules = function ($form) {
            $(".score").each(function () {
                var $holeNumber = $(this).attr("data-hole-number"),
                    $element = $(this);
                $element.rules("add", {
                    required: true,
                    range: [1, 99],
                    messages: {
                        required: "required",
                        range: "range"
                    }
                });
                bindValidationToElement($element);
            });
            bindValidationToSubmit($form);
        };
        makeRules($form);
        if (makeMessagesNow) {
            scoreValidator.makeAndShowErrorMessages();
        }
    }
};

var validateForm = function ($form, makeMessagesNow) {
    "use strict";

    var $errorContainer = $(".error-container");

    $form.removeData("validator").removeData("unobtrusiveValidation");

    $.validator.unobtrusive.parse($form);

    $errorContainer.empty();

    if ($form.find(".score")) {
        scoreValidator.validateScores($form, makeMessagesNow);
    }
};