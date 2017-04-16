"use strict";

var renderChartsAndGraphs = {
    barChart: function (data, id, numOfHoles, $location, animation) {
        console.log(data);
        const HEIGHT_INCREASE = 1.5;
        var h = statCalculations.getHighestScore(data) * HEIGHT_INCREASE,
            padding = 2,
            dataset = data,
            chart = d3.select($location)
                .append("svg")
                .attr("id", id)
                .attr("class", "stat-chart")
                .attr("height", h);

        var w = $("#" + id).width();

        var colorPicker = function (v) {
            if (v <= 20) {
                return "#666";
            } else if (v > 20) {
                return "#F03";
            }
        };

        chart.selectAll("rect")
            .data(dataset)
            .enter()
            .append("rect")
            .attrs({
                x: function (d, i) { return i * (w / dataset.length); },
                y: function (d) { return h - (d.roundScore * HEIGHT_INCREASE); },
                width: w / dataset.length - padding,
                height: function (d) { return d.roundScore * HEIGHT_INCREASE; },
                fill: function (d) { return colorPicker(d.roundScore); }
            });

        chart.selectAll("text")
            .data(dataset)
            .enter()
            .append("text")
            .text(function (d) { return d.roundScore; })
            .attrs({
                "text-anchor": "middle",
                x: function (d, i) { return i * (w / dataset.length) + (w / dataset.length - padding) / 2; },
                y: function (d) { return h - (d.roundScore * HEIGHT_INCREASE) + 14; },
                "font-family": "sans-serif",
                "font-size": 12,
                "fill": "#fff"
            });

        var $label = $("<h6>Your Latest " + numOfHoles + "-Hole Rounds</h6>");
        $("#" + id).after($label);

        if (animation != undefined) {
            $("#" + id)
                .addClass("animated")
                .addClass(animation);

            $label.addClass("animated")
                .addClass(animation);
        }
    }     
};

var resizeChartsAndGraphs = {
    barChart: function (id, data) {
        var chart = d3.select("#" + id),
            w = $("#" + id).width(),
            dataset = data,
            padding = 2;

        chart.selectAll("rect")
            .attrs({
                x: function (d, i) { return i * (w / dataset.length); },
                width: w / dataset.length - padding,
            });

        chart.selectAll("text")
            .attr("x", function (d, i) { return i * (w / dataset.length) + (w / dataset.length - padding) / 2; });
    }
};

var statCalculations = {
    getHighestScore: function (data) {
        var highestScore = 0;
        for (var i = 0; i < data.length; i++) {
            if (data[i].roundScore > highestScore) {
                highestScore = data[i].roundScore;
            }
        }
        return highestScore;
    }
}