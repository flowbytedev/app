




function renderLineChart(chartId, config) {

    // if chart does not exist, continue
    //if (!document.getElementById(chartId)) {
    //    return;
    //}

    if (charts[chartId]) {
        charts[chartId].destroy(); // Destroy existing chart instance
    }
    const ctx = document.getElementById(chartId).getContext("2d");
    charts[chartId] = new Chart(ctx, config);

}


function updateLineChart(chartId, newLabels, newData, config = null) {
    const chart = charts[chartId];
    if (chart) {
        chart.data.labels = newLabels;
        chart.data.datasets[0].data = newData;
        chart.update(); // Update the chart with new data

    }

    else {
        renderChart(chartId, config);
    }
}



// reset chart
function resetLineChart(chartId, config) {
    const chart = charts[chartId];
    if (chart) {
        chart.destroy();

        renderChart(chartId, config);
    }
}
