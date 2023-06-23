function change() {

    //references
    const body = document.querySelector('body');
    const toggle = document.getElementById('toggle');

    //if is light mode
    if (toggle.classList.contains('active')) {
        toggle.classList.remove('active'); //dark mode
        body.classList.remove('active'); //dark mode
    } else { //dark mode
        toggle.classList.toggle('active'); //light mode
        body.classList.toggle('active'); //light mode
    }

    //states
    const modo = body.classList.contains('active') ? 'claro' : 'oscuro';
    const modeBtn = toggle.classList.contains('active') ? 'true' : 'false';

    //save color and button state
    $.ajax({
        url: "/SuperAdmin/saveMode",
        type: "post",
        data: { mode: modo, modeBtn: modeBtn }, //parameters
        success: function () {
        }
    });
}