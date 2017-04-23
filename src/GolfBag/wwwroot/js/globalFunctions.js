/**********************************************************************************
            GLOBAL FUNCTIONS
**********************************************************************************/

var stripeRow = function ($teebox) {
    "use strict";

    var $selectedTeeBoxId = $teebox.val();
    $(".table").each(function (i) {
        $(this).find("tr").each(function (i) {
            $(this).removeClass("teebox-played");
            $(this).children().removeClass("teebox-played");
            if ($(this).data("teebox-id") === $selectedTeeBoxId) {
                $(this).children().addClass("teebox-played");
            }
        });
    });
};

var configureFloatingInputs = function ($input) {
    "use strict";

    var $field = $input.closest(".field");
    if ($input.val() !== "") {
        $field.addClass("field-not-empty");
    } else {
        $field.removeClass("field-not-empty");
    }
};