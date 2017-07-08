/**********************************************************************************
            SCREEN-SIZE DETECTOR:
                - Each of the below constants correspond to the z-index of #page-size,
                    which alters according to bootstrap's breakpoints
                - This allows js to sync with bootstrap's breakpoints
**********************************************************************************/

const SCREEN_XS = 1;
const SCREEN_SM = 2;
const SCREEN_MD = 3;
const SCREEN_LG = 4;
const SCREEN_XL = 5;

var getScreenSize = function () {
    return $("#page-size").height();
};



/***********************************************************************************
                ACTIVATE CAROUSEL
***********************************************************************************/

$(".carousel").carousel({
    interval: 3000
});


/***********************************************************************************
                USED TO MAKE TEEBOX NAMES CORRESPOND WHEN ENTERING AND
                EDITING COURSES
***********************************************************************************/

var makeTeeboxNamesCorrespond = function () {
    $(".teebox-name-input").each(function () {
        var $curInput = $(this);
        $(".teebox-name-label" + $curInput.data("teebox-number")).text($curInput.val());
    });
};

var makeNewTeeboxNamesCorrespond = function () {
    $(".new-teebox-name-input").each(function () {
        var $curInput = $(this);
        $(".new-teebox-name-label" + $curInput.data("new-teebox-num")).text($curInput.val());
    });
};


/***********************************************************************************
                        AJAX AND JQUERY FORM VALIDATION CALL
 **********************************************************************************/

$(function () {
    "use strict";
    var ajaxGetCourse = function () {
        var $course = $(this),
            options = {
                url: $course.attr("data-action"),
                type: "GET",
                data: $course.serialize(),
                cache: false
            };

        $.ajax(options).done(function (data) {
            var $target = $($course.attr("data-target"));
            var $newHtml = $(data);
            $target.html($newHtml);
            $.validator.unobtrusive.parse($(".writable-scorecard"));
            validateForm($(".writable-scorecard"), false, false);
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
            maxDate: "today",
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
        if (field.is(".hasDatepicker")) {
            field.datepicker("hide").datepicker("show");
        }
    });
});


/***********************************************************************************
                REMOVE SIDEBAR IF EMPTY AND SCREEN NOT FULL SIZE
                (cannot use CSS :empty because @RenderSection makes witespace)
***********************************************************************************/

var hideEmptySideBar = function () {
    if (getScreenSize() < SCREEN_LG) {
        $(".sidebar").each(function () {
            if ($(this).children().length < 1) {
                $(this).hide();
            }
        });
    } else {
        $(".sidebar").each(function () {
            $(this).show();
        });
    }
};

$(document).ready(function () {
    hideEmptySideBar();
});

$(window).resize(function () {
    hideEmptySideBar();
});