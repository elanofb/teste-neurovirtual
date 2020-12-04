function ObjAjudaCategoria() {
    
	this.init = function() {
	};

    //Abre a modal para adicionar uma nova categoria
    this.modalAjudaCategoria = function (element) {

        var url = $(element).data("url");

        $.get(url, {}, function (response) {

            var Modal = $(response).modal({ title: 'Cadastrar Categoria de Ajuda' });

            $(Modal).on("shown.bs.modal", function (e) { DefaultSistema.iniciarPluginsAposAjax($("#boxFormAjudaCategoria")); });

            $(Modal).on("hidden.bs.modal", function (e) { $(this).remove(); });
        });
    }

    //Evento chamado ao submeter o form da modal de cadastrar categoria
    this.onSuccessAjudaCategoria = function (response) {

        if (response.error == undefined) {
            DefaultSistema.iniciarPluginsAposAjax($("#boxFormAjudaCategoria"));
            DefaultAreaAssociado.reiniciarBotao();
            return;
        }

        if (response.error == false) {

            jM.success("Categoria cadastrada com sucesso!",function(){
                location.reload()
            });

            return;

        }

        DefaultAreaAssociado.iniciarPlugins($("#boxFormAjudaCategoria"));

        DefaultAreaAssociado.reiniciarBotao();
    }
};

var AjudaCategoria = new ObjAjudaCategoria();
$(document).ready(function(){
    AjudaCategoria.init();
});
