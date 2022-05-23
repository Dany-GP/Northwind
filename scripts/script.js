function animarSignIn() {
    var config = {};
    config.duration = 500;
    config.delay = 0;
    config.fill = 'forwards';

    var keyframes = [];

    var keyframes2 = [];

    var signInLogin = document.getElementById("signInLogin");
    var auxSignUp = document.getElementById("auxSignUp");
    var btn = document.getElementById("btnSign");
    var titleSign = document.getElementById("titleSign");
    var msg = document.getElementById("msg");
    var ocultos = document.getElementsByClassName("mostrar");
    var noOcultos = document.getElementsByClassName("noMostrar");

    if (btn.innerText === "Sign Up") {
        keyframes = [
            { 'left': '10%' },
            { 'left': '59%' }];

        keyframes2 = [
            { 'left': '37%' },
            { 'left': '9%' }];

        auxSignUp.animate(keyframes, config);
        signInLogin.animate(keyframes2, config);

        btn.innerText = "Sign In";
        titleSign.innerText = "Welcome Back!";
        msg.innerText = "To keep connected with us please login with your personal details";
        for (let i = 0; i < ocultos.length; i++) {
            ocultos[i].classList.remove("d-none");
        }
        noOcultos[0].classList.add("d-none");
    } else {
        keyframes = [
            { 'left': '9%' },
            { 'left': '37%' }];

        keyframes2 = [
            { 'left': '59%' },
            { 'left': '12%' }];

        auxSignUp.animate(keyframes2, config);
        signInLogin.animate(keyframes, config);

        btn.innerText = "Sign Up";
        titleSign.innerText = "Hello friend!";
        msg.innerText = "Enter your personal details to and start journey with us";
        for (let i = 0; i < ocultos.length; i++) {
            ocultos[i].classList.add("d-none");
        }
        noOcultos[0].classList.remove("d-none");
    }


}

function animarSignUp() {
    var config = {};
    config.duration = 2000;
    config.delay = 500;
    config.fill = 'forwards';

    var signInLogin = document.getElementById("signInLogin");
    var signUpLogin = document.getElementById("signUpLogin");
    var auxSignIn = document.getElementById("auxSignIn");
    var auxSignUp = document.getElementById("auxSignUp");


}
fetch("https://localhost:7153/api/Products/salesbot?startDate=1996-05-05&endDate=1998-05-05")
.then(response => response.json())
.then(data => {
    console.log(data);
    // Load the Visualization API and the corechart package.
    google.charts.load('current', { 'packages': ['corechart', 'controls', 'table'] });

    // Set a callback to run when the Google Visualization API is loaded.
    google.charts.setOnLoadCallback(() => { cargarChart(data) });

    $(document).ready( function () {
        $('#table_id').DataTable({
            data: data,
            columns:[                
                {data: 'proveedor'},
                {data: 'ventas'}
            ]
        });
    } );

});



function cargarChart(data) {
var datos = new google.visualization.DataTable();
//data.addColumn('string', 'Trimestre');
// datos.addColumn('number','Id');
datos.addColumn('number', 'Proveedor');
datos.addColumn('number', 'Ventas');
data.forEach((row) => {
    console.log(typeof (row));
    datos.addRow([row.proveedor, row.ventas]);
});

var dashboard = new google.visualization.Dashboard(
    document.getElementById('dashboard_div'));

var donutRangeSlider = new google.visualization.ControlWrapper({
    'controlType': 'NumberRangeFilter',
    'containerId': 'filter_div',
    'options': {
        'title':'Cantidad',
        'filterColumnLabel': 'Cantidad'
    }
});

var LineChart = new google.visualization.ChartWrapper({
    'chartType': 'LineChart',
    'containerId': 'chart_div',
    'options': {
        'title': 'Cantidad de ventas por año',
        'width': 900,
        'height': 600,
        'negativeColor': 'black',
        'pieSliceText': 'value',
        'curveType':'function',
        'legend': 'right'
    }
});


dashboard.bind(donutRangeSlider, LineChart);

//Draw the dashboard.
dashboard.draw(datos);
dashboard.draw(datos);
}
