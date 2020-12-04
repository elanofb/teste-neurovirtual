function ObjGeoLocalizacao() {

    var campoLatitude = "#latitude";
    var campoLongitude = "#longitude";

    this.init = function () {
        GeoLocalizacao.carregarLocalizacao();
    };

    this.carregarLocalizacao = function () {
        if (navigator.geolocation) {
            navigator.geolocation.getCurrentPosition(function (p) {
                var url = $("#baseUrlGeral").val() + "home/gravar-localizacao";
                $.get(url, { latitude: p.coords.latitude, longitude: p.coords.longitude }, function () {

                });
            });
        }
    };

    this.exibirMapa = function (latitude, longitude) {

        var url = $("#baseUrlGeral").val() + "Localizacao/GeoLocalizacao/modal-localizacao";
        
        var success = function () {

            var mapaConfig = {
                center: { lat: latitude, lng: longitude },
                zoom: 15,
                mapTypeId: google.maps.MapTypeId.ROADMAP
            };

            var mapaModal = new google.maps.Map(document.getElementById("mapaGoogle"), mapaConfig);

            var marcador = new google.maps.Marker({
                position: { lat: latitude, lng: longitude },
                title: "Localização do registro da ocorrência"
            });

            marcador.setMap(mapaModal);
        }

        DefaultSistema.showModal(url, success);
    };

}

var GeoLocalizacao = new ObjGeoLocalizacao();
$(document).ready(function(){
    GeoLocalizacao.init();
});