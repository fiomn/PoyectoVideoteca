//Get the modal
var modal = document.getElementById("modal1");

//Get the button that opens the modal
var btn = document.getElementById("pop-up-button");

//Get span element that closes modal
var span = document.getElementsByClassName("close-pop-up")[0];

//When user clicks button, modal opens
btn.onclick = function () {
    modal.style.display = "block";
}

//When user clicks span, close the modal
span.onclick = function () {
    modal.style.display = "none";
}

window.onclick = function (event) {
    if (event.target == modal) {
        modal.style.display = "none";
    }
}

