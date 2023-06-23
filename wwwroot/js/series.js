$("#btnSeason").click(function createSeason() {
    const TITLE = $("#inputTitle").val();
    alert('entrooooooo');
    $.ajax({
        url: "/admin/createSeason",
        type: "post",
        data: { "TITLE": TITLE }, //parameters
        success: function () {
        }
    });
});

//$("#btnEpisode").click(function createSe() {
//    const TITLE = $("#inputTitle").val();
//    $.ajax({
//        url: "/Admin/createSeason",
//        type: "post",
//        data: { "TITLE": TITLE }, //parameters
//        success: function () {
//        }
//    });
//});