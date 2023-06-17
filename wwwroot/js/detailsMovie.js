
function addComent() {
    var comment = document.getElementById('commentTA').value;
    var score = document.getElementById('score').value;
    $.ajax({
        url: "/Client/userComment",
        type: "GET",
        data: { comment: comment, score: score }, //parametros
        success: function (response) {
            alert('Comment Added Succesfully');
            $("#commentTA").val("");
            $("#score").val("");
        },
        error: function (error) {
            $(this).remove();
            alert(error);
        }
    });


}