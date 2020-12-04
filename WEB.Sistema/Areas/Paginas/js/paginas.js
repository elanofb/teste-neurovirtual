function PaginasClass() {

    this.init = function () {

        this.iniciarEditores();

    }

    this.iniciarEditores = function () {

        var urlUploadImagem = new String($("#baseUrlGeral").val()).concat("Arquivos/froalaupload/salvar-foto");
        var urlUploadFile = new String($("#baseUrlGeral").val()).concat("Arquivos/froalaupload/salvar-arquivo");

        $(".froala-editor").each(function () {

            $(this).froalaEditor({
                htmlRemoveTags: ['base'],
                htmlAllowedAttrs: ['accept', 'accept-charset', 'accesskey', 'action', 'align', 'allowfullscreen', 'allowtransparency', 'alt', 'async', 'autocomplete', 'autofocus', 'autoplay', 'autosave', 'background', 'bgcolor', 'border', 'charset', 'cellpadding', 'cellspacing', 'checked', 'cite', 'class', 'color', 'cols', 'colspan', 'content', 'contenteditable', 'contextmenu', 'controls', 'coords', 'data', 'data-.*', 'datetime', 'default', 'defer', 'dir', 'dirname', 'disabled', 'download', 'draggable', 'dropzone', 'enctype', 'for', 'form', 'formaction', 'frameborder', 'headers', 'height', 'hidden', 'high', 'href', 'hreflang', 'http-equiv', 'icon', 'id', 'ismap', 'itemprop', 'keytype', 'kind', 'label', 'lang', 'language', 'list', 'loop', 'low', 'max', 'maxlength', 'media', 'method', 'min', 'mozallowfullscreen', 'multiple', 'muted', 'name', 'novalidate', 'open', 'optimum', 'pattern', 'ping', 'placeholder', 'playsinline', 'poster', 'preload', 'pubdate', 'radiogroup', 'readonly', 'rel', 'required', 'reversed', 'rows', 'rowspan', 'sandbox', 'scope', 'scoped', 'scrolling', 'seamless', 'selected', 'shape', 'size', 'sizes', 'span', 'src', 'srcdoc', 'srclang', 'srcset', 'start', 'step', 'summary', 'spellcheck', 'style', 'tabindex', 'target', 'title', 'type', 'translate', 'usemap', 'value', 'valign', 'webkitallowfullscreen', 'width', 'wrap', 'onchange'],
                language: 'pt_br',
                height: '500',
                imageUploadURL: urlUploadImagem,
                fileUploadURL: urlUploadFile
            })
            .on('froalaEditor.file.beforeUpload', function (e, editor, files) { })
            .on('froalaEditor.file.uploaded', function (e, editor, response) { })
            .on('froalaEditor.file.inserted', function (e, editor, $file, response) { })
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
            });
        });
    }
}

var Paginas = new PaginasClass();

$(document).ready(function () {

    Paginas.init();

});