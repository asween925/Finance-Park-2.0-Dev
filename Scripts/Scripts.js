//JavaScript Document

//          USE CTRL+M+H TO HIDE HIGHLIGHTED TEXT


//--------------MAIN FUNCTIONS--------------

function refresh() {
    location.reload();
}

function refresh3Secs() {
    setTimeout(refresh(), 3000);
}

function print() {
    window.print();
}

function toggle() {
    //Blur the div with the blur id
    var blur = document.getElementById('blur');
    blur.classList.toggle('active');

    //Toggle the Sim Research Popup
    var popup = document.getElementById('popup');
    popup.classList.toggle('active');

}

function blurscreen() {
    //Blur the div with the blur id
    var blur = document.getElementById('blur');
    blur.classList.toggle('active');
}

