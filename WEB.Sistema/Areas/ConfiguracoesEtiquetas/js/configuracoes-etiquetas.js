function ConfiguracoesEtiquetaClass() {

    //Inicializador de metodos
    this.init = function () {

        this.listenerPopover();
        this.iniciarEditores();
    };

    //Iniciar plugin popover
    this.listenerPopover = function () {
        $('.for-popover').each(function () {

            var pop = $(this);
            var divConteudo = pop.data("url");
            var titulo = pop.data("title");

            console.log(divConteudo);

            pop.popover({
                title: (titulo),
                html: true,
                container: 'body',
                content: function () {
                    return $(divConteudo).html();
                }
            });
        });
    };

    //Iniciar plugin popover
    this.iniciarEditores = function () {
        $(".froala-editor").each(function () {
            $(this).froalaEditor({
                language: 'pt_br',
                height: 305,
                toolbarButtons: [
                    'fullscreen', 'bold', 'italic', 'underline', 'strikeThrough', 'subscript', 'superscript', 'fontFamily', 'fontSize',
                    'color', 'paragraphFormat', 'align', 'formatOL', 'formatUL', 'outdent', 'indent', 'insertLink', 'insertImage',
                    'insertFile', 'insertTable', 'quote', 'insertHR', 'undo', 'redo', 'clearFormatting', 'html']
            });
        });
    };
}

var ConfiguracoesEtiqueta = new ConfiguracoesEtiquetaClass();
$(document).ready(function () {
    ConfiguracoesEtiqueta.init();
});