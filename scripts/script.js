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
            { 'left': '13%' },
            { 'left': '59%' }];

        keyframes2 = [
            { 'left': '37%' },
            { 'left': '13%' }];

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
            { 'left': '13%' },
            { 'left': '37%' }];

        keyframes2 = [
            { 'left': '59%' },
            { 'left': '13%' }];

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