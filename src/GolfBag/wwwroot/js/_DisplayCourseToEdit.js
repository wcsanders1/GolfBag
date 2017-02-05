/********************************************************************************************************
                    DELETES AND UN-DELEETES EXISTING TEE BOXES
********************************************************************************************************/

$(function () {
    "use strict";

    /*******************   deleteTeebox()   **************************************************/

    var deleteTeebox = function () {
        var $teeboxToDelete = $(this),
            $modal = $("#delete-modal"),
            $id = $teeboxToDelete.parents("tr").attr("data-teebox-id");

        var roundIsAssociatedWithTeebox = function () {
            var roundIsAssociated,
                options = {
                    async: false,
                    url: "/RoundOfGolf/DatesPlayedTeebox/" + $id,
                    type: "GET",
                };

            var getDatesPlayed = function (dateData) {
                var $dates = [];
                
                for (var i = 0; i < dateData.length; i++) {
                    var date = "";
                    while (dateData.charAt(i) != ":") {
                        date += dateData.charAt(i);
                        i++;
                    }
                    $dates.push(date);
                }
                return $dates;
            };

            var displayModal = function ($dates, $teeboxName) {
                var $htmlDates = "";

                $("#modal-header-p").text("You cannot delete the " + $teeboxName + " tees.");

                if ($dates.length === 1) {
                    $("#modal-body-h1").text("You played the " + $teeboxName + " tees on the following day:");
                    $("#modal-body-h2").text("You must edit that round before you can delete the " + $teeboxName + " tees.");
                } else {
                    $("#modal-body-h1").text("You played the " + $teeboxName + " tees on the following days:");
                    $("#modal-body-h2").text("You must edit those rounds before you can delete the " + $teeboxName + " tees.");
                }

                $.each($dates, function (i) {
                    $htmlDates += "<p>" + $dates[i] + "</p>";
                });

                $("#dates-played").html($htmlDates);
                $modal.modal("show");
            };

            $.ajax(options).done(function (data) {
                if (data.length > 0) {
                    var $dates = [],
                        $teeboxName = "";                       

                    $dates = getDatesPlayed(data);
                    $teeboxName = $teeboxToDelete.parents("tr").attr("data-teebox-name");
                    displayModal($dates, $teeboxName);                   
                    roundIsAssociated = true;
                } else {
                    roundIsAssociated = false;
                }                
            });
            return roundIsAssociated;
        };

        var everyTeeboxIsAlreadyDeleted = function () {
            var numberOfTeeboxes = 0,
                numberOfTeeboxesToDelete = 0;

            var showModal = function () {
                $("#modal-header-p").text("Hey!");
                $("#modal-body-h1").text("You can't delete all the teeboxes!");
                $("#modal-body-h2").text("Why would you want to do that anyway?");
                $modal.modal("show");
            };

            $("#front-nine-table").find(".teebox-row").each(function (i) {
                numberOfTeeboxes++;
                if ($(this).hasClass("teebox-row-to-delete")) {
                    numberOfTeeboxesToDelete++;
                }
            });

            if (numberOfTeeboxesToDelete >= (numberOfTeeboxes - 1)) {
                showModal();
                return true;
            } else {
                return false;
            }
        };

        var disableToolTip = function () {
            $teeboxToDelete
                .tooltip("disable")
                .addClass("faded-font")
                .prop("disabled", true);
        };

        var enableUndoDelete = function () {
            $teeboxToDelete
                .parents("tr")
                .find(".undo-delete")
                .removeClass("hidden");
        };

        var disableDeletedTeebox = function () {
            $(".teebox-row").each(function (i) {
                if ($(this).attr("data-teebox-id") === $id) {
                    $(this)
                        .addClass("teebox-row-to-delete")
                        .find("input, p")
                        .addClass("teebox-to-delete")
                        .prop("readonly", true);
                }
            });
        };

        var addDeletedTeeboxToListOfDeletedTeeboxes = function () {
            $(".deleted-teebox-input").each(function (i) {
                if ($(this).val() == 0) {
                    $(this).val($id);
                    return false;
                }
            });
        };

        $teeboxToDelete.tooltip("hide");

        if (roundIsAssociatedWithTeebox()) {
            return false;
        }

        if (everyTeeboxIsAlreadyDeleted()) {
            return false;
        }
       
        disableToolTip();
        enableUndoDelete();
        disableDeletedTeebox();
        addDeletedTeeboxToListOfDeletedTeeboxes();        
    };


    /*******************   undoDeleteTeebox()   **************************************************/

    var undoDeleteTeebox = function () {
        var $deleteToUndo = $(this),
            $id = $(this).parents("tr").attr("data-teebox-id");

        var enableTeebox = function ($teebox) {
            $teebox
                .removeClass("teebox-row-to-delete")
                .find("input, p")
                .removeClass("teebox-to-delete")
                .prop("readonly", false);
        };

        var enableDeleteIcon = function ($teebox) {
            $teebox
                .find(".delete-teebox")
                .tooltip("enable")
                .removeClass("faded-font")
                .prop("disabled", false);
        };

        var removeTeeboxFromList = function () {
            $(".deleted-teebox-input").each(function (i) {
                if ($(this).val() == $id) {
                    $(this).val(0);
                    return false;
                }
            });
        };

        $deleteToUndo.addClass("hidden");
        removeTeeboxFromList();

        $(".teebox-row").each(function (i) {
            if ($(this).attr("data-teebox-id") === $id) {
                var $teeboxRow = $(this);

                enableTeebox($teeboxRow);
                enableDeleteIcon($teeboxRow);
            }
        });
    };

    $(document).on("click", ".delete-teebox", deleteTeebox);
    $(document).on("click", ".undo-delete", undoDeleteTeebox);
});


