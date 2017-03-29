/*********************************************************************************************
          TOGGLES LIGHTBULB ICON AND STRIPES SCORECARD WHEN TOGGLING TEEBOXES AND PUTTS
*********************************************************************************************/
$(function () {
    "use strict";

    var toggleIcons = function () {
        var shownTeeboxes = [];

        $(".teebox-not-played").each(function () {
            if ($(this).hasClass("in")) {
                shownTeeboxes.push($(this).attr("data-teebox-id"));
            }
        });

        $(".teebox-toggler-icon").each(function () {
            $(this).addClass("hidden");
        });

        $.each(shownTeeboxes, function (i) {
            $(".teebox-toggler-icon").each(function () {
                if ($(this).attr("data-teebox-id") == shownTeeboxes[i]) {
                    $(this).removeClass("hidden");
                }
            });
        });
    };

    var stripeTable = function () {
        $(".table-striped-custom").each(function (i) {
            var x = 2;
            $(this).find("tr").each(function (i) {
                $(this).removeClass("table-row-striped");
                if ((!($(this).hasClass("collapse")) && (x % 2 === 0))
                        || ($(this).hasClass("in")) && (x % 2 === 0)) {
                    $(this).addClass("table-row-striped");
                    x++;
                } else if ((!($(this).hasClass("collapse")))
                        || ($(this).hasClass("in"))) {
                    x++;
                }
            });
        });
        toggleIcons();
    };

    $(document).on("shown.bs.collapse hidden.bs.collapse", ".collapse", stripeTable);
});