$(function () {
    "use strict";

    var emptyStatContainers = function () {
        $("#nine-hole-score-by-course-barchart-container").empty();
    };

    var displayCourseName = function () {
        var courseName = $("#course-name").val();
        $("#right-sidebar").find("h3").text(courseName + " Statistics");
    };

    var makeScoreBarChart = function (id, location, numOfHoles, mostRecentRounds, courseId, animation) {
        var options = {
            type: "GET",
            url: "/Statistics/GetScoresByCourse",
            data: {
                holes: numOfHoles,
                courseId: courseId,
                mostRecentScores: mostRecentRounds
            },
            dataType: "json",
            success: function (data) {
                renderChartsAndGraphs.barChart(data, id, numOfHoles, location, animation);
                $(window).resize(function () {
                    resizeChartsAndGraphs.barChart(data, id, numOfHoles, location);
                });
            }
        };

        $.ajax(options);
    };

    $(document).ready(function () {
        emptyStatContainers();
        displayCourseName();
        // pass in the div you want the chart appended to, the number of holes the chart will represent,
        // ID of the chart, the most recent number of rounds the chart will represent, the course id, and any animation
        makeScoreBarChart("nine-hole-score-by-course-barchart", "#nine-hole-score-by-course-barchart-container", 9, 10, $("#course-id").val(), "fadeIn");
    });
});

$(function () {
    "use strict";

    var showRound = function () {
        var $round = $(this);

        window.location.href = "ViewRounds?selectedRound=" + $round.data("round-id");
    };

    $(document).on("click", "rect, .link-label", showRound);
});