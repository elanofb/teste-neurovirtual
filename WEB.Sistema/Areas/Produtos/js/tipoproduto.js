function TipoProdutoClass() {

    this.init = function () {
        
    };

    //Abrir modal para cadastrar novo local de evento
    this.modalNovoTipoProduto = function (elemento) {

        var url = $(elemento).data("url");

        $.get(url, {}, function (response) {

            var Modal = $(response).modal({ title: 'Cadastrar Tipo de Produto' });

            $(Modal).on("shown.bs.modal", function (e) {
                DefaultSistema.iniciarPluginsAposAjax();
            });

            $(Modal).on("hidden.bs.modal", function (e) {
                $(this).remove();
            });

        });

    }

    //Retorno após submissoes do form no modal
    this.onSuccess = function (response) {

        if (typeof (response.error) == 'undefined') {
            DefaultSistema.iniciarPluginsAposAjax();
            return;
        }

        //Se o registro for salvo com sucesso
        if (response.error === false) {

            var combo = $("#Produto_idTipoProduto");

            var flagJaExiste = combo.find("option[value=" + response.id + "]").length > 0;

            if (flagJaExiste === false) {

                combo.append($("<option></option>").val(response.id).html(response.descricao));

                combo.val(response.id);

                DefaultSistema.removerModais();
            }

        }
    }
};

var TipoProduto = new TipoProdutoClass();

$(document).ready(function () {
    TipoProduto.init();
});
