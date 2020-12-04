function PedidoCadastroOperacaoClass() {

    var baseUrl;

    this.init = function () {

        this.baseUrl = $("#baseUrlGeral").val();
        
    };

    this.novoPedido = function () {

        var funcYes = function() {
        
            location.href = PedidoCadastroOperacao.baseUrl + "Pedidos/PedidoCadastroOperacao/novo-pedido";

        }

        jM.confirmation("Voc&ecirc; deseja iniciar um novo pedido? Todos os dados atuais ser&atilde;o perdidos.", funcYes, null);

    }

};

var PedidoCadastroOperacao = new PedidoCadastroOperacaoClass();

$(document).ready(function () {

    PedidoCadastroOperacao.init();

});