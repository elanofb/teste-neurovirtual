function ContribuicaoTabelaPrecoClass() {

    var idBoxModalTabelaPreco = '#boxTabelaPreco';
    var idBoxFormTabelaPrecoItem = '#boxFormTabelaPrecoItem';

    //Metodo de inicializacao dos plugins
    this.init = function () {

    };

    //Listener
    this.onSuccessForm = function(response) {

        if (response.error === false) {

            location.reload(true);

            return;
        }

        $(idBoxModalTabelaPreco).find("input:text").setMask();

        DefaultSistema.reiniciarBotao();
    };

    //Remover uma tabela de preço
    this.removerTabela = function (elemento) {

        var fYes = function () {

            var id = $(elemento).data("id");

            var url = new String($("#baseUrlGeral").val()).concat("Contribuicoes/ContribuicaoTabelaPreco/excluir");

            $.post(url,
                { id: id },
                function (response) {

                    if (response.error === false) {
                        location.reload(true);
                        return;
                    }

                    jM.error(response.message);

                }
            );
        };

        var fNo = function () {
            return false;
        };

        jM.confirmation("Confirma a remo&ccedil;&atilde;o da tabela?", fYes, fNo);
    }


};

var ContribuicaoTabelaPreco = new ContribuicaoTabelaPrecoClass();

$(document).ready(function(){
    ContribuicaoTabelaPreco.init();
});
