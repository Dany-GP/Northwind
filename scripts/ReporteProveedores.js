var table;

        fetch("https://localhost:7153/api/Employees/CampaniasDatos")
        .then(response => response.json())
        .then(data => {
            console.log(data);
            // Load the Visualization API and the corechart package.
            google.charts.load('current', { 'packages': ['corechart', 'controls', 'table'] });

            // Set a callback to run when the Google Visualization API is loaded.
            google.charts.setOnLoadCallback(() => { cargarChart(data) });

            table = $(document).ready(function () {
                $('#table_id').DataTable({
                    //dom: 'Bfrtip',
                    destroy: true,
                    buttons: [
                        'copy', 'csv', 'excel', 'pdf', 'print'
                    ],
                    data: data,

                    columns: [
                        { data: 'nombre' },
                        { data: 'productosVendidos' },
                        { data: 'ventaXProveedor' }
                    ],
                    "language": {
                        "url": "//cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
                    },

                });
            });

        });
        
function cargarChart(data) {
    var datos = new google.visualization.DataTable();
    console.log(data);
    //data.addColumn('string', 'Trimestre');
    // datos.addColumn('number','Id');
    datos.addColumn('string', 'nombre');
    datos.addColumn('number', 'productosVendidos');
    //datos.addColumn('number', 'VentaXProveedor');
    data.forEach((row) => {
        console.log(typeof (row));
        datos.addRow([row.nombre, row.productosVendidos]);
    });

    var dashboard = new google.visualization.Dashboard(
        document.getElementById('dashboard_div'));

    var donutRangeSlider = new google.visualization.ControlWrapper({
        'controlType': 'NumberRangeFilter',
        'containerId': 'filter_div',
        'options': {
            'title': 'nombre',
            'filterColumnLabel': 'productosVendidos'
        }
    });

    var LineChart = new google.visualization.ChartWrapper({
        'chartType': 'LineChart',
        'containerId': 'chart_div',
        'options': {
            'title': 'Ventas de productos y por porveedor',
            'width': 700,
            'height': 600,
            'negativeColor': 'black',
            'pieSliceText': 'value',
            'curveType': 'function',
            'legend': 'right'
        }
    });


    dashboard.bind(donutRangeSlider, LineChart);

    //Draw the dashboard.
    dashboard.draw(datos);
    dashboard.draw(datos);
}
