// Obtener las referencias a los elementos del DOM
const carouselContainers = document.querySelectorAll('.carousel-container');
const leftArrows = document.querySelectorAll('.left-arrow');
const rightArrows = document.querySelectorAll('.right-arrow');
const indicators = document.querySelectorAll('.indicators');
const movies = document.querySelectorAll('.movie');

// Recorrer los carousels y asignar los event listeners a los botones
carouselContainers.forEach((carouselContainer, index) => {
    const movies = carouselContainer.querySelectorAll('.movie');
    const numberOfPages = Math.ceil(movies.length / 5);

    let currentPage = 0;

    // Crear los indicadores de paginación
    for (let i = 0; i < numberOfPages; i++) {
        const indicator = document.createElement('button');

        if (i === 0) {
            indicator.classList.add('active');
        }

        indicators[index].appendChild(indicator);

        indicator.addEventListener('click', (e) => {
            currentPage = i;
            updateCarouselPosition(carouselContainer, currentPage);
            updateIndicators(indicators[index], currentPage);
        });
    }

    // Event Listener para el botón de la flecha izquierda
    leftArrows[index].addEventListener('click', () => {
        if (currentPage > 0) {
            currentPage--;
            updateCarouselPosition(carouselContainer, currentPage);
            updateIndicators(indicators[index], currentPage);
        }
    });

    // Event Listener para el botón de la flecha derecha
    rightArrows[index].addEventListener('click', () => {
        if (currentPage < numberOfPages - 1) {
            currentPage++;
            updateCarouselPosition(carouselContainer, currentPage);
            updateIndicators(indicators[index], currentPage);
        }
    });
});

// Update carousel position
function updateCarouselPosition(carouselContainer, currentPage) {
    const carouselWidth = carouselContainer.offsetWidth;
    carouselContainer.scrollLeft = currentPage * carouselWidth;
}

//Update Pagination Indicators
function updateIndicators(indicatorsContainer, currentPage) {
    const indicators = indicatorsContainer.querySelectorAll('button');
    indicators.forEach((indicator, index) => {
        if (index === currentPage) {
            indicator.classList.add('active');
        } else {
            indicator.classList.remove('active');
        }
    });
}

// Image Hover
movies.forEach((movie) => {
    movie.addEventListener('mouseenter', (e) => {
        const element = e.currentTarget;
        setTimeout(() => {
            movies.forEach((movie) => movie.classList.remove('hover'));
            element.classList.add('hover');
        }, 300);
    });
});

// Remove hover class from all images when leaving the carousel
//carouselContainer.addEventListener('mouseleave', () => {
//    movies.forEach((movie) => movie.classList.remove('hover'));
//});

// Video Section

// Video Test

const thumbnails = document.querySelectorAll('.video-thumbnail');

thumbnails.forEach((thumbnail) => {
    const videoOverlay = thumbnail.querySelector('.video-overlay');
    const videoPlayer = thumbnail.querySelector('iframe');

    let timeoutId;
    let timeoutFinal;


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


