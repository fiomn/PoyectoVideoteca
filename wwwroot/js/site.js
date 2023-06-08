$("#btnSend").click(function sendEmail() {
    alert('esta en el ajax');
    const email = $("#inInput").val();
    $.ajax({
        url: "/userAuthentication/sendEmail",
        type: "post",
        data: { "str": email}, //parametros
        success: function () {
            alert('Correo enviado');
        }
    });
});

//function to have a preview image
function previewProfileImage(input) {
    if (input.files && input.files[0]) { //si se ha seleccionado al menos un archivo
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#profileImagePreview').attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}