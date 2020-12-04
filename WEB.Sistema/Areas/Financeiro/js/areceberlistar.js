function ObjAReceberListar() {

    this.init = function () {
        DefaultSistema.iniciarPlugins();
    };

    this.atualizarTabelaItens = function(id)
    {
        var dados = {
            "id": id,
            "idCentroCusto": $("#idCentroCusto").val(),
            "descricao": $("#descricao").val(),
            "flagPago": $("#flagPago").val(),
            "pesquisarPor": $("#pesquisarPor").val(),
            "dtInicio": $("#dtInicio").val(),
            "dtFim": $("#dtFim").val()
        }

        var callback = function (data) {
            $("#tabelaItensReceitas").html(data);
            AReceberListar.onSuccessPagamentoForm("");
        }

        var url = $("#baseUrlGeral").val() + 'financeiro/areceberlista/partial-tabela-itens';
        var Assync = new ObjAjax();
        Assync.init(url, dados, callback, null, "get", "html");
    }

    //Apos formulario de registro de pagamento da anuidade ser enviado
    this.onSuccessPagamentoForm = function () {

        var boxPagamento = $("#boxRegistrarPagamento");

        boxPagamento.find("input:text").setMask();

        DefaultSistema.iniciarBotoes(boxPagamento);
        DefaultSistema.iniciarPlugins();
    };

    this.removePagamentos = function (element) {

        var id = $(element).attr("data-id");
        var postData = { 'id': [] };

        if (id > 0) {
            postData["id"].push(id);
        } else {
            $("input[type=checkbox][name='checkRegistro[]']:checked").each(function () {
                postData["id"].push($(this).val());
            });
        }

        var url = $(element).attr("data-url");
        if (postData["id"].length == 0) { jM.info("Selecione ao menos um registro."); return false; }

        var func = function () {
            location.reload();
        };

        var confirmDeletePagamentos = 'Tem certeza que deseja efetuar a exclus&atilde;o?<br /> ' +
            'Essa opera&ccedil;&atilde;o n&atilde;o poder&aacute; ser desfeita!<br /><br /> ';

        Ajax.init(url, postData, func, confirmDeletePagamentos);
    };

}

var AReceberListar = new ObjAReceberListar();

$(document).ready(function () {
    AReceberListar.init();
});