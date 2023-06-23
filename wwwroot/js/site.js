
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

//search movies by name and genre
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

//search serie by name and genre
function searchSeries() {
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


//show results in carousel
function showCarousel() {
    document.getElementById('carRe').style.display = 'flex';
}

//replace carousel with movies
function replaceHTML(data) {

    //if doesnt exists movie or genre
    if (data.length < 1) {
        var carousel = "<div id='carRe'>" +
            "<h3>NOT FOUND</h3>" +
            "</div>";
        $("#carRe").replaceWith(carousel);
    }

    //new carousel with movies
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

    //if doesnt exists serie or genre
    if (data.length < 1) {
        var carousel = "<div id='carRe'>" +
            "<h3>NOT FOUND</h3>" +
            "</div>";
        $("#carRe").replaceWith(carousel);
    }
    if (data.length > 1) { //new carousel with series
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

//inicialize carousel
function slickSlide(data) {
    $('#carRe').slick({
        slidesToShow: (data.length > 1) ? ((data.length < 5) ? 2 : 5) : 1, //validations of data.lenght
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



