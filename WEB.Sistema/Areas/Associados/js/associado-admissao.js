function AssociadoAdmissaoClass(){
    

    //Metodo de inicializacao dos plugins
    this.init = function () {

        $("#boxAssociadosParaAdmissao").slimScroll({
            height: 600
        });

    };
    
    // Abrir o modal de admissão
    this.abriModalAdmissao = function () {

        var postData = { 'idsAssociados': [] };

        $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
            postData["idsAssociados"].push($(this).val());
        });

        if (postData["idsAssociados"].length == 0) {
            jM.info("Selecione ao menos um associado.");
            return false;
        }

        var url = $("#baseUrlGeral").val() + "Associados/AssociadoAdmissaoAcao/modal-admitir-associados";

        $.post(url, postData, function (response) {

            var Modal = $(response).modal();

            $(Modal).on("shown.bs.modal", function (e) {

                DefaultSistema.reiniciarBotao();

                iniciarDatePicker();

                $('input:text').setMask();

                $("#boxAssociadosSelecionados").slimScroll({
                    height: 206
                });

                $("#boxAssociadosSelecionados").removeClass("hide");

            });

            $(Modal).on("hidden.bs.modal", function (e) {
                $(this).remove();
            });

        });

    }

	//Executado ao submeter formulario de admissao do associado
    this.onSuccessFormAdmissao = function (response) {

        DefaultSistema.reiniciarBotao();

        DefaultSistema.iniciarPluginsAposAjax($("#boxFormAdmissao"));

        $("#boxAssociadosSelecionados").slimScroll({
            height: 206
        });

        $("#boxAssociadosSelecionados").removeClass("hide");

        if (response.error == true) {
            jM.error(response.message);
            return;
        }

        if (response.error == false) {
            $("#boxFormAdmissao").addClass("carregando");
            location.reload();
        }

    };

};

var AssociadoAdmissao = new AssociadoAdmissaoClass();

$(document).ready(function(){
    AssociadoAdmissao.init();
});
