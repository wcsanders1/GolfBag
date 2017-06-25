$(function () {
    "use strict";

    var emptyStatContainers = function () {
        $("#nine-hole-score-by-course-barchart-container").show().empty();
        $("#eighteen-hole-score-by-course-barchart-container").show().empty();
        $("#nine-hole-putts-by-course-linegraph-container").show().empty();
        $("#eighteen-hole-putts-by-course-linegraph-container").show().empty();
        $("#score-to-par-piechart-container").show().empty();
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
            cache: false,
            success: function (data) {
                if (data.length < 2) {
                    $(location).hide();
                    return;
                }
                $(location).show();
                renderChartsAndGraphs.barChart(data, id, numOfHoles, location, animation);
                $(window).resize(function () {
                    resizeChartsAndGraphs.barChart(data, id, numOfHoles, location);
                });
            },
            error: {}
        };

        $.ajax(options);
    };

    var makePuttsLineGraph = function (id, location, numOfHoles, mostRecentRounds, courseId, animation) {
        var options = {
            type: "GET",
            url: "/Statistics/GetPuttsByCourse",
            data: {
                holes: numOfHoles,
                courseId: courseId,
                mostRecentScores: mostRecentRounds
            },
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.length < 2) {
                    $(location).hide();
                    return;
                }
                $(location).show();
                renderChartsAndGraphs.lineGraph(data, id, numOfHoles, location, animation);
                $(window).resize(function () {
                    resizeChartsAndGraphs.lineGraph(data, id, numOfHoles, location);
                });
            },
            error: {}
        };

        $.ajax(options);
    };

    var makeScoreToParPieChart = function (id, location, mostRecentRounds, courseId, animation) {
        var options = {
            type: "GET",
            url: "/Statistics/GetScoresToParByCourse",
            data: {
                courseId: courseId,
                mostRecentScores: mostRecentRounds
            },
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.length < 2) {
                    $(location).hide();
                    return;
                }
                $(location).show();
                renderChartsAndGraphs.pieChart(data, id, location, animation);
                $(window).resize(function () {
                    resizeChartsAndGraphs.pieChart(data, id, location);
                });
            },
            error: {}
        };

        $.ajax(options);
    };

    $(document).ready(function () {
        emptyStatContainers();
        displayCourseName();
        makeScoreBarChart("nine-hole-score-by-course-barchart", "#nine-hole-score-by-course-barchart-container", 9, 10, $("#course-id").val(), "fadeIn");
        makeScoreBarChart("eighteen-hole-score-by-course-barchart", "#eighteen-hole-score-by-course-barchart-container", 18, 10, $("#course-id").val(), "fadeIn");
        makePuttsLineGraph("nine-hole-putts-by-course-linegraph", "#nine-hole-putts-by-course-linegraph-container", 9, 10, $("#course-id").val(), "fadeIn");
        makePuttsLineGraph("eighteen-hole-putts-by-course-linegraph", "#eighteen-hole-putts-by-course-linegraph-container", 18, 10, $("#course-id").val(), "fadeIn");
        makeScoreToParPieChart("score-to-par-piechart", "#score-to-par-piechart-container", 10, $("#course-id").val(), "fadeIn");
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