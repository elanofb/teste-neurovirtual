function ObjGaleriaFoto() {

    var idBoxArquivoLista = "#boxArquivosListar";

    this.init = function () {

        this.iniciarEditor();
        this.carregarListaArquivos();
    };


    //Inicializacao do plugin de edicao do campo textarea
    this.iniciarEditor = function () {

        var urlUploadImagem = new String($("#baseUrlGeral").val()).concat("Arquivos/froalaupload/salvar-foto");
        var urlUploadFile = new String($("#baseUrlGeral").val()).concat("Arquivos/froalaupload/salvar-arquivo");

        $('#editor').froalaEditor({
            language: 'pt_br',
            height: '350',
            imageUploadURL: urlUploadImagem,
            fileUploadURL: urlUploadFile
        })
        .on('froalaEditor.file.beforeUpload', function (e, editor, files) {
        })
        .on('froalaEditor.file.uploaded', function (e, editor, response) {
        })
        .on('froalaEditor.file.inserted', function (e, editor, $file, response) {
        })
        .on('froalaEditor.file.error', function (e, editor, error, response) {
            console.log(e);
            console.log(error);
            console.log(response);
            if (error.code === 6) {
                jM.error("O arquivo informado &eacute; inv&aacute;lido!");
            }
        })
        .on('froalaEditor.image.error', function (e, editor, error) {
            if (error.code === 6) {
                jM.error("O arquivo informado &eacute; inv&aacute;lido!");
            }
        })
        ;

    }

    //Carregamento da lista de arquivos
    this.carregarListaArquivos = function () {
        var elemento = $(idBoxArquivoLista);
        var url = elemento.data("url");

        $.get(url, {},
            function (response) {
                elemento.html(response);
                elemento.removeClass("carregando");
                //DefaultSistema.iniciarSortable();
                DefaultSistema.iniciarLinksAcao();
                DefaultSistema.iniciarPlugins();
                EditableCustom.listenerEditables();
            }
        );
    };
};

var GaleriaFoto = new ObjGaleriaFoto();

$(document).ready(function () {
    GaleriaFoto.init();
});
