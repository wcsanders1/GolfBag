/**********************************************************************************
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
                message += ",";
            }          
        }
        return message;
    },
    isDuplicate: function (array, item) {
        "use strict";

        for (var i = 0; i < array.length; i++) {
            if (array[i] == item) {
                return true;
            }
        }
        return false;
    },
    showMessages: function (messages, $errorElement) {
        "use strict";

        var $message = "";  //"<div class='" + errorClass + "'>";

        for (var i = 0; i < messages.length; i++) {
            $message += "<p>" + messages[i] + "</p>";
        }

        $errorElement.empty();
        $errorElement.append($message);
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
    PUTT VALIDATOR
*****************************************/

var puttValidator = {
    makeAndShowErrorMessages: function () {
        var requiredArray = [],
            rangeArray = [],
            messageArray = [],
            requiredMessage = "",
            rangeMessage = "";

        $(".putt-errors").empty();

        $(".putt").each(function () {
            if ($(this).hasClass("invalid-putt")) {
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
                requiredMessage = "Enter a putt for the following hole:" + customValidations.turnArrayToMessage(requiredArray);
            } else {
                requiredMessage = "Enter a putt for the following holes:" + customValidations.turnArrayToMessage(requiredArray);
            }
            messageArray.push(requiredMessage);
        }

        if (rangeArray.length > 0) {
            if (rangeArray.length == 1) {
                rangeMessage = "The putt for the following hole must be between 0 and 9:" + customValidations.turnArrayToMessage(rangeArray);
            } else {
                rangeMessage = "Putts for the following holes must be between 0 and 9:" + customValidations.turnArrayToMessage(rangeArray);
            }
            messageArray.push(rangeMessage);
        }

        if (messageArray.length > 0) {
            customValidations.showMessages(messageArray, $(".putt-errors"));
        }
    },
    validatePutts: function ($form, makeMessagesNow) {
        "use strict";

        var makeRules = function ($form) {
            $(".putt").each(function () {
                var $element = $(this);

                $element.rules("add", {
                    required: true,
                    range: [0, 9],
                    messages: {
                        required: "required",
                        range: "range"
                    }
                });
                customValidations.bindValidationToElement($element, "invalid-putt", puttValidator.makeAndShowErrorMessages);
            });
            customValidations.bindValidationToSubmit($form, $(".putt"), "invalid-putt", puttValidator.makeAndShowErrorMessages);
        };

        makeRules($form);
        $(".error-container").append("<div class='putt-errors'></div>");

        if (makeMessagesNow) {
            puttValidator.makeAndShowErrorMessages();
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
                    requiredMessage = "Please enter the name of the course.";
                } else if (error == "maxlength") {
                    rangeMessage = "The course name cannot be more than 50 characters long.";
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
    HANDICAP VALIDATOR
*****************************************/

var handicapValidator = {
    makeAndShowErrorMessages: function () {
        var requiredArray = [],
            rangeArray = [],
            messageArray = [],
            requiredMessage = "",
            rangeMessage = "";

        $(".handicap-errors").empty();

        $(".handicap").each(function () {
            if ($(this).hasClass("invalid-handicap")) {
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
                requiredMessage = "Enter a handicap for the following hole:" + customValidations.turnArrayToMessage(requiredArray);
            } else {
                requiredMessage = "Enter a handicap for the following holes:" + customValidations.turnArrayToMessage(requiredArray);
            }
            messageArray.push(requiredMessage);
        }

        if (rangeArray.length > 0) {
            if (rangeArray.length == 1) {
                rangeMessage = "The handicap for the following hole must be between 1 and 18:" + customValidations.turnArrayToMessage(rangeArray);
            } else {
                rangeMessage = "Handicaps for the following holes must be between 1 and 18:" + customValidations.turnArrayToMessage(rangeArray);
            }
            messageArray.push(rangeMessage);
        }

        if (messageArray.length > 0) {
            customValidations.showMessages(messageArray, $(".handicap-errors"));
        }
    },
    validateHandicaps: function ($form, makeMessagesNow) {
        "use strict";

        var makeRules = function ($form) {
            $(".handicap").each(function () {
                var $element = $(this);

                $element.rules("add", {
                    required: true,
                    range: [1, 18],
                    messages: {
                        required: "required",
                        range: "range"
                    }
                });
                customValidations.bindValidationToElement($element, "invalid-handicap", handicapValidator.makeAndShowErrorMessages);
            });
            customValidations.bindValidationToSubmit($form, $(".handicap"), "invalid-handicap", handicapValidator.makeAndShowErrorMessages);
        };

        makeRules($form);
        $(".error-container").append("<div class='handicap-errors'></div>");

        if (makeMessagesNow) {
            handicapValidator.makeAndShowErrorMessages();
        }
    }
};



/*****************************************
    TEEBOX NAME VALIDATOR
*****************************************/

var teeboxNameValidator = {
    makeAndShowErrorMessages: function () {
        var messageArray = [],
            requiredMessage = "",
            rangeMessage = "";

        $(".teebox-name-errors").empty();

        $(".teebox-name").each(function () {
            if ($(this).hasClass("invalid-teebox-name")) {
                var error = $(this).siblings(".field-validation-error").find("span").text();
                if (error == "required") {
                    requiredMessage = "All teeboxes must have a name.";
                } else if (error == "maxlength") {
                    rangeMessage = "Teebox names cannot be more than 20 characters long.";
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
            customValidations.showMessages(messageArray, $(".teebox-name-errors"));
        }
    },
    validateTeeboxNames: function ($form, makeMessagesNow) {
        "use strict";

        var makeRules = function ($form) {
            $(".teebox-name").each(function () {
                var $element = $(this);

                if (!($element.parents("tr").hasClass("hidden"))) {
                    $element.rules("add", {
                        required: true,
                        maxlength: 20,
                        messages: {
                            required: "required",
                            maxlength: "maxlength"
                        } 
                    });
                    customValidations.bindValidationToElement($element, "invalid-teebox-name", teeboxNameValidator.makeAndShowErrorMessages);
                    customValidations.bindValidationToSubmit($form, $(".teebox-name"), "invalid-teebox-name", teeboxNameValidator.makeAndShowErrorMessages);
                } else {
                    $element.off();
                    $element.removeClass("invalid-teebox-name");
                }               
            });
            
        };

        makeRules($form);
        $(".error-container").append("<div class='teebox-name-errors'></div>");

        if (makeMessagesNow) {
            teeboxNameValidator.makeAndShowErrorMessages();
        }
    }
};



/*****************************************
    TEEBOX VALIDATOR
*****************************************/

var teeboxValidator = {
    makeAndShowErrorMessages: function () {
        var requiredArray = [],
            rangeArray = [],
            messageArray = [],
            requiredMessage = "",
            rangeMessage = "";

        $(".teebox-errors").empty();

        $(".teebox").each(function () {
            if ($(this).hasClass("invalid-teebox")) {
                var error = $(this).siblings(".field-validation-error").find("span").text(),
                    holeNumber = $(this).data("hole-number");
                if (error == "required" && (!customValidations.isDuplicate(requiredArray, holeNumber))) {
                    requiredArray.push(holeNumber);
                } else if (error == "range" && (!customValidations.isDuplicate(rangeArray, holeNumber))) {
                    rangeArray.push(holeNumber);
                }             
            }
        });

        requiredArray.sort(function (a, b) {
            return a - b;
        });

        rangeArray.sort(function (a, b) {
            return a - b;
        });

        if (requiredArray.length > 0) {
            if (requiredArray.length == 1) {
                requiredMessage = "Enter a yardage for the following hole:" + customValidations.turnArrayToMessage(requiredArray);
            } else {
                requiredMessage = "Enter a yardage for the following holes:" + customValidations.turnArrayToMessage(requiredArray);
            }
            messageArray.push(requiredMessage);
        }

        if (rangeArray.length > 0) {
            if (rangeArray.length == 1) {
                rangeMessage = "The yardage for the following hole must be between 1 and 1000:" + customValidations.turnArrayToMessage(rangeArray);
            } else {
                rangeMessage = "Yardages for the following holes must be between 1 and 1000:" + customValidations.turnArrayToMessage(rangeArray);
            }
            messageArray.push(rangeMessage);
        }

        if (messageArray.length > 0) {
            customValidations.showMessages(messageArray, $(".teebox-errors"));
        }
    },
    validateTeeboxes: function ($form, makeMessagesNow) {
        "use strict";

        var makeRules = function ($form) {
            $(".teebox").each(function () {
                var $element = $(this);

                if (!($element.parents("tr").hasClass("hidden"))) {
                    $element.rules("add", {
                        required: true,
                        range: [1, 1000],
                        messages: {
                            required: "required",
                            range: "range"
                        }
                    });
                    customValidations.bindValidationToElement($element, "invalid-teebox", teeboxValidator.makeAndShowErrorMessages);
                    customValidations.bindValidationToSubmit($form, $element, "invalid-teebox", teeboxValidator.makeAndShowErrorMessages);
                } else {
                    $element.off();
                    $element.removeClass("invalid-teebox");
                }
            });
        };

        makeRules($form);
        $(".error-container").append("<div class='teebox-errors'></div>");

        if (makeMessagesNow) {
            teeboxValidator.makeAndShowErrorMessages();
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

    if ($form.find(".handicap").length != 0) {
        handicapValidator.validateHandicaps($form, makeMessagesNow);
    }

    if ($form.find(".teebox-name").length != 0) {
        teeboxNameValidator.validateTeeboxNames($form, makeMessagesNow);
    }

    if ($form.find(".teebox").length != 0) {
        teeboxValidator.validateTeeboxes($form, makeMessagesNow);
    }

    if ($form.find(".putt").length != 0) {
        puttValidator.validatePutts($form, makeMessagesNow);
    }
};

var validateCustom = function ($form) {
    if ($(".error-container").has("p").length != 0) {
        return false;
    } else {
        $form.submit();
    }
};