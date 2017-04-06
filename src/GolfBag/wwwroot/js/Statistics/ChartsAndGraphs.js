﻿var renderChartsAndGraphs = {
    barChart: function (data, $location, animation = false) {
        var h = 150,
            padding = 2,
            dataset = data,
            chart = d3.select($location)
                .append("svg")
                .attr("id", "bar-chart")
                .attr("height", h);

        var w = $("#bar-chart").width();

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
                y: function (d) { return h - (d * 4); },
                width: w / dataset.length - padding,
                height: function (d) { return d * 4; },
                fill: function (d) { return colorPicker(d); }
            });

        chart.selectAll("text")
            .data(dataset)
            .enter()
            .append("text")
            .text(function (d) { return d; })
            .attrs({
                "text-anchor": "middle",
                x: function (d, i) { return i * (w / dataset.length) + (w / dataset.length - padding) / 2; },
                y: function (d) { return h - (d * 4) + 14; },
                "font-family": "sans-serif",
                "font-size": 12,
                "fill": "#fff"
            });

        $("#bar-chart").css({ top: 50, position: "relative" });

        if (animation) {
            $("#bar-chart")
                .addClass("animated")
                .addClass(animation);
        }
    }     
};

var resizeChartsAndGraphs = {
    barChart: function (data) {
        var chart = d3.select("#bar-chart"),
            w = $("#bar-chart").width(),
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