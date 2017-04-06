$(function () {
    "use strict";

    var makeScoreBarChart = function ($location, animation = false) {
        var options = {
            type: "GET",
            url: "/Statistics/GetScores",
            data: "{}",
            dataType: "json",
            success: function (data) {
                renderChartsAndGraphs.barChart(data, $location, animation);
                $(window).resize(function () {
                    resizeChartsAndGraphs.barChart(data);
                });
            }
        };

        $.ajax(options);
    };

    $(document).ready(function () {
        makeScoreBarChart("#left-sidebar", "bounceInLeft");
    });
});