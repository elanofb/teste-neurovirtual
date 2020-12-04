function ConfiguracaoNotificacaoClass() {
    
    //Inicializador de metodos
    this.init = function() {

        this.iniciarEditores();

    };

    //Iniciar plugin popover
    this.iniciarEditores = function () {

        $(".froala-editor").each(function() {

            $(this).froalaEditor({
                language: 'pt_br',
                height: 200,
                toolbarButtons: ['bold', 'italic', 'underline', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink', '|', 'html'],
                toolbarButtonsXS: ['bold', 'italic', 'underline', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink', '|', 'html'],
                toolbarButtonsSM: ['bold', 'italic', 'underline', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink', '|', 'html'],
                toolbarButtonsMD: ['bold', 'italic', 'underline', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink', '|', 'html']
            });

            
        });

    };


}

var ConfiguracaoNotificacao = new ConfiguracaoNotificacaoClass();

$(document).ready(function () {

    ConfiguracaoNotificacao.init();
});