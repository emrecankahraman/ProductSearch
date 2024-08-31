document.addEventListener('DOMContentLoaded', function () {
    var chartOneData = JSON.parse(chartOneDataJson);
    var chartOneLabels = JSON.parse(chartOneLabelsJson);

    var chartOne = document.getElementById('chartOne');
    var myChart = new Chart(chartOne, {
        type: 'bar',
        data: {
            labels: chartOneLabels,
            datasets: [{
                label: 'Quantity',
                data: chartOneData,
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                borderColor: 'rgba(75, 192, 192, 1)',
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true,
                    callback: function (value, index, values) {
                        return value !== null ? value : 'N/A';
                    }
                }
            }
        }
    });
});
   
