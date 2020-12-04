function AssociadoCadastroClass(){
    

    //Metodo de inicializacao dos plugins
    this.init = function () {
        AssociadoCadastro.iniciarEditableTipoAssociado();
    };

    this.iniciarEditableTipoAssociado = function () {
        var campoValor = $(".id-tipo-associado");

        campoValor.editable({
            source: $("#sourceTipoAssociado").val(),
            success: function (response, newValue) {

                if (response.error == false) {
                    AssociadoCadastro.atualizarBoxDadosCadastraisTipoAssociado(newValue, true);
                } else {
                    return response.message;
                }
            }
        });
    };

    //Evento Pos-Submit do cadastro de associado
	this.onSuccessForm = function (response) {

	    if (response.error == false) {

	         location.href = (response.urlRedirecionamento);

            return;
        }

	    $("#tab-dados-cadastrais input").setMask();

	    $("#tab-dados-cadastrais").find(".link-loading").on('click', function () {

	        var btn = $(this).button().data('loading-text', 'Processando...');

	        btn.button('loading');

	    });

	    DefaultSistema.zerarErros();

	};

    this.atualizarDadosCadastraisTipoAssociado = function(element) {

        var atualizaPage = AssociadoCadastro.atualizarBoxDadosCadastraisTipoAssociado($(element).val());

        if (atualizaPage == false) { return null; }
        
        var search = window.location.search;
        if (search == "") {
            window.location.search += "?idTipoAssociado=" + $(element).val();
            return;
        }

        var param, params_arr = [];
        var queryString = search.substr(1)

        if (queryString !== "") {
            params_arr = queryString.split("&");
            for (var i = params_arr.length - 1; i >= 0; i -= 1) {
                param = params_arr[i].split("=")[0];
                if (param === "idTipoAssociado") {
                    params_arr.splice(i, 1);
                }
            }
        }
        window.location.search = params_arr.join("&") +  "&idTipoAssociado=" + $(element).val();
    }

    this.atualizarBoxDadosCadastraisTipoAssociado = function (idTipoAssociado, editable) {
        var box = $("#tab-dados-cadastrais");
        var idAssociado = $("#Associado_id").val()

        if (box.length > 0) {

            if (idAssociado > 0 && editable != true) { return false; }

            var controller = window.location.pathname.indexOf("AssociadoCadastroPF") != -1 ? "AssociadoCadastroPF" : "AssociadoCadastroPJ";

            var param, params_arr = [];
            var queryString = window.location.search.substr(1)
            if (queryString !== "") {
                params_arr = queryString.split("&");
                for (var i = params_arr.length - 1; i >= 0; i -= 1) {
                    param = params_arr[i].split("=")[0];
                    if (param === "idTipoAssociado") {
                        params_arr.splice(i, 1);
                    }
                }
            }

            var url = $("#baseUrlGeral").val() + "Associados/" + controller + "/aba-dados-cadastrais/" + params_arr.join("&");

            box.loadingOverlay();
            $.get(url, { id: idAssociado, idTipoAssociado: idTipoAssociado }, function (response) {
                box.html(response);
                DefaultSistema.iniciarPluginsAposAjax(box);
                box.loadingOverlay('remove');
            });
            return false;
        }
        return true;
    }
};

var AssociadoCadastro = new AssociadoCadastroClass();

$(document).ready(function(){

    AssociadoCadastro.init();

});