/********************************************************************************************************
                    ALLOWS NEW TEEBOXES TO BE ADDED TO A COURSE
********************************************************************************************************/

$(function () {
    "use strict";

    var setButtonStatus = function ($newTeeboxRows, $addBtn, $removeBtn) {
        var numberOfNewTeeboxRows = 0,
            numberOfHiddenNewTeeboxRows = 0;

        $newTeeboxRows.each(function (i) {
            numberOfNewTeeboxRows++;
            if ($(this).hasClass("hidden")) {
                numberOfHiddenNewTeeboxRows++;
            }
        });
        if (numberOfHiddenNewTeeboxRows > 0) {
            $addBtn.removeClass("disabled");
        } else {
            $addBtn.addClass("disabled");
        }
        if (numberOfNewTeeboxRows > numberOfHiddenNewTeeboxRows) {
            $removeBtn.removeClass("disabled");
        } else {
            $removeBtn.addClass("disabled");
        }
    };

    var showNewTeebox = function () {
        var $frontNineNewTeeboxRows = $("#front-nine-table").find(".new-teebox-row"),
            $backNineNewTeeboxRows = $("#back-nine-table").find(".new-teebox-row"),
            $addTeeboxBtn = $("#add-teebox-btn"),
            $removeNewTeeboxBtn = $("#remove-new-teebox");

        var showBackNineNewTeebox = function (newTeeboxNum) {
            $backNineNewTeeboxRows.each(function (i) {
                if ($(this).attr("data-new-teebox-num") === newTeeboxNum) {
                    $(this).removeClass("hidden");
                    return false;
                }
            });
        };

        $frontNineNewTeeboxRows.each(function (i) {
            if ($(this).hasClass("hidden")) {
                $(this).removeClass("hidden");
                if ($backNineNewTeeboxRows.length > 0) {
                    showBackNineNewTeebox($(this).attr("data-new-teebox-num"));
                }
                setButtonStatus($frontNineNewTeeboxRows, $addTeeboxBtn, $removeNewTeeboxBtn);
                $(".custom-submit").off();
                validateForm($(".writable-scorecard"), true, false);
                return false;
            }
        });

    };

    var removeNewTeebox = function () {
        var $frontNineNewTeeboxRows = $("#front-nine-table").find(".new-teebox-row"),
            $backNineNewTeeboxRows = $("#back-nine-table").find(".new-teebox-row"),
            $addTeeboxBtn = $("#add-teebox-btn"),
            $removeNewTeeboxBtn = $("#remove-new-teebox");

        var hideBackNineNewTeebox = function (newTeeboxNum) {
            for (var i = $backNineNewTeeboxRows.length - 1; i >= 0; i--) {
                if ($backNineNewTeeboxRows.eq(i).attr("data-new-teebox-num") === newTeeboxNum) {
                    $backNineNewTeeboxRows.eq(i).addClass("hidden");
                    return false;
                }
            }
        };

        for (var i = $frontNineNewTeeboxRows.length - 1; i >= 0; i--) {
            if (!($frontNineNewTeeboxRows.eq(i).hasClass("hidden"))) {
                $frontNineNewTeeboxRows.eq(i).addClass("hidden");
                hideBackNineNewTeebox($frontNineNewTeeboxRows.eq(i).attr("data-new-teebox-num"));
                setButtonStatus($frontNineNewTeeboxRows, $addTeeboxBtn, $removeNewTeeboxBtn);
                $(".custom-submit").off();
                validateForm($(".writable-scorecard"), true, true);
                return false;
            }
        }
    }

    $(document).on("click", "#add-teebox-btn", showNewTeebox);
    $(document).on("click", "#remove-new-teebox", removeNewTeebox);
});



