function AssociadoDependenteCadastroModalClass() {

    //Metodo de inicializacao dos plugins
    this.init = function () {};

    this.autoCompleteAssociadoEstipulante = function () {

        $("#nomeAssociadoEstipulante").autocomplete({
            appendTo: "#boxModelAdicionarDependente",
            source: $("#baseUrlGeral").val() + "AssociadosConsultas/AssociadoConsulta/busca-auto-complete/",
            minLength: 2,
            select: function (event, ui) {
                $('#idAssociadoEstipulante').val(ui.item.id);
            }
        });
    }

    //Evento Pos-Submit do cadastro de associado
    this.onSuccessForm = function (response) {

        if (response.error == false) {
            if ($("#boxLoadListaDependentes").length) {
                DefaultSistema.carregarConteudo($("#boxLoadListaDependentes"));
                DefaultSistema.removerModais();
            } else {
                location.href = (response.urlRedirecionamento);
            }
            return;
        }

        $("#tab-dados-cadastrais input").setMask();

        $("#tab-dados-cadastrais").find(".link-loading").on('click', function () {
            var btn = $(this).button().data('loading-text', 'Processando...');
            btn.button('loading');
        });

        DefaultSistema.zerarErros();
    };


    this.modalCadastrarDependente = function (element) {

        var url = $(element).data("url");
        $.get(url, {}, function (response) {
            if (response.error != 'undefined' && response.error == true) {
                jM.error(response.message);
                return false;
            }

            var Modal = $(response).modal();
            $(Modal).on("shown.bs.modal", function (e) {
                AssociadoDependenteCadastroModal.atualizarBoxDadosCadastraisTipoAssociado($("input[name*='idTipoAssociado'"));
                AssociadoDependenteCadastroModal.autoCompleteAssociadoEstipulante();
                DefaultSistema.iniciarPluginsAposAjax($("#tab-dados-cadastrais-dependente"));
            });

            $(Modal).on("hidden.bs.modal", function (e) {
                
            });
        });
    };

    this.atualizarBoxDadosCadastraisTipoAssociado = function (element) {

        var idTipoAssociado = $(element).val();

        var box = $("#tab-dados-cadastrais-dependente");

        if (box.length > 0) {

            var idEstipulante = $("input[name*='idAssociadoEstipulante']").val();
            var url = $("#baseUrlGeral").val() + "AssociadosDependentes/AssociadoDependenteCadastro/modal-cadastrar-dependente?idAssociadoEstipulante=" + idEstipulante;

            box.loadingOverlay();

            $.get(url, { id: 0, idTipoAssociado: idTipoAssociado }, function (response) {
                
                box.html($(response).find("#tab-dados-cadastrais-dependente"));
                DefaultSistema.iniciarPluginsAposAjax(box);
                box.loadingOverlay('remove');
            });
            return false;
        }
        return true;
    }
};

var AssociadoDependenteCadastroModal = new AssociadoDependenteCadastroModalClass();
$(document).ready(function () {
    AssociadoDependenteCadastroModal.init();
});
