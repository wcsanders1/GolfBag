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

    $(document).ready(function () {
        // pass in the div you want the chart appended to, the number of holes the chart will represent,
        // ID of the chart, the most recent number of rounds the chart will represent, and any animation
        makeScoreBarChart("#nine-hole-score-barchart-container", "nine-hole-score-barchart", 9, 10, "bounceInLeft");
        makeScoreBarChart("#eighteen-hole-score-barchart-container", "eighteen-hole-score-barchart", 18, 10, "bounceInLeft");
    });
});