function AssociadoDependenteCadastroClass() {


    //Metodo de inicializacao dos plugins
    this.init = function () {
        AssociadoDependenteCadastro.iniciarEditableTipoAssociado();
        AssociadoDependenteCadastro.autoCompleteAssociadoEstipulante();
    };

    this.iniciarEditableTipoAssociado = function () {
        var campoValor = $(".id-tipo-associado");
        campoValor.editable({ source: $("#sourceTipoAssociado").val() });
    };

    this.autoCompleteAssociadoEstipulante = function () {

        $("#nomeAssociadoEstipulante").autocomplete({
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
     
};

var AssociadoDependenteCadastro = new AssociadoDependenteCadastroClass();

$(document).ready(function () {

    AssociadoDependenteCadastro.init();

});
