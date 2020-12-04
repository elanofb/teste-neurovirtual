function ObjTituloImpostoCadastro() {

	this.init = function() {
	};

    //Evento chamado ao submeter o form da modal de cadastrar a sub conta
    this.onSuccess = function (response) {

        if (response.error == undefined) {
            DefaultSistema.iniciarPluginsAposAjax($("#boxFormTituloImposto"));

            return;
        }

        if (response.error == false){
            
            var func = function(){
                location.reload();
            }
            
            jM.success(response.message, func);
            
        } 
    }
    
}

var TituloImpostoCadastro = new ObjTituloImpostoCadastro();
$(document).ready(function(){
    TituloImpostoCadastro.init();
});
