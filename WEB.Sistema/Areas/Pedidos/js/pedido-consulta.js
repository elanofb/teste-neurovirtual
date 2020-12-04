function PedidoConsultaClass() {

    var baseUrl;
    
    //
    this.init = function () {
        
        this.baseUrlGeral = $("#baseUrlGeral").val();

        this.onFormSubmit();

        this.iniciarBoxListaEmAberto();

        this.iniciarBoxListaPagos();

        this.iniciarBoxListaEmProducao();

        this.iniciarBoxListaEmTransporte();
        
        this.iniciarBoxListaFinalizados();

        this.iniciarBoxListaAtrasados();
        
        this.iniciarBoxListaCancelados();

    };

    //
    this.onFormSubmit = function () {

        $("#formFiltros").on("submit", function (e) {

            e.preventDefault();

            PedidoConsulta.iniciarBoxListaEmAberto();

            PedidoConsulta.iniciarBoxListaPagos();

            PedidoConsulta.iniciarBoxListaEmProducao();

            PedidoConsulta.iniciarBoxListaEmTransporte();

            PedidoConsulta.iniciarBoxListaFinalizados();

            PedidoConsulta.iniciarBoxListaAtrasados();
            
            PedidoConsulta.iniciarBoxListaCancelados();

            DefaultSistema.reiniciarBotao();

        });

    }

    //
    this.iniciarBoxListaEmAberto = function () {

        var boxLista = $("#boxListaPedidosEmAberto");

        var url = this.baseUrlGeral.concat("Pedidos/PedidoConsulta/em-aberto");

        this.iniciarBoxLista(boxLista, url);

    }

    //
    this.iniciarBoxListaPagos = function () {

        var boxLista = $("#boxListaPedidosPagos");

        var url = this.baseUrlGeral.concat("Pedidos/PedidoConsulta/pagos");

        this.iniciarBoxLista(boxLista, url);

    }

    //
    this.iniciarBoxListaEmProducao = function () {

        var boxLista = $("#boxListaPedidosEmProducao");

        var url = this.baseUrlGeral.concat("Pedidos/PedidoConsulta/em-producao");

        this.iniciarBoxLista(boxLista, url);

    }
    
    //
    this.iniciarBoxListaEmTransporte = function () {

        var boxLista = $("#boxListaPedidosEmTransporte");

        var url = this.baseUrlGeral.concat("Pedidos/PedidoConsulta/em-transporte");

        this.iniciarBoxLista(boxLista, url);

    }


    //
    this.iniciarBoxListaFinalizados = function () {

        var boxLista = $("#boxListaPedidosFinalizados");

        var url = this.baseUrlGeral.concat("Pedidos/PedidoConsulta/finalizados");

        this.iniciarBoxLista(boxLista, url);

    }

    //
    this.iniciarBoxListaAtrasados = function () {

        var boxLista = $("#boxListaPedidosAtrasados");

        var url = this.baseUrlGeral.concat("Pedidos/PedidoConsulta/atrasados");

        this.iniciarBoxLista(boxLista, url);

    }
    
    //
    this.iniciarBoxListaCancelados = function () {

        var boxLista = $("#boxListaPedidosCancelados");

        var url = this.baseUrlGeral.concat("Pedidos/PedidoConsulta/cancelados");

        this.iniciarBoxLista(boxLista, url);

    }

    //
    this.iniciarBoxLista = function (boxLista, url) {

        boxLista.html("");
        boxLista.addClass("carregando");

        var nroPagina = boxLista.find("#nroPagina").val();

        var dadosForm = $("#formFiltros").serializeObject();
        dadosForm["nroPagina"] = nroPagina;

        $.post(url, dadosForm, function (response) {

            boxLista.html(response);

            boxLista.removeClass("carregando");
            
            DefaultSistema.iniciarCheckBoxes();

        })

    }

};

var PedidoConsulta = new PedidoConsultaClass();

$(document).ready(function () {
    PedidoConsulta.init();
});