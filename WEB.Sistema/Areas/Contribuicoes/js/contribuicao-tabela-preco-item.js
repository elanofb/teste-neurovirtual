function ContribuicaoTabelaPrecoItemClass() {

    //Metodo de inicializacao dos plugins
    this.init = function () {

    };

    //Listener
    this.onSuccessFormPrecoItem = function (response) {

        DefaultSistema.carregarConteudoElemento("boxTabelaPreco");

        DefaultSistema.carregarConteudoElemento("boxTabelaPrecoModal");

        $(boxFormTabelaPrecoItem).find("input:text").setMask();

        DefaultSistema.reiniciarBotao();
    }

    //Remover item de uma tabela de preço
    this.removerItem = function (elemento) {

        var fYes = function() {

            var id = $(elemento).data("id");

            var url = new String($("#baseUrlGeral").val()).concat("Contribuicoes/ContribuicaoTabelaPrecoItem/excluir");

            $.post(url,
                { id: id },
                function (response) {
                    console.log(response);
                   if (response.error === true) {
                       jM.error(response.message);
                       return;
                   }

                   jM.success(response.message);

                   DefaultSistema.carregarConteudoElemento("boxTabelaPreco");
                   DefaultSistema.carregarConteudoElemento("boxTabelaPrecoModal");
                }
            );
        };

        var fNo = function() {
            return false;
        };

        jM.confirmation("Confirma a remo&ccedil;&atilde;o do item selecionado?", fYes, fNo);
    }
};

var ContribuicaoTabelaPrecoItem = new ContribuicaoTabelaPrecoItemClass();

$(document).ready(function(){
    ContribuicaoTabelaPrecoItem.init();
});
