function ObjDetalhePagamento() {

    this.init = function () {
        
    }

    this.recarregarModal = function (url) {
        
        $.get(url, {}, function (response) {
            
            if (response.error == true){
                
                return;
                
            }
            
            DefaultSistema.removerModais();

            var Modal = $(response).modal({ title: 'Detalhe do Pagamento' });

            $(Modal).on("shown.bs.modal", function (e) {
                DefaultSistema.iniciarPluginsAposAjax();
                EditableCustom.listenerEditables();
            });

            $(Modal).on("hidden.bs.modal", function (e) {
                $(this).remove();
            });            

        });
        
    }
}

var DetalhePagamento = new ObjDetalhePagamento();
$(document).ready(function () {
    DetalhePagamento.init();
});