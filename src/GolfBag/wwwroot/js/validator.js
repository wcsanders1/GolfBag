/**********************************************************************************
            GLOBAL VALIDATION OBJECTS AND FUNCTIONS
**********************************************************************************/


/*****************************************
    CUSTOM VALIDATION FUNCTIONS
*****************************************/

var customValidations = {
    validateElement: function ($element, validationClassnName) {
        "use strict";

        if (!($element.valid())) {
            $element.addClass(validationClassName);
        } else {
            $element.removeClass(validationClassName);
        }
    },
    bindValidationToElement: function ($element, validationClassName, messageFunction) {
        "use strict";

        $element.on("focusout keyup", function (e) {
            var code = e.keyCode || e.which;
            if (code != 9) {                        //RETURNS FALSE IF TAB KEY
                customValidations.validateElement($element, validationClassName);
                messageFunction();
            }
        });
    },
    bindValidationToSubmit: function ($form, $element, validationClassName, messageFunction) {
        "use strict";

        $form.on("invalid-form.validate", function () {
            $element.each(function () {
                customValidations.validateElement($(this), validationClassName);
                messageFunction();
            });
        });
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


/*****************************************
    SCORE VALIDATOR
*****************************************/

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

        var makeRules = function ($form) {
            $(".score").each(function () {
                var $element = $(this);

                $element.rules("add", {
                    required: true,
                    range: [1, 99],
                    messages: {
                        required: "required",
                        range: "range"
                    }
                });
                customValidations.bindValidationToElement($element, "invalid-score", scoreValidator.makeAndShowErrorMessages);
            });
            customValidations.bindValidationToSubmit($form, $(".score"), "invalid-score", scoreValidator.makeAndShowErrorMessages);
        };
        makeRules($form);
        if (makeMessagesNow) {
            scoreValidator.makeAndShowErrorMessages();
        }
    }
};



/*****************************************
    COURSE NAME VALIDATOR
*****************************************/

var courseNameValidator = {
    makeAndShowErrorMessages: function () {
        var messageArray = [],
            requiredMessage = "",
            rangeMessage = "",
            $errorContainer = $(".error-container");

        $errorContainer.empty();

        $(".course-name").each(function () {
            if ($(this).hasClass("invalid-course-name")) {
                var error = $(this).siblings(".field-validation-error").find("span").text();
                if (error == "required") {
                    requiredMessage = "Please enter the name of the course."
                } else if (error == "maxlength") {
                    rangeMessage = "The course name cannot be more than 50 characters long."
                }
            }
        });

        if (requiredMessage != "") {
            messageArray.push(requiredMessage);
        }

        if (rangeMessage != "") {
            messageArray.push(rangeMessage);
        }

        if (messageArray.length > 0) {
            customValidations.showMessages(messageArray);
        }
    },
    validateCourseNames: function ($form, makeMessagesNow) {
        "use strict";

        var makeRules = function ($form) {
            $(".course-name").each(function () {
                var $element = $(this);
                $element.rules("add", {
                    required: true,
                    maxlength: 50,
                    messages: {
                        required: "required",
                        maxlength: "maxlength"
                    }
                });
                customValidations.bindValidationToElement($element, "invalid-course-name", courseNameValidator.makeAndShowErrorMessages);
            });
            customValidations.bindValidationToSubmit($form, $(".course-name"), "invalid-course-name", courseNameValidator.makeAndShowErrorMessages);
        };

        makeRules($form);
        if (makeMessagesNow) {
            courseNameValidator.makeAndShowErrorMessages();
        }
    }
};



/*****************************************
    VALIDATOR
*****************************************/

var validateForm = function ($form, makeMessagesNow) {
    "use strict";

    var $errorContainer = $(".error-container");

    $form.removeData("validator").removeData("unobtrusiveValidation");
    $.validator.unobtrusive.parse($form);
    $errorContainer.empty();

    if ($form.find(".score")) {
        scoreValidator.validateScores($form, makeMessagesNow);
    }

    if ($form.find(".course-name")) {
        console.log("validating coursename");
        courseNameValidator.validateCourseNames($form, makeMessagesNow);
    }
};