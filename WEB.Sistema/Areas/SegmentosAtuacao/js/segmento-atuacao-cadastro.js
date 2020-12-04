function SegmentoAtuacaoCadastroClass() {
    
    var baseUrl;
    
    this.init = function () {
        
        this.baseUrl = $("#baseUrlGeral").val();
                
    };

    this.onSuccessForm = function(response) {

        if (response.error == false) {

            $("#modal-segmento").modal("toggle");
            
            $("#idSegmento").append("<option value='"+response.id+"' selected>"+response.descricao+"</option>");

            jM.success(response.message);

            return;
        }

        jM.error(response.message);

        DefaultSistema.reiniciarBotao();

    }
    
};

var SegmentoAtuacaoCadastro = new SegmentoAtuacaoCadastroClass();

$(document).ready(function () {
    SegmentoAtuacaoCadastro.init();
});
