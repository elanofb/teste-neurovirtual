function ConfiguracaoEmailClass() {
    
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
                toolbarButtons: ['bold', 'italic', 'underline', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink', 'html'],
                toolbarButtonsXS: ['bold', 'italic', 'underline', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink', 'html'],
                toolbarButtonsSM: ['bold', 'italic', 'underline', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink', 'html'],
                toolbarButtonsMD: ['bold', 'italic', 'underline', '|', 'color', '|', 'align', 'formatOL', 'formatUL', 'insertHR', 'insertLink', 'html']
            });

            
        });

    };
    
    //
    this.onChangeGatewayNotificacao = function (elem) {
        
        $(".box-configuracoes-gateway").addClass("hide");
        
        var idGateway = $(elem).val();

        if (idGateway == 1) { // E-mail Próprio
            $("#boxDadosEmailProprio").removeClass("hide");
        }

        if (idGateway != 1) { // SendGrid
            $("#boxDadosIntegracao").removeClass("hide");
        }
        
    }


}

var ConfiguracaoEmail = new ConfiguracaoEmailClass();

$(document).ready(function () {

    ConfiguracaoEmail.init();
});