"use strict";

var svgtooltip = d3.select("body")
    .append("div")
    .attr("class", "svg-tooltip")
    .style("opacity", 0);

const WIDTH_DECREASE_FOR_SCROLLBAR = 20;   //subtract 20 so that container scrollbar doesn't cut off chart

/**********************************************************************************
            SCREEN-SIZE DETECTOR:
                - Each of the below constants correspond to the z-index of #page-size,
                    which alters according to bootstraps breakpoints
                - This allows js to sync with bootstraps breakpoints
**********************************************************************************/

const SCREEN_XS = 1;
const SCREEN_SM = 2;
const SCREEN_MD = 3;
const SCREEN_LG = 4;
const SCREEN_XL = 5;

var getScreenSize = function () {
    return $("#page-size").height();
};


 /**********************************************************************************/

var renderChartsAndGraphs = {
    barChart: function (data, id, numOfHoles, location, animation) {
        var screenSize = getScreenSize();
        var heightIncrease;

        console.log(screenSize);

        if (screenSize >= SCREEN_SM && screenSize <= SCREEN_MD) {
            heightIncrease = 3;
        } else {
            heightIncrease = 1.5;
        }

        var h = statCalculations.getHighestScore(data) * heightIncrease,
            padding = 2,
            dataset = data,
            chart = d3.select(location)
                .append("svg")
                .attr("id", id)
                .attr("class", "stat-chart")
                .attr("height", h);

        var w = $("#" + id).width() - WIDTH_DECREASE_FOR_SCROLLBAR;

        var colorPicker = function (v) {  //this is wrong; has no application to this application
            if (v <= 20) {
                return "#666";
            } else if (v > 20) {
                return "#9E3668";
            }
        };

        chart.selectAll("rect")
            .data(dataset)
            .enter()
            .append("rect")
            .attrs({
                x: function (d, i) { return i * (w / dataset.length); },
                y: function (d) { return h - d.roundScore * heightIncrease; },
                width: w / dataset.length - padding,
                height: function (d) { return d.roundScore * heightIncrease; },
                fill: function (d) { return colorPicker(d.roundScore); },
                "class": "btn",
                "data-round-id": function (d) { return d.roundId; }
            })
            .on("mouseover", function (d) {
                svgtooltip.transition()
                    .duration(200)
                    .style("opacity", .85);
                svgtooltip.html(d.courseName + "<br>" + d.roundDate)
                    .style("left", d3.event.pageX + "px")
                    .style("top", d3.event.pageY - 28 + "px");
            })
            .on("mouseout", function (d) {
                svgtooltip.transition()
                    .duration(0)
                    .style("opacity", 0);
                svgtooltip.style("left", 0 + "px");
                svgtooltip.style("top", 0 + "px");             
        });
        chart.selectAll("text")
            .data(dataset)
            .enter()
            .append("text")
            .text(function (d) { return d.roundScore; })
            .attrs({
                "text-anchor": "middle",
                x: function (d, i) { return i * (w / dataset.length) + (w / dataset.length - padding) / 2; },
                y: function (d) { return h - d.roundScore * heightIncrease + 14; },
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
    },
    pieChart: function (data, id, location, animation) {

        var dataset = data,
            chart = d3.select(location)
                .append("svg")
                    .attr("id", id)
                    .attr("class", "stat-chart")
                    .attr("height", $(location).width())
                    .append("g")
                        .attr("height", $(location).width()),
            w = $("#" + id).width() - WIDTH_DECREASE_FOR_SCROLLBAR, 
            h = $("#" + id).height(),
            pie = d3.pie()
                .value(function (data) { return data.percentage; })
                .sort(null);

        chart.attr("transform", "translate(" + w / 2 + "," + h / 2 + ")");

        var radius = Math.min(w, h) / 2,
            arc = d3.arc()
                .innerRadius(0)
                .outerRadius(radius);

        chart.selectAll("path")
            .data(pie(data))
            .enter()
            .append("path")
            .attr("d", arc);

        chart.selectAll("path")
            .data(dataset)
            .attr("class", function (dataset) { return dataset.scoreName; });

        var makeBarChartLabel = function (labels) {
            var $label = "<div class='pie-chart-label-container'>";

            $.each(labels, function (index, label) {
                var text = label.scoreName.toLowerCase().replace(/\b[a-z]/g, function (letter) {
                    return letter.toUpperCase();
                }) + "s: " + Math.floor(label.percentage) + "%";

                $label += "<div class='pie-chart-label'><h6 class='" + label.scoreName + "'>" + text + "</h6></div>";
                console.log(label);
            });

            $label += "</div>";

            return $label;
        };

        var $barChartLabel = makeBarChartLabel(dataset);
        $("#" + id).after($barChartLabel);

        if (animation !== undefined) {
            $("#" + id)
                .addClass("animated")
                .addClass(animation);

            $(".pie-chart-label")
                .addClass("animated")
                .addClass(animation);
        }
    }
};

var resizeChartsAndGraphs = {
    barChart: function (data, id, numOfHoles, location, animation) {
        $(location).empty();
        renderChartsAndGraphs.barChart(data, id, numOfHoles, location);
    },
    pieChart: function (data, id, location, animation) {
        $(location).empty();
        renderChartsAndGraphs.pieChart(data, id, location, animation);
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