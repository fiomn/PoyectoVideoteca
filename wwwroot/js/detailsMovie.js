
function addComent() {
    var comment = document.getElementById('commentTA').value;
    var score = document.getElementById('score').value;
    $.ajax({
        url: "/Client/userComment",
        type: "GET",
        data: { comment: comment, score: score }, //parametros
        success: function (response) {
            alert('Added comment');
        },
        error: function (error) {
            $(this).remove();
            alert(error);
        }
    });
}