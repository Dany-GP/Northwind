
const spinner = document.getElementById("spinner");
var myModal = new bootstrap.Modal(document.getElementById('modal'), {
    keyboard: false
  })


function generarReporte() {
    spinner.removeAttribute('hidden');

    const val = document.getElementById('inputAnio').value;
    if (val != null) {
        try {
            fetch("https://localhost:7153/api/Products/salesbyear?year=" + val)
                .then(response => {
                    if (!response.ok) throw Error(response.status);
                    return response.json();
                })
                .then(data => {
                    spinner.setAttribute('hidden', '');
                    console.log(data);
                    // Load the Visualization API and the corechart package.
                    google.charts.load('current', { 'packages': ['corechart', 'controls', 'table'] });

                    // Set a callback to run when the Google Visualization API is loaded.
                    google.charts.setOnLoadCallback(() => { cargarChart(data) });

                    $(document).ready(function () {
                        $('#table_id').DataTable({
                            //dom: 'Bfrtip',
                            destroy: true,
                            "ordering": false,
                            buttons: [
                                'copy', 'csv', 'excel', 'pdf', 'print'
                            ],
                            data: data,
                            columns: [
                                //{ data: 'company' },
                                { data: 'fecha' },
                                { data: 'cantidad' },
                                { data: 'ventas' }
                            ],
                            "language": {
                                "url": "//cdn.datatables.net/plug-ins/1.11.5/i18n/es-ES.json"
                            },

                        });
                    });

                }).catch(() => {
                    spinner.setAttribute('hidden', '');
                    myModal.show();
                });
        }
        catch (Error) {
            setTimeout(function () { spinner.setAttribute('hidden', '') }, 5000)
        }
    }


}



function cargarChart(data) {
    var datos = new google.visualization.DataTable();
    //data.addColumn('string', 'Trimestre');
    // datos.addColumn('number','Id');
    datos.addColumn('string', 'Fecha');
    datos.addColumn('number', 'Ventas');
    data.forEach((row) => {
        console.log(typeof (row));
        datos.addRow([row.fecha, row.ventas]);
    });

    var dashboard = new google.visualization.Dashboard(
        document.getElementById('dashboard_div'));

    var donutRangeSlider = new google.visualization.ControlWrapper({
        'controlType': 'NumberRangeFilter',
        'containerId': 'filter_div',
        'options': {
            'title': 'Cantidad',
            'filterColumnLabel': 'Ventas',
            'color': '#e0440e'
        }
    });

    var LineChart = new google.visualization.ChartWrapper({
        'chartType': 'LineChart',
        'containerId': 'chart_div',
        'options': {
            'title': 'Cantidad de ventas por año',
            'width': 700,
            'height': 600,
            'negativeColor': 'black',
            'pieSliceText': 'value',
            'curveType': 'function',
            'legend': 'right',
            colors: ['#e0440e']
        }
    });


    dashboard.bind(donutRangeSlider, LineChart);

    //Draw the dashboard.
    dashboard.draw(datos);
}
