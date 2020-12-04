function DespesaCloneClass() {

    this.init = function () {
        
    };
    
    //
    this.abrirModal = function (url) {
        
        DefaultSistema.showModal(url, function () {
            
            Credor.iniciarCombosCredor();
            
        })
        
    }
    
    //
    this.onSuccessForm = function (response) {
        
        if (response.error === false) {
            
            DefaultSistema.removerModais();
            
            location.href = response.urlRedirect;
            
            return;
        }
        
        DefaultSistema.iniciarPluginsAposAjax($("#boxClonarDespesaForm"));

        Credor.iniciarCombosCredor();
        
    }
    
}
 
var DespesaClone = new DespesaCloneClass();

$(document).ready(function(){
    DespesaClone.init();
});
