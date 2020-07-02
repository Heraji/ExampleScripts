// The TMDb API script
var switcher = 0;

TMDb_News = function () {   // Function for finding the top 18 newest blockbuster films

    // API URL including API key and parameters
    let url = 'https://api.themoviedb.org/3/discover/movie?api_key=eba65380aec79a1812b9a1a0fa75debe&primary_release_year=2020'

    var num = 0; // Number used for indexing images/posters of film buttons
    fetch(url).then(result=>result.json()).then((data)=>{ // Fetch results as data from the URL (using JSON)

        // Change image source, title and text for all images/posters using data from URL/API
        for (i = 0; i < 18; i++) {
            num++ // Index count up
            document.getElementById('image' + num).src = "".concat("https://image.tmdb.org/t/p/w500", data.results[i].poster_path);
            document.getElementById('image' + num).title = data.results[i].title;
            document.getElementById('image' + num).alt = data.results[i].vote_average;
        }
    })

    // Make all film-poster buttons have an opacity of 1
    for (i = 0; i < 18; i++) {
        var imgPoster = document.getElementById('image' + (i+1));
        imgPoster.style.opacity = "1";
    };

}

TMDb_Popular = function () {    // Function for finding the top 18 most popular films

    // API URL including API key and parameters
    let url_pop = 'https://api.themoviedb.org/3/discover/movie?api_key=eba65380aec79a1812b9a1a0fa75debe&sort_by=popularity.desc'

    var num = 0; // Number used for indexing images/posters of film buttons
    fetch(url_pop).then(result=>result.json()).then((data)=>{   // Fetch results as data from the URL (using JSON)

        // Change image source, title and text for all images/posters using data from URL/API
        for (i = 0; i < 18; i++) {
            num++ // Index count up
            document.getElementById('image' + num).src = "".concat("https://image.tmdb.org/t/p/w500", data.results[i].poster_path);
            document.getElementById('image' + num).title = data.results[i].title;
            document.getElementById('image' + num).alt = data.results[i].vote_average;
        }
    })

    // Make all film-poster buttons have an opacity of 1
    for (i = 0; i < 18; i++) {
        var imgPoster = document.getElementById('image' + (i+1));
        imgPoster.style.opacity = "1";
    };

}

TMDb_Best = function () { // Function for finding the top 18 best selling films

    // API URL including API key and parameters
    let url_pop = 'https://api.themoviedb.org/3/discover/movie?api_key=eba65380aec79a1812b9a1a0fa75debe&sort_by=revenue.desc'

    var num = 0; // Number used for indexing images/posters of film buttons
    fetch(url_pop).then(result=>result.json()).then((data)=>{ // Fetch results as data from the URL (using JSON)

        // Change image source, title and text for all images/posters using data from URL/API
        for (i = 0; i < 18; i++) {
            num++   // Index count up
            document.getElementById('image' + num).src = "".concat("https://image.tmdb.org/t/p/w500", data.results[i].poster_path);
            document.getElementById('image' + num).title = data.results[i].title;
            document.getElementById('image' + num).alt = data.results[i].vote_average;
        }
    })

    // Make all film-poster buttons have an opacity of 1
    for (i = 0; i < 18; i++) {
        var imgPoster = document.getElementById('image' + (i+1));
        imgPoster.style.opacity = "1";
    };

}

document.addEventListener('DOMContentLoaded', TMDb_News);   // Run function on script load