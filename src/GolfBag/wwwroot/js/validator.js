﻿/**********************************************************************************
            GLOBAL VALIDATION OBJECTS AND FUNCTIONS
**********************************************************************************/


/*****************************************
    CUSTOM VALIDATION FUNCTIONS
*****************************************/

var customValidations = {
    validateElement: function ($element, validationClassName) {
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

        //$form.on("invalid-form.validate", function () {
        $(".custom-submit").on("click", function () {
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
    showMessages: function (messages, $errorElement) {
        "use strict";

        var $message = "";  //"<div class='" + errorClass + "'>";

        for (var i = 0; i < messages.length; i++) {
            $message += "<p>" + messages[i] + "</p>"
        }

        //$message += "</div>";

        $errorElement.empty();
        $errorElement.append($message);
        //$(".error-container").append($message);
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
            rangeMessage = "";

        $(".score-errors").empty();

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
            customValidations.showMessages(messageArray, $(".score-errors"));
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
        $(".error-container").append("<div class='score-errors'></div>");

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
            rangeMessage = "";

        $(".course-name-errors").empty();

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
            customValidations.showMessages(messageArray, $(".course-name-errors"));
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
        $(".error-container").append("<div class='course-name-errors'></div>");

        if (makeMessagesNow) {
            courseNameValidator.makeAndShowErrorMessages();
        }
    }
};



/*****************************************
    PAR VALIDATOR
*****************************************/

var parValidator = {
    makeAndShowErrorMessages: function () {
        var requiredArray = [],
            rangeArray = [],
            messageArray = [],
            requiredMessage = "",
            rangeMessage = "";

        $(".par-errors").empty();

        $(".par").each(function () {
            if ($(this).hasClass("invalid-par")) {
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
                requiredMessage = "Enter a par for the following hole:" + customValidations.turnArrayToMessage(requiredArray);
            } else {
                requiredMessage = "Enter a par for the following holes:" + customValidations.turnArrayToMessage(requiredArray);
            }
            messageArray.push(requiredMessage);
        }

        if (rangeArray.length > 0) {
            if (rangeArray.length == 1) {
                rangeMessage = "The par for the following hole must be between 1 and 9:" + customValidations.turnArrayToMessage(rangeArray);
            } else {
                rangeMessage = "Pars for the following holes must be between 1 and 9:" + customValidations.turnArrayToMessage(rangeArray);
            }
            messageArray.push(rangeMessage);
        }

        if (messageArray.length > 0) {
            customValidations.showMessages(messageArray, $(".par-errors"));
        }
    },
    validatePars: function ($form, makeMessagesNow) {
        "use strict";

        var makeRules = function ($form) {
            $(".par").each(function () {
                var $element = $(this);

                $element.rules("add", {
                    required: true,
                    range: [1, 9],
                    messages: {
                        required: "required",
                        range: "range"
                    }
                });
                customValidations.bindValidationToElement($element, "invalid-par", parValidator.makeAndShowErrorMessages);
            });
            customValidations.bindValidationToSubmit($form, $(".par"), "invalid-par", parValidator.makeAndShowErrorMessages);
        };

        makeRules($form);
        $(".error-container").append("<div class='par-errors'></div>");

        if (makeMessagesNow) {
            parValidator.makeAndShowErrorMessages();
        }
    }
};



/*****************************************
    VALIDATOR
*****************************************/

var validateForm = function ($form, makeMessagesNow, turnOff) {
    "use strict";

    var $errorContainer = $(".error-container");

    $form.removeData("validator").removeData("unobtrusiveValidation");
    $form.removeData();

    if (turnOff) {
        $form.off();
    }

    $.validator.unobtrusive.parse($form);
    $errorContainer.empty();

    if ($form.find(".score").length != 0) {
        scoreValidator.validateScores($form, makeMessagesNow);
    }

    if ($form.find(".course-name").length != 0) {
        courseNameValidator.validateCourseNames($form, makeMessagesNow);
    }

    if ($form.find(".par").length != 0) {
        parValidator.validatePars($form, makeMessagesNow);
    }
};

var validateCustom = function ($form) {
    if ($(".error-container").has("p").length != 0) {
        console.log("erroring");

        return false;
    } else {
        console.log("submitting");

        //$form.submit();
    }
};