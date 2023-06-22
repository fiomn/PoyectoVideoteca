function change() {
    const body = document.querySelector('body');
    const toggle = document.getElementById('toggle');

    if (toggle.classList.contains('active')) {
        toggle.classList.remove('active');
        body.classList.remove('active');
    } else {
        toggle.classList.toggle('active');
        body.classList.toggle('active');
    }

    const modo = body.classList.contains('active') ? 'claro' : 'oscuro';
    const modeBtn = toggle.classList.contains('active') ? 'true' : 'false';

    $.ajax({
        url: "/SuperAdmin/saveMode",
        type: "post",
        data: { mode: modo, modeBtn: modeBtn }, //parameters
        success: function () {
        }
    });
}