/**********************************************************************************
            GLOBAL VALIDATION FUNCTIONS
**********************************************************************************/

var validateForm = function ($form, makeMessagesNow) {
    "use strict";

    var $errorContainer = $(".error-container");

    var showMessages = function (messages) {
        var $message = "";
        
        $errorContainer.empty();

        for (var i = 0; i < messages.length; i++) {
            $message += "<li>" + messages[i] + "</li>"
        }
        $errorContainer.append($message);
    };

    var makeAndShowErrorMessages = function () {
        var requiredArray = [],
            rangeArray = [],
            messageArray = [],
            requiredMessage = "",
            rangeMessage = "";

        var turnArrayToMessage = function (array) {
            var message = "";

            for (var i = 0; i < array.length; i++) {
                message += " " + array[i];
                if (i < array.length - 1) {
                    message += ","
                }
            }
            return message;
        };

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
                requiredMessage = "Enter a score for the following hole:" + turnArrayToMessage(requiredArray);
            } else {
                requiredMessage = "Enter a score for the following holes:" + turnArrayToMessage(requiredArray);
            }
            messageArray.push(requiredMessage);
        }

        if (rangeArray.length > 0) {
            if (rangeArray.length == 1) {
                rangeMessage = "Scores for the following hole must be between 1 and 99:" + turnArrayToMessage(rangeArray);
            } else {
                rangeMessage = "Scores for the following holes must be between 1 and 99:" + turnArrayToMessage(rangeArray);
            } 
            messageArray.push(rangeMessage);
        }
        
        if (messageArray.length > 0) {
            showMessages(messageArray);
        }
    };

    var validateElement = function ($element) {
        if (!($element.valid())) {
            $element.addClass("invalid-score");
        } else {
            $element.removeClass("invalid-score");
        }
    };

    

    $form.validate();

    $errorContainer.empty();

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
        $element.on("focusout keyup", function (e) {     
            var code = e.keyCode || e.which;
            if (code != 9) {                        //RETURNS FALSE IF TAB KEY
                validateElement($element);
                makeAndShowErrorMessages();
            }
        });
    });

    $form.on("invalid-form.validate", function () {
        $(".score").each(function () {
            validateElement($(this));
            makeAndShowErrorMessages();
        });
    });

    if (makeMessagesNow) {
        makeAndShowErrorMessages();
    }
};