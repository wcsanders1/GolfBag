$(function () {
    "use strict";

    var deleteTeebox = function () {
        var $teeboxToDelete = $(this),
            $numberOfTeeboxesToDelete = 0,
            $dates = [],
            $id = $teeboxToDelete.parents("tr").attr("data-teebox-id"),
            options = {
                async: false,
                url: "/RoundOfGolf/DatesPlayedTeebox/" + $id,
                type: "GET",
            };

        $teeboxToDelete.tooltip("hide");

        $(".teebox-row").each(function (i) {
            if ($(this).hasClass("teebox-row-to-delete")) {
                $numberOfTeeboxesToDelete++;
            }
        });

        if ($numberOfTeeboxesToDelete >= ($(".teebox-row").length) - 1) {   //this doesn't work
            alert("You can't delete all the teeboxes.");
            console.log($numberOfTeeboxesToDelete);
            return false;
        }

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
                            .addClass("teebox-row-to-delete")
                            .find("input, p")
                            .addClass("teebox-to-delete")
                            .prop("readonly", true);
                    }
                });

                $(".deleted-teebox-input").each(function (i) {
                    if ($(this).val() == 0) {
                        $(this).val($id);
                        return false;
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
                $(".teebox-name").each(function () {
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
                    .removeClass("teebox-row-to-delete")
                    .find("input, p")
                    .removeClass("teebox-to-delete")
                    .prop("readonly", false);

                $(this)
                    .find(".delete-teebox")
                    .tooltip("enable")
                    .removeClass("faded-font")
                    .prop("disabled", false);
            }
        });

        $(".deleted-teebox-input").each(function (i) {
            if ($(this).val() == $id) {
                $(this).val(0);
                return false;
            }
        });
    };

    $(document).on("click", ".delete-teebox", deleteTeebox);
    $(document).on("click", ".undo-delete", undoDeleteTeebox);
});