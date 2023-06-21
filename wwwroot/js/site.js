
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
                slickSlide(dataList);
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
                replaceHTMLSERIES(dataList);
                showCarousel();
                slickSlide(dataList);
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

//replace carousel with movies
function replaceHTML(data) {
    if (data.length < 1) {
        alert('No movie found');
    }
    if (data.length > 1) {
        var carousel = "<div id='carRe'>";
        for (movie in data) {
            carousel += "<div class='movie'>" +
                "<div class='video-thumbnail'>" +
                "<a href='/client/detailsMovies?TITLE=" + data[movie].TITLE + "'><img src=" + data[movie].IMG + " class='carouselImg'  style='width: 230px; height: 320px; margin-left:30px; margin-top:30px; margin-bottom:30px;'></a>" +
                "</div>" +
                "</div>";
        }
        carousel += "</div>" +
            "</div>";

        $("#carRe").replaceWith(carousel);

    } if (data.length === 1) {
        alert('solo una');
        alert(data[0].TITLE);
        var carousel = "<div id='carRe'>" +
            "<div class='movie'>" +
            "<div class='video-thumbnail'>" +
            "<a href='/client/detailsMovies?TITLE=" + data[0].TITLE + "'><img src=" + data[0].IMG + " style='width: 200px; height: 300px; margin-left:30px; margin-top:30px; margin-bottom:30px;'></a>" +
            "</div>" +
            "</div>" +
            "</div>";

        $("#carRe").replaceWith(carousel);
    }
}

//replace carousel with series
function replaceHTMLSERIES(data) {
    if (data.length < 1) {
        alert('No movie found');
    }
    if (data.length > 1) {
        var carousel = "<div id='carRe'>";
        for (serie in data) {
            carousel += "<div class='movie'>" +
                "<div class='video-thumbnail'>" +
                "<a href='/client/detailsSerie?TITLE=" + data[serie].TITLE + "'><img src=" + data[serie].IMG + " class='carouselImg'  style='width: 230px; height: 320px; margin-left:30px; margin-top:30px; margin-bottom:30px;'></a>" +
                "</div>" +
                "</div>";
        }
        carousel += "</div>" +
            "</div>";

        $("#carRe").replaceWith(carousel);

    } if (data.length === 1) {
        alert('solo una');
        alert(data[0].TITLE);
        var carousel = "<div id='carRe'>" +
            "<div class='movie'>" +
            "<div class='video-thumbnail'>" +
            "<a href='/client/detailsSerie?TITLE=" + data[0].TITLE + "'><img src=" + data[0].IMG + " style='width: 200px; height: 300px; margin-left:30px; margin-top:30px; margin-bottom:30px;'></a>" +
            "</div>" +
            "</div>" +
            "</div>";

        $("#carRe").replaceWith(carousel);
    }
}

$(document).ready(function () {
    slickSlide();  //generate carousel
    $.ajax({
        url: "/SuperAdmin/getMode",
        type: "get",
        data: { }, //parameters
        datatype: 'text',
        success: function (data) {
            const mode = data.modeBtn;
            if (mode === 'false') {
                const toggle = document.getElementById('toggle');
                toggle.classList.remove('active');
                const body = document.getElementById('body');
                body.classList.remove('active');
            } else {
                const toggle = document.getElementById('toggle');
                toggle.classList.toggle('active');
                const body = document.getElementById('body');
                body.classList.toggle('active');
            }
        }
    });
    

    
});

const body = document.querySelector('body');
const toggle = document.getElementById('toggle');
toggle.onclick = function () {
    
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

    //recargar la pagina
    //setTimeout(function () {
    //    window.location.href = "/SuperAdmin/SuperAdminMain";
    //}, 500);

}




function slickSlide(data) {
    alert(data.length);
    $('#carRe').slick({
        slidesToShow: (data.length > 1) ? ((data.length < 5) ? 2 : 5) : 1,
        slidesToScroll: 3,
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



