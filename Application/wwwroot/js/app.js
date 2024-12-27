



let charts = {};

function renderChart(chartId, config) {
    if (charts[chartId]) {
        charts[chartId].destroy(); // Destroy existing chart instance
    }
    const ctx = document.getElementById(chartId).getContext("2d");
    charts[chartId] = new Chart(ctx, config);

}

function updateChart(chartId, newLabels, newData) {
    const chart = charts[chartId];
    if (chart) {
        chart.data.labels = newLabels;
        chart.data.datasets[0].data = newData;
        chart.update(); // Update the chart with new data

    }
}


// write a function that takes a parameter value and updates the elements
// the value is in div element
// the last updated is in span element

function updateRealTimeSalesKPI(valueId, lastUpdatedId, value, lastUpdated) {
    const valueElement = document.getElementById(valueId);
    const lastUpdatedElement = document.getElementById(lastUpdatedId);

    console.log(value);
    console.log(lastUpdated);

    if (valueElement) {
        valueElement.textContent = value;
    }
    if (lastUpdatedElement) {
        lastUpdatedElement.textContent = lastUpdated;
    }
}



// print the object in parameter
function printObject(obj) {
    console.log(obj);
}