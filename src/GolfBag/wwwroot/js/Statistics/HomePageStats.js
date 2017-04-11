$(function () {
    "use strict";

    var makeScoreBarChart = function ($location, numOfHoles, mostRecentRounds, label, animation = false) {
        var options = {
            type: "GET",
            url: "/Statistics/GetScores",
            data: {
                holes: numOfHoles,
                mostRecentScores: mostRecentRounds
            },
            dataType: "json",
            success: function (data) {
                renderChartsAndGraphs.barChart(data, $location, label, animation);
                $(window).resize(function () {
                    resizeChartsAndGraphs.barChart(data);
                });
            }
        };

        $.ajax(options);
    };

    $(document).ready(function () {
        // pass in the div you want the chart appended to, the number of holes the chart will represent,
        // the most recent number of rounds the chart will represent, and any animation
        makeScoreBarChart("#left-sidebar", 9, 10, "Last Ten 9-Hole Rounds", "bounceInLeft");
    });
});