
//send email
$("#btnSend").click(function sendEmail() {
    const email = $("#inEmail").val();
    const username = $("#inUserName").val();
    $.ajax({
        url: "/userAuthentication/sendEmail",
        type: "post",
        data: { "email": email, "username": username }, //parameters
        success: function () {
            alert('Correo enviado');
        }
    });
});

//function to have a preview image
function previewProfileImage(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

        reader.onload = function (e) {
            $('#profileImagePreview').attr('src', e.target.result);
        }
        reader.readAsDataURL(input.files[0]);
    }
}

//search movie by name and genre
document.addEventListener("DOMContentLoaded", function () {
    var btnSearch = document.getElementById("btnSearch");

    function search() {
        const inputSearch = $("#inputSearch").val(); //string input
        $.ajax({
            url: "/Client/search",
            type: "get",
            data: { "inputSearch": inputSearch }, //parameters
            datatype: 'text',
            success: function (data) {
                var dataList = JSON.parse(data);
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

//search serie by name and genre
document.addEventListener("DOMContentLoaded", function () {
    var btnSearch = document.getElementById("btnSerie");

    function search() {
        const busSeries = $("#inputSerie").val(); //string input
        $.ajax({
            url: "/Client/searchSeries",
            type: "get",
            data: { "busSeries": busSeries }, //parameters
            datatype: 'text',
            success: function (data) {
                var dataList = JSON.parse(data);
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
    if (data.length<1) {
        alert('No movie found');
    }
    if (data.length > 1) {
        var carousel = "<div id='carRe'>";
        for (movie in data) {
            carousel += "<div class='movie'>" +
                "<div class='video-thumbnail'>" +
                "<a asp-controller='client' asp-action='detailsMovies' asp-route-TITLE='" + data[movie].TITLE + "'><img src=" + data[movie].IMG + " class='carouselImg'  style='width: 230px; height: 300px; margin-left:30px; margin-top:30px; margin-bottom:30px;'></a>" +
                "</div>" +
                "</div>";
        }
        carousel += "</div>" +
            "</div>";

        $("#carRe").replaceWith(carousel);

    } if (data.length === 1) {
        alert('solo una');
        alert(data[0].TITLE);
        var carousel = "<div id='carRe'>"+
           "<div class='movie'>" +
                "<div class='video-thumbnail'>" +
                "<a asp-controller='client' asp-action='detailsMovies' asp-route-TITLE='" + data[0].TITLE + "'><img src=" + data[0].IMG + " style='width: 230px; height: 300px; margin-left:30px; margin-top:30px; margin-bottom:30px;'></a>" +
                "</div>" +
            "</div>" +
            "</div>";

        $("#carRe").replaceWith(carousel);
    }
}


$(document).ready(function () {
    slickSlide();  //generate carousel
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

    const modo = body.classList.contains('active') ? 'claro' : 'oscuro';
    $.ajax({
        url: "/SuperAdmin/saveMode",
        type: "post",
        data: { mode: modo }, //parameters
        success: function () {
        }
    });
    if (modo === 'claro') {
        document.getElementById('theme').href = 'lightMode.css';
    }
    else {
        document.getElementById('theme').href = 'main_page.css';
    }
}

