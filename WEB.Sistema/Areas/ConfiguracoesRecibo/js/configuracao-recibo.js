function ConfiguracaoReciboClass() {
    
    //Inicializador de metodos
    this.init = function() {

        this.iniciarEditores();

    };

    //Iniciar plugin popover
    this.iniciarEditores = function () {

        $(".froala-editor").each(function() {

            $(this).froalaEditor({
                language: 'pt_br',
                height: 400
            });

            
        });

    };


}

var ConfiguracaoRecibo = new ConfiguracaoReciboClass();

$(document).ready(function () {

    ConfiguracaoRecibo.init();
});