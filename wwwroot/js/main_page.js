const fila = document.querySelector('.contenedor-carousel'); //Movies Carousel
const peliculas = document.querySelectorAll('.pelicula');

const flechaIzquierda = document.getElementById('flecha-izquierda');
const flechaDerecha = document.getElementById('flecha-derecha');

//Event Listener For Right Angle
flechaDerecha.addEventListener('click', () => {
    fila.scrollLeft += fila.offsetWidth;

    const indicadorActivo = document.querySelector('.indicadores .activo');
    if (indicadorActivo.nextSibling) {
        indicadorActivo.nextElementSibling.classList.add('activo');
        indicadorActivo.classList.remove('activo');
    }
})

//Event Listener For Left Angle
flechaIzquierda.addEventListener('click', () => {
    fila.scrollLeft -= fila.offsetWidth;

    const indicadorActivo = document.querySelector('.indicadores .activo');
    if (indicadorActivo.previousSibling) {
        indicadorActivo.previousElementSibling.classList.add('activo');
        indicadorActivo.classList.remove('activo');
    }
})

//Pagination

//Math.ceil = round up
const numeroPaginas = Math.ceil(peliculas.length / 5);  //Get number blocks for pagination 
for (let i = 0; i < numeroPaginas; i++) {
    const indicador = document.createElement('button');

    if (i == 0) {
        indicador.classList.add('activo');
    }

    document.querySelector('.indicadores').appendChild(indicador);
    indicador.addEventListener('click', (e) => {

        fila.scrollLeft = i * fila.offsetWidth;

        document.querySelector('.indicadores .activo').classList.remove('activo');
        e.target.classList.add('activo');
    });
}

//Image Hover
peliculas.forEach((pelicula) => {
    pelicula.addEventListener('mouseenter', (e) => {
        const elemento = e.currentTarget;
        setTimeout(() => {
            peliculas.forEach(pelicula => pelicula.classList.remove('hover'));
            elemento.classList.add('hover');
        }, 300);
    });
});

//When you get out of the carousel, all images hover are removed
fila.addEventListener('mouseleave', () => {
    peliculas.forEach(pelicula => pelicula.classList.remove('hover'));
});