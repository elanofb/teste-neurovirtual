function ObjLocate(){
    
    this.init = function(){
      
    };

    
    /**
     * Procura de endereço a partir de um CEP informado.
     */
    this.carregarEndereco = function (sufixElement) {
    	var idPais = "";
        var cep = $("input[rel=cep" + sufixElement + "]").val();
        var objPais = $("input[rel=idPais" + sufixElement + "]");
        if (typeof (objPais) != 'undefined') {
        	idPais = $(objPais).val();
        }

        if (cep.length < 8) {
        	return false;
        }

        var callback = function (data) {
            if (typeof (data.id) != 'undefined') {
                //$("input[rel=cep" + sufixElement + "]").val(data.cepOriginal);
                $("input[rel=logradouro" + sufixElement + "]").val(data.tipoLogradouro + " " + data.logradouro);
                $("input[rel=bairro" + sufixElement + "]").val(data.bairroIni);
                $("input[rel=idCidade" + sufixElement + "]").val(data.idCidade);
                $("input[rel=nomeCidade" + sufixElement + "]").val(data.nomeCidade);
                selectState(data.idEstado, sufixElement);
                Locate.loadCities(sufixElement, data.idCidade);
            } else {
                jM.error(data.message);
            }
        }
        Ajax.init($("#baseUrlGeral").val() + 'locate/carregarEnderecoByCEP', { "cep": cep }, callback, null);
    };
    
    /**
     * Carregamento de cidades de acordo com o estado escolhido
     */
    this.loadCities = function (sufixElement, selectedCity) {
        var codState = $("select[rel=idEstado" + sufixElement + "]").val();

        var callback = function (data) {
            if (data.length > 0) {
            	var combo = $("select[rel=idCidade" + sufixElement + "]");

                combo.find("option").not("option:first").remove();
                combo.find("option:first").html(Vocabulary.loading);
                combo.find("option:first").removeAttr("selected");

                var selected = false;
                $.each(data, function (key, item) {
                    selected = (item.id == selectedCity ? true : false);
                    combo.append(new Option(item.nome, item.id, selected));
					
                });
                combo.find("option:first").html(Vocabulary.select);
                combo.val(selectedCity);

                $(combo).select2();
            }
        }

        Ajax.init($("#baseUrlGeral").val() + 'locate/loadCities', { "state": codState }, callback, null);
    };
    
    /**
     * Faz a selecção de um estado dentro de um combo pre carregado
     */
    var selectState = function(idState, sufixElement){
        var combo = $("select[rel=idEstado"+sufixElement+"]");
       
        $(combo).find("option").each(function(){
            if($(this).val() == idState){
                $(this).attr("selected", "selected");
                return;
            }
        });
        
    };
    
    
    /**
     * Caso o elemento esteja configurado com plugin selectbox, faz a reinicialização do mesmo para pegar as mudanças do combo
     */
    var resetSelectBox = function(element, enable){
        if(element.hasClass('selectBox')){
            element.selectBox("destroy");
            element.selectBox();
            if(enable){
                element.selectBox('enable');
            }else{
                element.selectBox('disable');
            }
        }
    }
    
    
    
};

var Locate = new ObjLocate();

$(document).ready(function(){
    Locate.init();
});
