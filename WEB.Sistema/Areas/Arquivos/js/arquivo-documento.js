function ArquivoDocumentoClass() {
    var idBoxLista = "#boxLoadListaDocumentos";
    var idBoxForm = "#boxLoadFormDocumentos";
    var idBoxListaModal = "#boxLoadListaModalDocumentos";
    var idBoxFormModal = "#boxLoadFormModalDocumentos";

    this.init = function () {

        this.carregarListaArquivoUpload();
    };

    //Carregamento via AJax do formulario para inserção de documentos
    this.carregarFormArquivoUpload = function () {
        var elemento = $(idBoxForm);
        var url = elemento.data("url");
        console.log(url);
        $.get(url, {},

            function (response) {
                console.log(response);
                $(idBoxForm).html(response);
                $(idBoxForm).find("#FileUpload").fileinput({ showCaption: true, allowedPreviewTypes: ['image'] });
            }
        );
    };

    //Carregamento da lista de documentos existentes
    this.carregarListaArquivoUpload = function () {
        var elemento = $(idBoxLista);

        if (!(elemento.length > 0)) {
            return;
        }

        var url = elemento.data("url");

        $.get(url, {},
            function(response) {
                elemento.html(response);

                DefaultSistema.iniciarLinksAcao($(idBoxLista));

                $(idBoxLista).find(".txtEditable").editable();

                ArquivoDocumento.ordenarLista();
            }
        );
    };

    //Carregamento via AJax do formulario para inserção de documentos
    this.carregarFormModalArquivoUpload = function () {
        var elemento = $(idBoxFormModal);
        var url = elemento.data("url");

        $.get(url, {},
            function (response) {
                $(idBoxFormModal).html(response);
                $(idBoxFormModal).find("#FileUpload").fileinput({ showCaption: true, allowedPreviewTypes: ['image'] });
            }
        );
    };

    //Carregamento da lista de documentos existentes
    this.carregarListaModalArquivoUpload = function () {
        var elemento = $(idBoxListaModal);
        var url = elemento.data("url");

        $.get(url, {},
            function (response) {
                elemento.html(response);

                DefaultSistema.iniciarLinksAcao($(idBoxListaModal));

                $(idBoxListaModal).find(".txtEditable").editable();

                ArquivoDocumento.ordenarLista();
            }
        );
    };

    //Ordenacao de imagens
    this.ordenarLista = function () {

        //var sortabledLista = $("#listaFotos").sortable({
        //    helper: fixWidthHelper
        //}).disableSelection();
    };

    //Upload de arquivos assincrono
    this.uploadArquivo = function (form) {

        var data = new FormData();

        var files = $(form).find("#FileUpload").get(0).files;
        var idReferencia = $(form).find("#idReferenciaEntidade").val();
        var entidade = $(form).find("#entidadeArquivo").val();
        var categoria = $(form).find("#categoriaArquivo").val();
        var legenda = $(form).find("#legenda").val();

        if (files.length > 0) {
            data.append("FileUpload", files[0]);
        } else {
            DefaultSistema.reiniciarBotao();
            jM.error("Informe o arquivo que deseja salvar.");
            return;
        }

        data.append("idReferenciaEntidade", idReferencia);
        data.append("entidade", entidade);
        data.append("categoria", categoria);
        data.append("legenda", legenda);

        var urlUpload = new String($("#baseUrlGeral").val()).concat("arquivos/arquivo/editar-documento");

        var ajaxRequest = $.ajax({
            type: "POST",
            url: urlUpload,
            contentType: false,
            processData: false,
            data: data,
            success: function (response) {

                DefaultSistema.reiniciarBotao();

                if (response.error === false) {
                    ArquivoDocumento.carregarFormArquivoUpload();
                    ArquivoDocumento.carregarListaArquivoUpload();

                    ArquivoDocumento.carregarFormModalArquivoUpload();
                    ArquivoDocumento.carregarListaModalArquivoUpload();
                }
            },
            error: function (error) {
                console.log(error);
                
            }
        });
    }

    this.showModal = function (urlContent) {

        $.get(urlContent, function (data) {

            var Modal = $(data).modal();

            $(Modal).on("shown.bs.modal", function (e) {
                $('input:text').setMask();
                $("[data-toggle=tooltip]").tooltip();
                ArquivoDocumento.carregarFormModalArquivoUpload();
                ArquivoDocumento.carregarListaModalArquivoUpload();
            });

            $(Modal).on("hidden.bs.modal", function (e) {
                $(this).remove();
            });
        });
    }
};

var ArquivoDocumento = new ArquivoDocumentoClass();

$(document).ready(function () {
    ArquivoDocumento.init();
});