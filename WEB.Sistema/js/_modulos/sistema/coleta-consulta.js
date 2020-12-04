function ObjColetaConsulta(){
    this.init = function () {
    };


    //
    this.retornoConsulta = function (response) {
        if (response.urlRedirect) {
            location.href = (response.urlRedirect);
            return false;
        }

        $("#boxResultadoConsulta").show();
        $(".link-loading").button("reset");
        DefaultSistema.iniciarPluginsAposAjax($("#boxResultadoConsulta"));
        iniciarDataTable();
        ShowColumas.init();
    }

    //
    this.inicioConsulta = function () {
        $("#boxResultadoConsulta").hide();
    }

}

var ColetaConsulta = new ObjColetaConsulta();
$(document).ready(function(){
    ColetaConsulta.init();
});
