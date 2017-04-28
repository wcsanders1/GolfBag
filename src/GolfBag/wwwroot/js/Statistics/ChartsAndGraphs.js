"use strict";

var renderChartsAndGraphs = {
    barChart: function (data, id, numOfHoles, $location, animation) {
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
                "data-toggle": "tooltip",
                "title": function (d) { return d.courseName + "<br>" + d.roundDate; },
                x: function (d, i) { return i * (w / dataset.length); },
                y: function (d) { return h - d.roundScore * HEIGHT_INCREASE; },
                width: w / dataset.length - padding,
                height: function (d) { return d.roundScore * HEIGHT_INCREASE; },
                fill: function (d) { return colorPicker(d.roundScore); },
                "class": "btn",
                "data-round-id": function (d) { return d.roundId; }
            });

        chart.selectAll("text")
            .data(dataset)
            .enter()
            .append("text")
            .text(function (d) { return d.roundScore; })
            .attrs({
                "text-anchor": "middle",
                x: function (d, i) { return i * (w / dataset.length) + (w / dataset.length - padding) / 2; },
                y: function (d) { return h - d.roundScore * HEIGHT_INCREASE + 14; },
                "font-family": "sans-serif",
                "font-size": 12,
                "fill": "#fff"
            });

        var $label = $("<h6>Your Latest " + numOfHoles + "-Hole Rounds</h6>");
        $("#" + id).after($label);

        if (animation !== undefined) {
            $("#" + id)
                .addClass("animated")
                .addClass(animation);

            $label.addClass("animated")
                .addClass(animation);
        }

        $("rect").tooltip({
            "container": "body",
            html: true
        });
    },

    // this needs lots of work
    pieChart: function (data, id, $location, animation) {
        var dataset = [
            { label: 'Abulia', count: 10 },
            { label: 'Betelgeuse', count: 20 },
            { label: 'Cantaloupe', count: 30 },
            { label: 'Dijkstra', count: 40 }
        ],
            color = d3.scaleOrdinal(d3.schemeCategory20b),
            chart = d3.select($location)
                .append("svg")
                .attr("id", id)
                .attr("height", "300px")
                .attr("class", "stat-chart")
                .append("g"),
            w = $("#" + id).width(),
            h = $("#" + id).width(),
            pie = d3.pie()
                .value(function (d) { return d.count; })
                .sort(null);

        chart.attr("transform", "translate(" + (w / 2) + "," + (h / 2) + ")")
            .attr("width", w)
            .attr("height", h * 2);

        var radius = Math.min(w, h) / 2,
            arc = d3.arc()
                .innerRadius(0)
                .outerRadius(radius);


        chart.selectAll("path")
            .data(pie(dataset))
            .enter()
            .append("path")
            .attr("d", arc)
            .attr("fill", function (d) {
                return color(d.data.label);
            });
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
                width: w / dataset.length - padding
            });

        chart.selectAll("text")
            .attr("x", function (d, i) { return i * (w / dataset.length) + (w / dataset.length - padding) / 2; });
    },
    // this is very bad; much repetition
    pieChart: function (data, id, $location) {
        $("#score-to-par-piechart-container").empty();

        var dataset = [
            { label: 'Abulia', count: 10 },
            { label: 'Betelgeuse', count: 20 },
            { label: 'Cantaloupe', count: 30 },
            { label: 'Dijkstra', count: 40 }
        ],
            color = d3.scaleOrdinal(d3.schemeCategory20b),
            chart = d3.select($location)
                .append("svg")
                .attr("id", id)
                .attr("height", "300px")
                .attr("class", "stat-chart")
                .append("g"),
            w = $("#" + id).width(),
            h = $("#" + id).width(),
            pie = d3.pie()
                .value(function (d) { return d.count; })
                .sort(null);

        chart.attr("transform", "translate(" + (w / 2) + "," + (h / 2) + ")")
            .attr("width", w)
            .attr("height", h * 2);

        var radius = Math.min(w, h) / 2,
            arc = d3.arc()
                .innerRadius(0)
                .outerRadius(radius);

        chart.selectAll("path")
            .data(pie(dataset))
            .enter()
            .append("path")
            .attr("d", arc)
            .attr("fill", function (d) {
                return color(d.data.label);
            });
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
};

$(function () {
    "use strict";

    var showRound = function () {
        var $round = $(this);

        window.location.href = "RoundOfGolf/ViewRounds?selectedRound=" + $round.data("round-id");
    };    

    $(document).on("click", "rect", showRound);
});