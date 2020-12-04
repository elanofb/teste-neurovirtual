function ObjDiretoriaMembro() {

    this.init = function () {

    };

    //Abrir modal para cadastrar uma novo tipo
    this.modalMembro = function (elemento) {

        var url = $(elemento).data("url");

        $.get(url, {}, function (response) {

            var Modal = $(response).modal({ title: 'Cadastrar um Membro da Diretoria' });

            $(Modal).on("shown.bs.modal", function (e) {
                DefaultSistema.iniciarPluginsAposAjax();
                Diretoria.iniciarEditor();
                Diretoria.carregarListaArquivos();
            });

            $(Modal).on("hidden.bs.modal", function (e) {
                $(this).remove();
            });

        });
    }

    this.enviarFormDiretoriaMembro = function () {
        var form = $('#formDiretoriaMembro');

        var formdata = new FormData(form[0]);

        var boxForm = $("#boxFormDiretoriaMembro");
        var url = $(form).attr('action');
        $.ajax({
            url: url, type: 'POST', data: formdata, async: false,
            success: function (data) {
                DefaultSistema.carregarConteudo("#boxLoadListaMembros");
                DefaultSistema.removerModais();
                if (data.error) {
                    alert('Voc&ecirc; n&atilde;o pode alterar/inserir dados para essa diretoria!');
                }
            },
            cache: false, contentType: false, processData: false
        });
    }

    this.delete = function (elemento) {

        var url = $(elemento).data("url");

        var func = function () {
            DefaultSistema.carregarConteudo("#boxLoadListaMembros");
        };

        Ajax.init(url, {}, func, Vocabulary.confirmDelete);

    }


};


var DiretoriaMembro = new ObjDiretoriaMembro();
var Diretoria = new ObjDiretoria();

$(document).ready(function () {
    DiretoriaMembro.init();
});