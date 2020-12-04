function ObjPedidosEmAndamento() {

    var boxListaPedidos = $("#boxListaPedidos");

    this.init = function () {

    };

    this.onSuccess = function (response) {
        DefaultSistema.iniciarPluginsAposAjax(boxListaPedidos);
    };

    this.buscarPedidosPorStatus = function (id) {

        $(".widget").removeClass("selected");
        $(".widget_status_" + id).addClass("selected");

        var url = new String($("#baseUrlGeral").val()).concat("Pedidos/pedidosemandamento/listar-em-andamento?idStatusPedido=").concat(id);
        boxListaPedidos.html("");
        boxListaPedidos.addClass("carregando");
        $.get(url, {}, function (response) {
            boxListaPedidos.removeClass("carregando");
            boxListaPedidos.html(response);
            DefaultSistema.iniciarPluginsAposAjax(boxListaPedidos);
        });

    }

};

var PedidosEmAndamento = new ObjPedidosEmAndamento();
$(document).ready(function () {
    PedidosEmAndamento.init();
});