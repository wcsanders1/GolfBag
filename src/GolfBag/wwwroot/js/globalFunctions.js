/**********************************************************************************
            GLOBAL FUNCTIONS
**********************************************************************************/

var stripeRow = function ($teebox) {
    "use strict";

    var $selectedTeeBoxId = $teebox.val();
    $(".table").each(function (i) {
        $(this).find("tr").each(function (i) {
            $(this).children().removeClass("teebox-played");
            if ($(this).data("teebox-id") == $selectedTeeBoxId) {
                $(this).children().addClass("teebox-played");
            }
        });
    });
};