$(function () {
    "use strict";

    var makeScoreBarChart = function ($location, id, numOfHoles, mostRecentRounds, animation) {
        var options = {
            type: "GET",
            url: "/Statistics/GetScores",
            data: {
                holes: numOfHoles,
                mostRecentScores: mostRecentRounds
            },
            dataType: "json",
            success: function (data) {
                renderChartsAndGraphs.barChart(data, id, numOfHoles, $location, animation);
                $(window).resize(function () {
                    resizeChartsAndGraphs.barChart(id, data);
                });
            }
        };

        $.ajax(options);
    };

    var makeScoreToParPieChart = function ($location, id, mostRecentRounds, animation) {
        var options = {
            type: "GET",
            url: "/Statistics/GetScoresToPar",
            data: {
                holes: numOfHoles,
                mostRecentScores, mostRecentRounds
            },
            dataType: "json",
            success: function (data) {
                renderChartsAndGraphs.pieChart(data, id, $location, animation);
            }
        };

        $.ajax(options);
    };

    $(document).ready(function () {
        // pass in the div you want the chart appended to, the number of holes the chart will represent,
        // ID of the chart, the most recent number of rounds the chart will represent, and any animation
        makeScoreBarChart("#nine-hole-score-barchart-container", "nine-hole-score-barchart", 9, 10, "bounceInLeft");
        makeScoreBarChart("#eighteen-hole-score-barchart-container", "eighteen-hole-score-barchart", 18, 10, "bounceInLeft");

        renderChartsAndGraphs.pieChart("fakeData", "score-to-par-piechart", "#score-to-par-piechart-container", "bounceInLeft");
    });
});