function ConfiguracaoRedesSociaisClass() {

    this.init = function () {

        this.onChangeCompartilharFacebook();

    }

    //
    this.onChangeCompartilharFacebook = function () {

        var value = $("#flagCompartilharFacebook").val();

        if (value == "True") {
            $("#compartilhar-facebook").show();
        } else {
            $("#compartilhar-facebook").hide();
        }

    };

}

var ConfiguracaoRedesSociais = new ConfiguracaoRedesSociaisClass();

$(document).ready(function () {

    ConfiguracaoRedesSociais.init();

});