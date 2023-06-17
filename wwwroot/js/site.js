//send email
$("#btnSend").click(function sendEmail() {
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
document.addEventListener("DOMContentLoaded", function () {
    var btnSearch = document.getElementById("btnSearch");

    //search by name and genre
    function search() {
        const inputSearch = $("#inputSearch").val(); //string input
        $.ajax({
            url: "/Client/search",
            type: "get",
            data: { "inputSearch": inputSearch }, //parameters
            datatype: 'text',
            success: function (data) {
                var dataList = JSON.parse(data);

                //call the select of main view
                var selectElement = $('#mySelect');
                selectElement.empty();
                selectElement.append($('<option>', {
                    text: 'select'
                }));

                //traverse the list and add to options
                $.each(dataList, function (i, movie) {
                    selectElement.append($('<option>', {
                        value: movie.ID,
                        text: movie.TITLE
                    }));
                });
                replaceHTML(dataList);
                showCarousel();
                slickSlide();
            }
        });
    }

    //call two functions for button btnSearch
    btnSearch.addEventListener("click", function () {
        search();
    });
});

//show results in carousel
function showCarousel() {
    document.getElementById('carRe').style.display = 'flex';
}

function replaceHTML(data) {
    var carousel = "<div id = 'carRe'>";
    for (movie in data) {
        carousel += "<div class='movie'>" +
            "<div class='video-thumbnail'>" +
            "<a asp-controller='client' asp-action='detailsMovies' asp-route-TITLE='" + data[movie].TITLE + "'><img src=" + data[movie].IMG + " class='carouselImg'></a>" +
            "</div>" +
            "</div>";
    }
    carousel += "</div>" +
        "</div>";

    $("#carRe").replaceWith(carousel);;

}

$(document).ready(function () {
    slickSlide();  //Genera el carrousel
});

function slickSlide() {
    $('#carRe').slick({
        slidesToShow: 5,
        slidesToScroll: 5,
        responsive: [
            {
                breakpoint: 768,
                settings: {
                    arrows: false,
                    centerMode: true,
                    centerPadding: '40px',
                    slidesToShow: 1
                }
            },
            {
                breakpoint: 480,
                settings: {
                    arrows: false,
                    centerMode: true,
                    centerPadding: '40px',
                    slidesToShow: 1
                }
            }
        ]
    });
}

const body = document.querySelector('body');
const toggle = document.getElementById('toggle');
toggle.onclick = function () {
    toggle.classList.toggle('active');
    body.classList.toggle('active');

    const modo = body.classList.contains('active') ? 'oscuro' : 'claro';
    localStorage.setItem('modo', modo); //save mode in local storage
    $.ajax({
        url: "/SuperAdmin/saveMode",
        type: "post",
        data: { "mode": modo }, //parametros
        success: function () {
            alert('Saved mode');
        }
    });
}

$.ajax({
    url: "/client/getMode",
    type: "get",
    success: function (color) {
        if (color === 'claro') {
            body.classList.add('active');
        }
    }
});

//recovery mode
const saveMode = localStorage.getItem('modo');
if (saveMode === 'oscuro') {
    body.classList.add('active');
}

