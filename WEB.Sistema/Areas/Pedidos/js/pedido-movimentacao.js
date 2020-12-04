function PedidoMovimentacaoClass() {
    
    var boxResumoInscricoes = "#boxResumoPedidos";
    var baseUrlGeral;

    this.init = function () {

        this.baseUrlGeral = $("#baseUrlGeral").val();

    };
    
    this.showModalMovimentacao = function (elem) {
        
        var postData = { 'idsPedidos': [] };
        
        $("input.idPedido").each(function () {
            postData["idsPedidos"].push($(this).val());
        });
        
        var url = $(elem).data("url");
        
        $.post(url, postData, function (response) {
            
            if (response.error == true) {

                jM.error(response.message);

                return;

            }
            
            var Modal = $(response).modal();
            
            $(Modal).on("shown.bs.modal", function (e) {

                DefaultSistema.reiniciarBotao();
                
                $('input:text').setMask();
                
                $("#boxPedidos").slimScroll({
                    height: 225
                });
                
                $("#boxPedidos").removeClass("hide");

            });
            
            $(Modal).on("hidden.bs.modal", function (e) {
                $(this).remove();
            });
            
        });

    }
    
    this.onSuccessForm = function (response) {
        $("#boxFormMovimentacaoPedidos").replaceWith(response);
        $(".campo-busca").focus();
        $("#idRemoverPedido").val(0);
    }
    
    this.movimentarPedidos = function (idStatusPedido, nomeProcesso) {
                
        var funcYes = function () {

            var formData = $("#formMovimentacaoPedidos").serialize();
            var postData = formData+"&idStatusPedido="+idStatusPedido;
            
            var url = PedidoMovimentacao.baseUrlGeral+"Pedidos/MovimentacaoPedido/movimentar-pedidos";
                        
            $.post(url, postData, function (response) {

                if (response.error == false){
                    location.reload();
                    return;
                }

                $("#boxFormMovimentacaoPedidos").html(response);
                DefaultSistema.reiniciarBotao();
            });
            
        }

        jM.confirmation("Voc&ecirc; est&aacute; movimentando o(s) "+$('#qtdPedidosSelecionados').html()+" para o processo de "+nomeProcesso+". Confirma a movimentação?", funcYes);
        DefaultSistema.reiniciarBotao();
    }
    
    this.removerPedidoLista = function (idPedido) {

        var formData = $("#formMovimentacaoPedidos").serialize();
        var postData = formData+"&idRemover="+idPedido;
        
        var url = PedidoMovimentacao.baseUrlGeral+"Pedidos/MovimentacaoPedido/remover-pedidos";

        $.post(url, postData, function (response) {
            
            $("#boxFormMovimentacaoPedidos").replaceWith(response);
            $(".campo-busca").focus();
            $("#idRemoverPedido").val(0);
            
        });

    }
        
};

var PedidoMovimentacao = new PedidoMovimentacaoClass();

$(document).ready(function () {
    PedidoMovimentacao.init();
});