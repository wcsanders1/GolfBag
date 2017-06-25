$(function () {
    "use strict";

    var makeScoreBarChart = function (id, location, numOfHoles, mostRecentRounds, animation) {
        var options = {
            type: "GET",
            url: "/Statistics/GetScores",
            data: {
                holes: numOfHoles,
                mostRecentScores: mostRecentRounds
            },
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.length < 2) {
                    return;
                }
                renderChartsAndGraphs.barChart(data, id, numOfHoles, location, animation);
                $(window).resize(function () {
                    resizeChartsAndGraphs.barChart(data, id, numOfHoles, location);
                });
                //  This timeout resizing is necessary so that in a large screen the rendering of the barcharts doesn't conflict with bootstrap
                setTimeout(
                    function () {
                        resizeChartsAndGraphs.barChart(data, id, numOfHoles, location, animation);
                    },
                    750
                );
            }
        };

        $.ajax(options);
    };

    var makePuttsLineGraph = function (id, location, numOfHoles, mostRecentRounds, animation) {
        var options = {
            type: "GET",
            url: "/Statistics/GetPutts",
            data: {
                holes: numOfHoles,
                mostRecentScores: mostRecentRounds
            },
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.length < 2) {
                    return;
                }
                renderChartsAndGraphs.lineGraph(data, id, numOfHoles, location, animation);
                $(window).resize(function () {
                    resizeChartsAndGraphs.lineGraph(data, id, numOfHoles, location);
                });
            },
            error: {}
        };

        $.ajax(options);
    };

    var makeScoreToParPieChart = function (id, location, mostRecentRounds, animation) {
        var options = {
            type: "GET",
            url: "/Statistics/GetScoresToPar",
            data: {
                mostRecentScores: mostRecentRounds
            },
            dataType: "json",
            success: function (data) {
                if (data.length < 2) {
                    return;
                }
                renderChartsAndGraphs.pieChart(data, id, location, animation);
                $(window).resize(function () {
                    resizeChartsAndGraphs.pieChart(data, id, location);
                });
            },
            error: {}
        };

        $.ajax(options);
    };

    var makePuttToTwoPieChart = function (id, location, mostRecentRounds, animation) {
        var options = {
            type: "GET",
            url: "/Statistics/GetPuttsToTwo",
            data: {
                mostRecentRounds: mostRecentRounds
            },
            dataType: "json",
            cache: false,
            success: function (data) {
                if (data.length < 2) {
                    return;
                }
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
        makeScoreBarChart("nine-hole-score-barchart", "#nine-hole-score-barchart-container", 9, 10, "bounceInLeft");
        makePuttsLineGraph("nine-hole-putts-linegraph", "#nine-hole-putts-linegraph-container", 9, 10, "bounceInLeft");
        makeScoreToParPieChart("score-to-par-piechart", "#score-to-par-piechart-container", 10, "bounceInLeft");
        makeScoreBarChart("eighteen-hole-score-barchart", "#eighteen-hole-score-barchart-container", 18, 10, "bounceInRight");
        makePuttsLineGraph("eighteen-hole-putts-linegraph", "#eighteen-hole-putts-linegraph-container", 18, 10, "bounceInRight");
        makePuttToTwoPieChart("score-to-two-piechart", "#score-to-two-piechart-container", 10, "bounceInRight");
    });
});

$(function () {
    "use strict";

    var showRound = function () {
        var $round = $(this);

        window.location.href = "RoundOfGolf/ViewRounds?selectedRound=" + $round.data("round-id");
    };

    $(document).on("click", "rect, .link-label", showRound);
});