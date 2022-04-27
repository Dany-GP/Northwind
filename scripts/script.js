function animarSignIn() {
    var config = {};
    

    config.duration = 2000;
    config.delay = 0;
    config.fill = 'forwards';

    

    var keyframes = [];

    var keyframes2 = [];

    var signInLogin = document.getElementById("signInLogin");
    var titleCon = document.getElementById("holaaa");
    var auxSignUp = document.getElementById("auxSignUp");
    var btn = document.getElementById("btnSign");
    var btnAbajo = document.getElementById("botonAbajo");

    var titleSign = document.getElementById("titleSign");
    var msg = document.getElementById("msg");
    var ocultos = document.getElementsByClassName("mostrar");
    var noOcultos = document.getElementsByClassName("noMostrar");

    if (btn.innerText === "Regístrate") {
        keyframes = [
            { 'opacity': '0' },

            { 'left': '10%' },
            { 'opacity': '1' },


            { 'left': '59%' }];


        keyframes2 = [
            { 'transform': 'scale(100%)' },

            { 'left': '37%' },

            { 'transform': 'scale(0%)' },

            { 'left': '9%' }];

        auxSignUp.animate(keyframes, config);
        signInLogin.animate(keyframes2, config);
        
        titleCon.innerText = "Regístrate";
        btn.innerText = "Inicia Sesión";
        btnAbajo.innerText = "Regístrate";
        titleSign.innerText = "Bienvenido de nuevo!";
        msg.innerText = "Para mantenerte conectado con nosotros por favor inicia sesión con tus datos personales";
        for (let i = 0; i < ocultos.length; i++) {
            ocultos[i].classList.remove("d-none");
        }
        noOcultos[0].classList.add("d-none");
    } else {
        keyframes = [
            { 'transform': 'scale(100%)' },

            { 'left': '9%' },
            { 'transform': 'scale(0%)' },


            { 'left': '37%' }];

        keyframes2 = [
            { 'opacity': '0' },

            { 'left': '59%' },
            { 'opacity': '1' },

            { 'left': '12%' }];

        auxSignUp.animate(keyframes2, config);
        signInLogin.animate(keyframes, config);
        titleCon.innerText = "Inicia Sesión en NorthWind"

        btn.innerText = "Regístrate";
        btnAbajo.innerText = "Inicia Sesión";
        titleSign.innerText = "Hola amigo!";
        msg.innerText = "Introduce tus credenciales para empezar a trabajar con nosotros";
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