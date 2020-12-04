function AssociadoContribuicaoEmailClass() {

    //
    this.init = function () {
    };

    //Disparar e-mail de cobranca
    this.enviarEmailCobranca = function (elemento) {

        var url = $(elemento).data("url");
        var idAssociadoCobranca = $(elemento).data("associado-contribuicao");

        
        var fYes = function () {
            $.post(url, {
                idAssociadoContribuicao: idAssociadoCobranca
                },
                function (response) {
                    if (response.error == true) {
                        jM.error(response.message);
                    } else {
                        jM.success(response.message);
                    }
                }
            );
        };

        var fNo = function () {
            return false;
        };

        jM.confirmation("Tem certeza que deseja enviar o e-mail de cobran&ccedil;a para o associado?", fYes, fNo);
    }


};

var AssociadoContribuicaoEmail = new AssociadoContribuicaoEmailClass();

$(document).ready(function(){
    AssociadoContribuicaoEmail.init();
});
