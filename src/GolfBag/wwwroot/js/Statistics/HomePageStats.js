$(function () {
    "use strict";

    var makeScoreBarChart = function ($location, id, numOfHoles, mostRecentRounds, animation = false) {
        var options = {
            type: "GET",
            url: "/Statistics/GetScores",
            data: {
                holes: numOfHoles,
                mostRecentScores: mostRecentRounds
            },
            dataType: "json",
            success: function (data) {
                renderChartsAndGraphs.barChart(data, id, $location, animation);
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
        makeScoreBarChart("#left-sidebar", "nine-hole-bar-chart", 9, 10, "bounceInLeft");
        makeScoreBarChart("#left-sidebar", "eightteen-hole-bar-chart", 18, 10, "bounceInLeft");
    });
});