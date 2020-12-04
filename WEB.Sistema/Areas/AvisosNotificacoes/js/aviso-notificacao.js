function ObjAvisoNotificacao() {

    var baseUrl;

    this.init = function () {

        this.baseUrl = $("#baseUrlGeral").val();

        this.iniciarEditorMensagem();

        // Verifica��es para carregamento da pagina
        this.verificarExibicaoBlocosEspecificos();

        this.showHideAbasNotificados();

        // Ativar eventos onChange
        this.onChangeRadioAssociados();

        this.onChangeFlagAssociados();

    };

    //Iniciar editor de conteudo
    this.iniciarEditorMensagem = function () {

        var urlUploadImagem = new String($("#baseUrlGeral").val()).concat("Arquivos/froalaupload/salvar-foto");
        var urlUploadFile = new String($("#baseUrlGeral").val()).concat("Arquivos/froalaupload/salvar-arquivo");

        $('#editor').froalaEditor({
            language: 'pt_br',
            imageUploadURL: urlUploadImagem,
            fileUploadURL: urlUploadFile,
            height: 300,
            toolbarButtons: ['bold', 'italic', 'underline', 'fontSize', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink', 'html'],
            toolbarButtonsXS: ['bold', 'italic', 'underline', 'fontSize', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink', 'html'],
            toolbarButtonsSM: ['bold', 'italic', 'underline', 'fontSize', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink', 'html'],
            toolbarButtonsMD: ['bold', 'italic', 'underline', 'fontSize', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink', 'html']

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

    this.showHideAbasNotificados = function () {

        if ($("input[name=\"flagAssociados\"]:checked").val() == "espec") {
            $(".tabAssociadoNotificado").show();
        }

    }

    this.onChangeFlagAssociados = function () {

        if ($("#flagTodosAssociados").val() == "False") {

            $(".boxFlagsAssociados").removeClass("hidden");

        } else {

            $("input[name=\"flagAssociados\"]").each(function () {
                $(this).removeAttr('checked');
            });

            $(".boxFlagsAssociados").addClass("hidden");

            $(".boxStatusAssociados").show();

            $("#boxAssociadoEspecifico").hide();

        }

    }

    this.verificarExibicaoBlocosEspecificos = function () {
        
        if ($("input[name=\"flagAssociados\"]:checked").val() == "espec") {
            
            // Exibir bloco associados espec�ficos
            $("#boxAssociadoEspecifico").show();

            // Esconder bloco e limpar campo de status de associado
            $("#flagStatusAssociados").val("");
            $(".boxStatusAssociados").hide();

            return;

        }
        
        $("#boxAssociadoEspecifico").hide();

    }

    this.onChangeRadioAssociados = function () {

        $("input[name=\"flagAssociados\"]").on("change", function () {

            if ($(this).val() == "espec") {

                $(".boxStatusAssociados").hide();

                $("#boxAssociadoEspecifico").show();

                $("#flagStatusAssociados").val("");

                return;
            }

            $(".boxStatusAssociados").show();

            $("#boxAssociadoEspecifico").hide();

        });

    };

    this.onChangeFlagMobile = function (elem) {

        var flagMobile = $(elem).val();
        
        if (flagMobile == "True") {

            $("#boxNotificacaoMobile").removeClass("hide");
            
            return;
        }

        $("#boxNotificacaoMobile").addClass("hide");
        
    };

    this.onChangeTemplate = function (elem) {

        var idTemplate = $(elem).val();
        
        if (idTemplate > 0) {

            $("#boxMensagemNotificacao").addClass("hide");
            
            return;
        }

        $("#boxMensagemNotificacao").removeClass("hide");
        
    };

    //
    this.recarregarContadorTarefas = function () {

        DefaultSistema.carregarConteudo($(".box-dinamico.tasks-menu"));

    }

    //
    this.excluirEnvio = function (id) {
        
        var url = this.baseUrl.concat("AvisosNotificacoes/AvisoNotificacaoOperacao/excluir-envio");

        var dados = { id: id };

        var funcYes = function() {
        
            $.post(url, dados, function (response) {

                if (response.error == false) {

                    jM.success(response.message);

                    DefaultSistema.carregarConteudo($("#boxAssociadosEnvio"));

                    return;
                }

                jM.success(response.error);

            });

        }

        jM.confirmation(Vocabulary.confirmDelete, funcYes, null);

    }
    
    this.preVisualizarTemplate = function (url) {
        
        var idTemplate = $("#ONotificacaoSistema_idTemplate").val();
        
        if (idTemplate > 0){
            DefaultSistema.showModal(url+"/"+idTemplate);
            return;
        }
        
        jM.error("Selecione o template para pré-visualizar");
        
    }

};

var AvisoNotificacao = new ObjAvisoNotificacao();
$(document).ready(function () {
    AvisoNotificacao.init();
});
