function PedidoCancelamentoClass() {

    var baseUrl;
    
    //
    this.init = function () {
        
        this.baseUrl = $("#baseUrlGeral").val();
        
    };
    
    //
    this.abrirModalCancelamento = function (id) {

        var postData = { 'ids': [] };

        if (id > 0) {
            
            postData["ids"].push(id);
            
        } else {
            
            $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () { 
                postData["ids"].push($(this).val()); 
            });
            
        }

        if (postData["ids"].length == 0) {
            
            jM.info("Informe ao menos um pedido.");
            
            return false;
            
        }

        var url = this.baseUrl + "Pedidos/PedidoCancelamento/modal-cancelar-pedido";

        $.post(url, postData, function(response) {
            
                var Modal = $(response).modal();

                Modal.on("shown.bs.modal", function (e) {
                    
                    DefaultSistema.reiniciarBotao();
                    
                    DefaultSistema.iniciarPlugins($("#boxModalCancelamento"));
                    
                });

            }
        );
        
    }
    
    //
    this.onSuccess = function (response) {

        if (response.error == false) {

            DefaultSistema.removerModais();

            location.reload();

            return;

        }

        DefaultSistema.iniciarPluginsAposAjax($("#boxModalCancelamento"));
        
        DefaultSistema.reiniciarBotao();

    }

};

var PedidoCancelamento = new PedidoCancelamentoClass();

$(document).ready(function () {
    PedidoCancelamento.init();
});