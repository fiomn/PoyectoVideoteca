const carouselContainer = document.querySelector('.carousel-container'); // Movies Carousel
const movies = document.querySelectorAll('.movie');

const leftArrow = document.getElementById('left-arrow');
const rightArrow = document.getElementById('right-arrow');

// Event Listener For Right Arrow
rightArrow.addEventListener('click', () => {
    carouselContainer.scrollLeft += carouselContainer.offsetWidth;

    const activeIndicator = document.querySelector('.indicators .active');
    if (activeIndicator.nextSibling) {
        activeIndicator.nextElementSibling.classList.add('active');
        activeIndicator.classList.remove('active');
    }
});

// Event Listener For Left Arrow
leftArrow.addEventListener('click', () => {
    carouselContainer.scrollLeft -= carouselContainer.offsetWidth;

    const activeIndicator = document.querySelector('.indicators .active');
    if (activeIndicator.previousSibling) {
        activeIndicator.previousElementSibling.classList.add('active');
        activeIndicator.classList.remove('active');
    }
});

// Pagination
const numberOfPages = Math.ceil(movies.length / 5); // Get number of blocks for pagination
for (let i = 0; i < numberOfPages; i++) {
    const indicator = document.createElement('button');

    if (i === 0) {
        indicator.classList.add('active');
    }

    document.querySelector('.indicators').appendChild(indicator);
    indicator.addEventListener('click', (e) => {
        carouselContainer.scrollLeft = i * carouselContainer.offsetWidth;

        document.querySelector('.indicators .active').classList.remove('active');
        e.target.classList.add('active');
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
carouselContainer.addEventListener('mouseleave', () => {
    movies.forEach((movie) => movie.classList.remove('hover'));
});

// Video Section

// Video Test

const thumbnails = document.querySelectorAll('.video-thumbnail');

thumbnails.forEach((thumbnail) => {
    const videoOverlay = thumbnail.querySelector('.video-overlay');
    const videoPlayer = thumbnail.querySelector('iframe');

    let timeoutId;
    let timeoutFinal;

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
