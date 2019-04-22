var chart;
var donutChart;
var opts = {
    lines: 13 // The number of lines to draw
, length: 28 // The length of each line
, width: 14 // The line thickness
, radius: 42 // The radius of the inner circle
, scale: 1 // Scales overall size of the spinner
, corners: 1 // Corner roundness (0..1)
, color: '#000' // #rgb or #rrggbb or array of colors
, opacity: 0.25 // Opacity of the lines
, rotate: 0 // The rotation offset
, direction: 1 // 1: clockwise, -1: counterclockwise
, speed: 1 // Rounds per second
, trail: 60 // Afterglow percentage
, fps: 20 // Frames per second when using setTimeout() as a fallback for CSS
, zIndex: 2e9 // The z-index (defaults to 2000000000)
, className: 'spinner' // The CSS class to assign to the spinner
, top: '50%' // Top position relative to parent
, left: '50%' // Left position relative to parent
, shadow: false // Whether to render a shadow
, hwaccel: false // Whether to use hardware acceleration
, position: 'absolute' // Element positioning
}



function GenerateReport(reportData) {
    if (chart !== undefined) {
        chart.destroy();
    }

    if (donutChart !== undefined) {
        donutChart.destroy();
    }

    CreateBarChart(reportData);
    CreateDonutChart(reportData);
};

function CreateBarChart(reportData) {
    var barLabels = [];
    var barValues = [];
    reportData.forEach(function (obj) {
        var name = obj.Label;
        if (obj.Name) {
            name = obj.Name + ' ' + obj.Label;
        }
        barLabels.push(name);
    });
    reportData.forEach(function (obj) { barValues.push(obj.Total); });

    var barChartData = {
        labels: barLabels,
        datasets: [
            {
                fillColor: "rgba(151,187,205,0.5)",
                strokeColor: "rgba(151,187,205,0.8)",
                highlightFill: "rgba(151,187,205,0.75)",
                highlightStroke: "rgba(151,187,205,1)",
                data: barValues
            }
        ]
    }
    var ctx = document.getElementById("barCanvas").getContext("2d");
    chart = new Chart(ctx).Bar(barChartData, {
        responsive: true
    });
}
function CreateDonutChart(reportData){
    var donutData = [];
    
    reportData.forEach(function (o) {
        var randomColor = '#' + Math.floor(Math.random() * 16777215).toString(16);
        var name = o.Label;
        if (o.Name) {
            name = o.Name + ' ' + o.Label;
        }

        var dd = { value: o.Total, label: name, color: randomColor, highlight: '#808080' };
        donutData.push(dd);
    });

    var options = {
        segmentShowStroke: false,
        animateRotate: true,
        animateScale: false,
        percentageInnerCutout: 50,
        tooltipTemplate: "<%= label %> <%= value %>",
        animationEasing: 'easeInOutQuart',
        responsive: true
    }

    var ctxPie = document.getElementById("doughnutCanvas").getContext("2d");
    donutChart = new Chart(ctxPie).Doughnut(donutData, options);
    document.getElementById('doLegend').innerHTML = donutChart.generateLegend();
}

function ShowReport() {
    var report = $('#reportName').val();
    var startDate = $('#startDate').val();
    var endDate = $('#endDate').val();
    var app = $('#appfilter').val();

    var d1 = startDate.split('/');
    d1 = new Date(d1.pop(), d1.pop() - 1, d1.pop());

    var d2 = endDate.split('/');
    d2 = new Date(d2.pop(), d2.pop() - 1, d2.pop());

    if (d1 > d2) {
        $('#dateAlert').html('<div class="alert alert-danger fade in"><a href="#" class="close" data-dismiss="alert">&times;</a><strong>Start Date cannot be greater than the End Date.</strong></div>');
        $(".alert").delay(2000).fadeOut("slow", function () { $('#dateAlert').html('') });
        return;
    }

    $.ajax({
        type: 'GET',
        url: "/Report/RequestTypeReport",
        data: { reportName: report, startDate: startDate, endDate: endDate, appFilter: app },
        success: function (reportData) {
            GenerateReport(reportData);
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            var msg = JSON.parse(XMLHttpRequest.responseText);
            alert(msg);
        }
    });
}



function ActivatePopover() {
    $('#myModal').on('show.bs.modal', function (e) {
        var requestId = $(e.relatedTarget).data('request');
        $(e.currentTarget).find('#ThisRequestId').val(requestId);
        $('#devModal').html('Assign request '+ requestId + ' to a developer');
    });

    $('.popover-markup > .trigger').popover({
        trigger: "hover",
        html: true,
        title: "More Info",
        content: function () {
            return $(this).parent().find('.content').html();
        },
        container: "body",
        placement: 'right'
    });
}