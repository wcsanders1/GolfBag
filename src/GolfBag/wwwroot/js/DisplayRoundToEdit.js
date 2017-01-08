/*********************************************************************************************
                        STRIPES ROW OF SELECTED TEEBOX
*********************************************************************************************/
$(function () {
    "use strict";

    //var stripeRow = function () {
    //    var $selectedTeeBoxId = $(this).val();
    //    $(".table").each(function (i) {
    //        $(this).find("tr").each(function (i) {
    //            $(this).children().removeClass("teebox-played");
    //            if ($(this).data("teebox-id") == $selectedTeeBoxId) {
    //                $(this).children().addClass("teebox-played");
    //            }
    //        });
    //    });
    //};

    var stripe = function () {
        stripeRow($(this));
    }

    $(document).on("change", ".teebox-selector", stripe);
});