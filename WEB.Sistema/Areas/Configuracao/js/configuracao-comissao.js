function ConfiguracaoComissaoClass() {
    this.init = function () { };

    this.vincularPerfilComissionavel = function (elemento) {

        var idPerfilAcesso = $("#idPerfilAcesso").val();
        var idConfiguracaoComissao = $(elemento).data("idconfiguracaocomissao");

        var url = $(elemento).data("url");

        $.post(url, { idConfiguracaoComissao: idConfiguracaoComissao, idPerfilAcesso: idPerfilAcesso }, function (response) {
            DefaultSistema.carregarConteudo($("#boxPerfilComissavel"))
        });
    };

    this.itemExcluido = function () {

        DefaultSistema.carregarConteudo($("#boxPerfilComissavel"));
    }
};

var ConfiguracaoComissao = new ConfiguracaoComissaoClass();
$(document).ready(function () {
    ConfiguracaoComissao.init();
});