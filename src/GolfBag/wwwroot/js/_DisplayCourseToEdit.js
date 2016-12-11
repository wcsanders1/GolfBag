$(function () {
    "use strict";

    var deleteTeebox = function () {
        var $teeboxToDelete = $(this),
            $dates = [],
            $id = $teeboxToDelete.parents("tr").attr("data-teebox-id"),
            options = {
                async: false,
                url: "/RoundOfGolf/DatesPlayedTeebox/" + $id,
                type: "GET",
            };

        $teeboxToDelete.tooltip("hide");

        $.ajax(options).done(function (data) {

            if (data.length === 0) {
                $teeboxToDelete
                    .tooltip("disable")
                    .addClass("faded-font")
                    .prop("disabled", true);

                $teeboxToDelete
                    .parents("tr")
                    .find(".undo-delete")
                    .removeClass("hidden");

                $(".teebox-row").each(function (i) {
                    if ($(this).attr("data-teebox-id") === $id) {
                        $(this)
                            .find("input, p")
                            .addClass("teebox-to-delete")
                            .prop("disabled", true);
                    }
                });
            } else {
                for (var i = 0; i < data.length; i++) {
                    var date = "";
                    while (data.charAt(i) != ":") {
                        date += data.charAt(i);
                        i++;
                    }
                    $dates.push(date);
                }
                
                var $modal = $("#delete-teebox-modal");
                $(".teebox-name").each(function() {
                    $(this).text($teeboxToDelete.parents("tr").attr("data-teebox-name"));
                });
                var $htmlDates = "";
                $.each($dates, function (i) {
                    $htmlDates += "<p>" + $dates[i] + "</p>";
                });
                console.log($htmlDates);
                $("#dates-teebox-played").html($htmlDates);
                $modal.modal("show");
            }
        });
    };

    var undoDeleteTeebox = function () {
        var $deleteToUndo = $(this),
            $id = $(this).parents("tr").attr("data-teebox-id");

        $deleteToUndo.addClass("hidden");

        $(".teebox-row").each(function (i) {
            if ($(this).attr("data-teebox-id") === $id) {

                $(this)
                    .find("input, p")
                    .removeClass("teebox-to-delete")
                    .prop("disabled", false);

                $(this)
                    .find(".delete-teebox")
                    .tooltip("enable")
                    .removeClass("faded-font")
                    .prop("disabled", false);
            }
        });
    };

    $(document).on("click", ".delete-teebox", deleteTeebox);
    $(document).on("click", ".undo-delete", undoDeleteTeebox);
});