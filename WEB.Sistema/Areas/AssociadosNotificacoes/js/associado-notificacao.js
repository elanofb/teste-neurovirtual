function AssociadoNotificacaoClass(){
    
    var form = ".formFiltro";

    //Metodo de inicializacao dos plugins
    this.init = function () {
        
    };
    
    // Envio de notificação para associados selecionados
    this.enviarNotificacaoSelecionados = function () {

        var postData = { 'idsAssociados': [] };

        $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
            postData["idsAssociados"].push($(this).val());
        });

        if (postData["idsAssociados"].length == 0) {
            jM.info("Selecione ao menos um associado.");
            return false;
        }

        var url = $("#baseUrlGeral").val() + "AssociadosNotificacoes/AssociadoNotificacao/modal-notificar-associados";

        $.post(url, postData, function (response) {

            AssociadoNotificacao.onSuccessAberturaModal(response)

        });

    }

    // Envio de notificação para todos os associados (da consulta)
    this.enviarNotificacaoTodos = function () {

        var postData = $(".formFiltro").serialize();

        var url = $("#baseUrlGeral").val() + "AssociadosNotificacoes/AssociadoNotificacao/modal-notificar-associados";

        $.post(url, postData, function (response) {

            AssociadoNotificacao.onSuccessAberturaModal(response)

        });

    }

    //
    this.onSuccessAberturaModal = function (response) {

        if (response.error == true) {

            jM.error(response.message);

            return;

        }

        var Modal = $(response).modal();

        $(Modal).on("shown.bs.modal", function (e) {

            DefaultSistema.reiniciarBotao();

            iniciarDatePicker();

            $('input:text').setMask();

            AssociadoNotificacao.iniciarEditorMensagem();

            $("#boxAssociadosSelecionados").slimScroll({
                height: 393
            });

            $("#boxAssociadosSelecionados").removeClass("hide");

        });

        $(Modal).on("hidden.bs.modal", function (e) {
            $(this).remove();
        });

    }

    //
    this.iniciarEditorMensagem = function () {

        var urlUploadImagem = new String($("#baseUrlGeral").val()).concat("Arquivos/froalaupload/salvar-foto");
        var urlUploadFile = new String($("#baseUrlGeral").val()).concat("Arquivos/froalaupload/salvar-arquivo");

        $('#editor').froalaEditor({
            language: 'pt_br',
            imageUploadURL: urlUploadImagem,
            fileUploadURL: urlUploadFile,
            height: 300,
            toolbarButtons: ['bold', 'italic', 'underline', 'fontSize', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink'],
            toolbarButtonsXS: ['bold', 'italic', 'underline', 'fontSize', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink'],
            toolbarButtonsSM: ['bold', 'italic', 'underline', 'fontSize', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink'],
            toolbarButtonsMD: ['bold', 'italic', 'underline', 'fontSize', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink']

        })
        .on('froalaEditor.file.beforeUpload', function (e, editor, files) {
        })
        .on('froalaEditor.file.uploaded', function (e, editor, response) {
        })
        .on('froalaEditor.file.inserted', function (e, editor, $file, response) {
        })
        .on('froalaEditor.file.error', function (e, editor, error, response) {
            if (error.code === 6) {
                jM.error("O arquivo informado &eacute; inv&aacute;lido!");
            }
        })
        .on('froalaEditor.image.error', function (e, editor, error) {
            if (error.code === 6) {
                jM.error("O arquivo informado &eacute; inv&aacute;lido!");
            }
        });

    }

	//Executado ao submeter formulario de admissao do associado
    this.onSuccessFormNotificacao = function (response) {

        DefaultSistema.reiniciarBotao();

        DefaultSistema.iniciarPluginsAposAjax($("#boxFormNotificacao"));

        AssociadoNotificacao.iniciarEditorMensagem();

        $("#boxAssociadosSelecionados").slimScroll({
            height: 393
        });

        $("#boxAssociadosSelecionados").removeClass("hide");

        if (response.error == true) {

            DefaultSistema.removerModais();

            jM.error(response.message);

            return;

        }

        if (response.error == false) {
            $("#boxFormNotificacao").addClass("carregando");
            location.reload();
        }

    };

};

var AssociadoNotificacao = new AssociadoNotificacaoClass();

$(document).ready(function(){
    AssociadoNotificacao.init();
});
