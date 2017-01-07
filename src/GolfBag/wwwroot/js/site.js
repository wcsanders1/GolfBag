/***********************************************************************************
                        AJAX AND JQUERY FORM VALIDATION
 **********************************************************************************/

$(function () {
    "use strict";

    var validateForm = function ($form) {

        $form.validate();

        $(".score").each(function () {
            $(this).rules("add", {
                required: true,
                range: [1, 99],
                messages: {
                    required: "You must enter a score",
                    range: "Be a good man"
                }
            })
        });

        $form.data("validator").settings.onfocusout = function (element) { $(element).valid(); };
        $form.data("validator").settings.showErrors = function (errorMap, errorList) {
            this.defaultShowErrors();
            $("#error-container").empty();
            $(".field-validation-error span", $form)
                .clone()
                .appendTo("#error-container")
                .wrap("<li>");

        };
    };

    var ajaxGetCourse = function () {
        var $course = $(this),
            options = {
                url: $course.attr("data-action"),
                type: "GET",
                data: $course.serialize()
            };

        $.ajax(options).done(function (data) {
            var $target = $($course.attr("data-target"));
            var $newHtml = $(data);
            $target.html($newHtml);
            $.validator.unobtrusive.parse("#nine-holes");
            validateForm($("#nine-holes"));
        });
    };

    $(document).on("click", "a[data-ajax='true']", ajaxGetCourse);
});


/***********************************************************************************
                        SWITCHES ON TOOLTIPS
 **********************************************************************************/

$(function () {
    "use strict";

    $("body").tooltip({
        selector: "[data-toggle='tooltip']"
    });    
});



/***********************************************************************************
                        SWITCHES ON JQUERY UI DATEPICKER
***********************************************************************************/

$(function () {
    "use strict";

    $(document).on("focus", ".datepicker", function () {
        $(this).datepicker({
            changeMonth: true,
            changeYear: true,
            yearRange: "1900:+nn",
            dateFormat: "DD, MM d, yy",
            showAnim: ""
        });
    });

    // to prevent backspace in input from sending user to prior page
    $(document).on("keydown", "input[readonly]", function (e) {
        e.preventDefault();
    });

    // to force datepicker to be in correct place on resize
    $(window).resize(function () {
        var field = $(document.activeElement);
        if (field.is('.hasDatepicker')) {
            field.datepicker('hide').datepicker('show');
        }
    });
});

// $("#carousel").carousel();  this will activate a carousel