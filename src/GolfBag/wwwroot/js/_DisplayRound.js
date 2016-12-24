/*********************************************************************************************
                        STRIPES SCORECARD WHEN TOGGLING TEEBOXES
*********************************************************************************************/

$(function () {
    "use strict";
    
    var stripeTables = function () {
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
    };

    $(document).ready(stripeTables);
    $(document).on("shown.bs.collapse hidden.bs.collapse", ".collapse", stripeTables);
});


/*********************************************************************************************
                        TOGGLES LIGHTBULB ICON WHEN TOGGLING TEEBOXES
*********************************************************************************************/
$(function () {
    "use strict";

    var toggleIcon = function () {
        var $icon = $(this).siblings(".teebox-toggler-icon");

        if ($icon.hasClass("hidden")) {
            $icon.removeClass("hidden");
        } else {
            $icon.addClass("hidden");
        }
    };

    $(document).on("click", ".teebox-toggler", toggleIcon);
});