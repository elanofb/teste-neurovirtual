var HomeClass = function () {

    this.init = function () {

        this.iniciarCarrosseis();

        $('a.prettyphoto').prettyPhoto();
    };

    //Iniciar Plugins da home do site
    this.iniciarCarrosseis = function () {
        $('#evento-carousel').carousel({ interval: 10000, pause: "hover" });
    }

};


var Home = new HomeClass();

$(document).ready(function () {
    Home.init();
});