/********************************************************************************************************
                    ENSURES NO ROUNDS ARE ASSOCIATED WITH COURSE BEFORE DELETING COURSE
********************************************************************************************************/

$(function () {
    "use strict";

    var deleteCourse = function () {
        var $courseId = $(this).attr("data-course-id"),
            $courseName = $(this).attr("data-course-name"),
            $modal = $("#delete-modal");

        var roundIsAssociatedWithCourse = function () {
            var roundIsAssociated,
                options = {
                    async: false,
                    url: "/RoundOfGolf/DatesPlayedCourse/" + $courseId,
                    type: "GET",
                };

            var getDatesPlayed = function (dateData) {
                var $dates = [];

                for (var i = 0; i < dateData.length; i++) {
                    var date = "";
                    while (dateData.charAt(i) != ":") {
                        date += dateData.charAt(i);
                        i++;
                    }
                    $dates.push(date);
                }
                return $dates;
            };

            var displayModal = function ($dates, $courseName) {
                var $htmlDates = "";

                $("#modal-header-p").text("You cannot delete the " + $courseName + " course.");

                if ($dates.length === 1) {
                    $("#modal-body-h1").text("You played the " + $courseName + " course on the following day:");
                    $("#modal-body-h2").text("You must delete that round before you can delete the " + $courseName + " course.");
                } else {
                    $("#modal-body-h1").text("You played the " + $courseName + " course on the following days:");
                    $("#modal-body-h2").text("You must delete those rounds before you can delete the " + $courseName + " course.");
                }

                $.each($dates, function (i) {
                    $htmlDates += "<p>" + $dates[i] + "</p>";
                });

                $("#dates-played").html($htmlDates);
                $modal.modal("show");
            };

            $.ajax(options).done(function (data) {
                if (data.length > 0) {
                    var $dates = [];

                    $dates = getDatesPlayed(data);
                    displayModal($dates, $courseName);
                    roundIsAssociated = true;
                } else {
                    roundIsAssociated = false;
                }
            });
            return roundIsAssociated;
        };

        if (roundIsAssociatedWithCourse()) {
            return false;
        } else {
            $("#modal-header-p").text("Delete " + $courseName + " course.");
            $("#modal-body-h1").text("Are you sure you want to delete the " + $courseName + " course?");
            $("#modal-body-h2").text("");
            $("#delete-confirm-button").removeClass("hidden");
            $("#cancel-button").text("Don't Delete");
            $modal.modal("show");
        }
    };

    $(document).on("click", "#delete-course-button", deleteCourse);
});



/*********************************************************************************************
               JQUERY VALIDATOR ON FORM
*********************************************************************************************/

$(function () {
    "use strict";

    validateForm($(".writable-scorecard"), false, false);

    var validateThis = function () {
        validateCustom($(".writable-scorecard"));
    };

    $(document).on("click", ".custom-submit", validateThis);
});