// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

const options = {
    enableHighAccuracy: true,
    timeout: 5000,
    maximumAge: 0,
};

function error(err) {
    console.warn(`ERROR(${err.code}): ${err.message}`);
}

function Updatelocation() {
    getCurrentPosition(success, error, options)
}

function success(pos) {
    var crd = pos.coords;
    var long = crd.longitude;
    var lat = crd.latitude;
    var source="https://www.google.com/maps/embed/v1/view?key=AIzaSyBpSX02zBnR-ueioYxcK67SF3DIpyo4hWE&center=" + long + "," + lat +"&zoom=18"
    document.getElementById("map").src = source;
}

