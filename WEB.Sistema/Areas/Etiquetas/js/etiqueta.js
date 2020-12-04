function EtiquetaClass(){
    
    var baseUrl;
    
    this.init = function () {
        
        this.baseUrl = $("#baseUrlGeral").val();
        
    }
    
    this.alterarModeloEtiqueta = function (element) {

        var idConfiguracaoEtiqueta = $(element).val();

        var param, params_arr = [];
        
        var queryString = window.location.search.substr(1);
        
        if (queryString !== "") {
            
            params_arr = queryString.split("&");
            
            for (var i = params_arr.length - 1; i >= 0; i -= 1) {
                
                param = params_arr[i].split("=")[0];
                
                if (param === "idConfiguracaoEtiqueta") {
                    
                    params_arr.splice(i, 1);
                    
                }
                
            }
            
        }

        var url = this.baseUrl + "Etiquetas/EtiquetaProduto/Index?" + params_arr.join("&") + "&idConfiguracaoEtiqueta=" + idConfiguracaoEtiqueta;
        
        window.location = url;
        
    }
    
}

var Etiqueta = new EtiquetaClass();

$(document).ready(function () {
    
    Etiqueta.init();
    
})