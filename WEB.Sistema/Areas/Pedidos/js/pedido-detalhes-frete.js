function PedidoDetalhesFreteClass() {

    var baseUrl;
    
    //
    this.init = function () {

        this.baseUrl = $("#baseUrlGeral").val();

    };

    //
    this.carregarEndereco = function (sufixo) {

        var campoCep = $("input[rel=cep" + sufixo + "]").val();

        if (!campoCep) {
            return;
        }
        
        var fnSuccessEndereco = function (response) {
            
            var nomeCidadeUF = (response.nomeCidade + "/" + response.siglaEstado);
            $("#cidade-entrega").val(nomeCidadeUF);

            var campoIdEstado = $("input[rel=idEstado" + sufixo + "]");
            campoIdEstado.val(response.idEstado);

        }

        Localizacao.carregarEndereco(sufixo, fnSuccessEndereco);
        
    }

    //
    this.onSuccess = function (response) {

        if (response.error == false) {

            DefaultSistema.removerModais();

            location.reload();

            return;

        }

        DefaultSistema.iniciarPluginsAposAjax($("#boxModalEnderecoEntrega"));

        DefaultSistema.reiniciarBotao();

    }

};

var PedidoDetalhesFrete = new PedidoDetalhesFreteClass();

$(document).ready(function () {
    PedidoDetalhesFrete.init();
});