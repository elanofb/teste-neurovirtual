function ReceitaCloneClass() {

    this.init = function () {
        
    };
    
    //
    this.abrirModal = function (url) {
        
        DefaultSistema.showModal(url, function () {
            
            Devedor.iniciarCombosDevedor();
            
        })
        
    }
    
    //
    this.onSuccessForm = function (response) {
        
        if (response.error === false) {
            
            DefaultSistema.removerModais();
            
            location.href = response.urlRedirect;
            
            return;
        }
        
        DefaultSistema.iniciarPluginsAposAjax($("#boxClonarReceitaForm"));

        Credor.iniciarCombosCredor();
        
    }
    
}
 
var ReceitaClone = new ReceitaCloneClass();

$(document).ready(function(){
    ReceitaClone.init();
});
