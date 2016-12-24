/*********************************************************************************************
                        STRIPES ROW OF SELECTED TEEBOX
*********************************************************************************************/
$(function () {
    "use strict";

    var stripeRow = function () {
        var $selectedTeeBoxId = $(this).val();
        console.log($selectedTeeBoxId);
        $(".table").each(function (i) {
            $(this).find("tr").each(function (i) {
                $(this).children().removeClass("teebox-played");
                console.log($(this).data("teebox-id"));
                if ($(this).data("teebox-id") == $selectedTeeBoxId) {
                    $(this).children().addClass("teebox-played");
                }
            });
        });
    };

    $(document).on("change", ".teebox-selector", stripeRow);
});