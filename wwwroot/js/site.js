//send email
$("#btnSend").click(function sendEmail() {
    alert('esta en el ajax');
    const email = $("#inEmail").val();
    const username = $("#inUserName").val();
    $.ajax({
        url: "/userAuthentication/sendEmail",
        type: "post",
        data: { "email": email, "username": username }, //parametros
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

//search by name and genre
window.search = function () {
    alert('entro al ajax');
    const inputSearch = $("#inputSearch").val();
    $.ajax({
        url: "/Client/search",
        type: "get",
        data: { "inputSearch": inputSearch }, //parámetros
        datatype: 'json',
        success: function (data) {
            console.log(data);
            //var dataList = JSON.parse(data);
            //llama al select de html
            var dataList = data;
            var selectElement = $('#mySelect');
            selectElement.empty();
            selectElement.append($('<option>', {
                text: 'select'
            }));
            //recorre la lista de pokemones y los mete en un select
            $.each(dataList, function (i, op) {
                selectElement.append($('<option>', {
                    value: op.value,
                    text: dataList[i].TITLE
                }));
            });
        }
    });
}