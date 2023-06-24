// Obtener las referencias a los elementos del DOM
const carouselContainers = document.querySelectorAll('.carousel-container');
const leftArrows = document.querySelectorAll('.left-arrow');
const rightArrows = document.querySelectorAll('.right-arrow');
const indicators = document.querySelectorAll('.indicators');
const movies = document.querySelectorAll('.movie');

const thumbnails = document.querySelectorAll('.video-thumbnail');

thumbnails.forEach((thumbnail) => {
    const videoOverlay = thumbnail.querySelector('.video-overlay');
    const videoPlayer = thumbnail.querySelector('iframe');

    let timeoutId;


    //Corregir parte del video
    thumbnail.addEventListener('mouseover', () => {
        timeoutId = setTimeout(() => {
            videoOverlay.style.opacity = '1';
            videoPlayer.src = 'https://www.youtube.com/embed/LYS2O1nl9iM';
        }, 2000);
    });

    thumbnail.addEventListener('mouseout', () => {
        clearTimeout(timeoutId);
        timeoutFinal = setTimeout(() => {
            videoOverlay.style.opacity = '0';
            videoPlayer.src = '';
        }, 500);
    });
});

function redirectToView(selectElement) {
    var selectedValue = selectElement.value;
    if (selectedValue !== "") {
        window.location.href = selectedValue;
    }
}

// PRUEBA MENU RESPONSIVE

const menuToggle = document.querySelector('.menu-toggle');
const menu = document.querySelector('.menu');

menuToggle.addEventListener('click', function () {
    this.classList.toggle('active');
    menu.classList.toggle('active');
});

