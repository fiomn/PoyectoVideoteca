$("#btnSend").click(function emailSend() {
    alert('Correo enviadoooooo');
    const email = $("#send").val();
    $.ajax({
        url: "/userAuthentication/sendEmail",
        type: "post",
        data: { "string": email}, //parameters
        success: function () {
            alert('Correo enviado');
        }
    });
});


//add profile image
const $fileInput = document.getElementById('image')
const $dragZone = document.getElementById('result-image')
const $img = document.getElementById('img-result')
$dragZone.addEventListener('click', () => $fileInput.click())

$dragZone.addEventListener('dragover', (e) => {
    e.preventDefault()
    $dragZone.classList.add('form-file_result--active')
})

$dragZone.addEventListener('dragleave', (e) => {
    e.preventDefault()

    $dragZone.classList.remove('form-file_result--active')
})

const uploadImage = (file) => {
    const fileReader = new FileReader()
    fileReader.readAsDataURL(file)

    fileReader.addEventListener('load', (e) => {
        $img.setAttribute('src', e.target.result)
    })
}

$dragZone.addEventListener('drop', (e) => {
    e.preventDefault()

    $fileInput.files = e.dataTransfer.files //asignar la imagen al input de tipo files
    const file = $fileInput.files[0]
    
    uploadImage(file)
})

$fileInput.addEventListener('change', (e) => {
    const file = e.target.files[0]
    uploadImage(file)
})
