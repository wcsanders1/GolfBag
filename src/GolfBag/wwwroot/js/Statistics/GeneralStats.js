$(function () {
    "use strict";

    var renderBarChart = function () {
        var h = 150,
            padding = 2,
            dataset = [5, 10, 14, 20, 25, 11, 25, 22, 18, 7],
            chart = d3.select("#left-sidebar")
                .append("svg")
                //.attr("width", "95%")
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

        $("#bar-chart").addClass("animated bounceInLeft");

    };

    var resizeChart = function () {
        var barChart = d3.select("#bar-chart"),
            w = $("#bar-chart").width(),
            dataset = [5, 10, 14, 20, 25, 11, 25, 22, 18, 7],
            padding = 2;

        barChart.selectAll("rect")
            .attrs({
                x: function (d, i) { return i * (w / dataset.length); },
                width: w / dataset.length - padding,
            });

        barChart.selectAll("text")
            .attr("x", function (d, i) { return i * (w / dataset.length) + (w / dataset.length - padding) / 2; });
    };

    $(document).ready(renderBarChart());

    $(window).resize(resizeChart);
});