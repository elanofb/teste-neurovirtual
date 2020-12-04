function ArquivoFotoClass() {

    var baseUrl;

    var idBoxLista = "#boxLoadListaFotos";
    var idBoxForm = "#boxLoadFormFotos";

    this.init = function () {
        
        this.baseUrl = $("#baseUrlGeral").val();

        this.iniciarEditable();

        this.iniciarSortable();

        this.iniciarFancybox();

    };

    //Carregamento via AJax do formulario para inserção de Fotos
    this.carregarFormArquivoUpload = function () {

        var elemento = $(idBoxForm);

        var url = elemento.data("url");
        
        $.get(url, {},

            function (response) {
                
                $(idBoxForm).html(response);

                $(idBoxForm).find("#FileUpload").fileinput({ showCaption: true, allowedPreviewTypes: ['image'] });

                DefaultSistema.reiniciarBotao();

            }
        );

    };

    //Carregamento da lista de Fotos existentes
    this.carregarListaArquivoUpload = function () {
        var elemento = $(idBoxLista);

        if (!(elemento.length > 0)) {
            return;
        }
        
        var url = elemento.data("url");

        $.get(url, {},

            function (response) {

                elemento.html(response);

                DefaultSistema.iniciarLinksAcao($(idBoxLista));

                ArquivoFoto.iniciarEditable();
                
                ArquivoFoto.iniciarSortable();

                ArquivoFoto.iniciarFancybox();

            }
        );
    };
    
    this.iniciarSortable = function () {

        $(idBoxLista).sortable({

            items: ".itemListaFotos",

            stop: function (event, ui) {

                var idSelecionado = ui.item.data("id");

                var pos = 0;

                var itemsSortable = $(idBoxLista).find(".itemListaFotos");

                itemsSortable.each(function (index) {

                    var idEach = $(this).data("id");

                    if (idEach == idSelecionado) {

                        pos = index + 1;

                    }

                });
                
                var url = ArquivoFoto.baseUrl + "Arquivos/ArquivoFoto/alterar-ordem";

                $.post(url, { id: idSelecionado, pos: pos }, function (response) {

                    ArquivoFoto.carregarFormArquivoUpload();

                    ArquivoFoto.carregarListaArquivoUpload();

                });

            }

        });

    }

    this.iniciarEditable = function () {

        $(idBoxLista).find(".txtEditable").editable("destroy");

        $(idBoxLista).find(".txtEditable").editable({
            container: "#conteudo-principal",
        });

    }

    this.iniciarFancybox = function () {

        $(".itemFotoListaFotos").fancybox({
            maxWidth: 800,
            maxHeight: 600,
            fitToView: false,
            width: '70%',
            height: '70%',
            autoSize: false,
            closeClick: false,
            openEffect: 'none',
            closeEffect: 'none'
        });

    }

    this.registrarPrincipal = function (id) {

        var funcYes = function () {

            var url = ArquivoFoto.baseUrl + "Arquivos/ArquivoFoto/registrar-foto-principal";

            $.post(url, { id: id }, function (response) {

                if (!response.error) {

                    ArquivoFoto.carregarFormArquivoUpload();

                    ArquivoFoto.carregarListaArquivoUpload();

                    return;

                }

                jM.message(response.message);

            })

        }

        jM.confirmation("Voc&ecirc; deseja marcar a foto selecionada como principal?", funcYes, null);

    }

    //Upload de arquivos assincrono
    this.uploadArquivo = function (form) {

        var form = $("#formArquivoFoto");

        var files = form.find("#FileUpload").get(0).files;

        if (files.length == 0) {
            DefaultSistema.reiniciarBotao();
            jM.error("Informe o arquivo que deseja salvar.");
            return;
        }

        var data = new FormData(form[0]);

        var urlUpload = this.baseUrl + "Arquivos/ArquivoFoto/salvar";

        var ajaxRequest = $.ajax({

            type: "POST",
            url: urlUpload,
            enctype: 'multipart/form-data',
            contentType: false,
            processData: false,
            data: data,
            success: function (response) {

                DefaultSistema.reiniciarBotao();

                if (response.error === false) {

                    ArquivoFoto.carregarFormArquivoUpload();

                    ArquivoFoto.carregarListaArquivoUpload();

                }

            },

            error: function (error) {
                console.log(error);
            }

        });

    }

    this.excluir = function (id) {

        var funcYes = function () { 

            var url = ArquivoFoto.baseUrl + "Arquivos/ArquivoFoto/excluir";

            $.post(url, { id: id }, function (response) {

                if (!response.error) {

                    ArquivoFoto.carregarFormArquivoUpload();

                    ArquivoFoto.carregarListaArquivoUpload();

                    return;

                }

                jM.message(response.message);

            })

        }

        jM.confirmation("Voc&ecirc; deseja remover a foto selecionada?", funcYes, null);

    }

};

var ArquivoFoto = new ArquivoFotoClass();

$(document).ready(function () {
    ArquivoFoto.init();
});