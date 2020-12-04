function ClassLocalizacao(){
    
    var baseUrl;

    this.init = function(){
      
        this.baseUrl = $("#baseUrlGeral").val();

    };

    //Procura de endere�o a partir de um CEP informado.
    this.carregarEndereco = function (sufixElement, funcSuccess) {

        var idPais = "";
        var campoCep = $("input[rel=cep" + sufixElement + "]");
        var campoPais = $("input[rel=idPais" + sufixElement + "]");
        var campoLogradoudo = $("input[rel=logradouro" + sufixElement + "]");
        var campoBairro = $("input[rel=bairro" + sufixElement + "]");
        var campoIdCidade = $("input[rel=idCidade" + sufixElement + "]");
        var campoNomeCidade = $("input[rel=nomeCidade" + sufixElement + "]");
        var boxBuscarCep = $("#buscar" + sufixElement);

        var cep = campoCep.val().replace("-","");

        if (typeof (campoPais) != 'undefined') {
            idPais = $(campoPais).val();
        }

        if (cep.length < 8) {
        	return false;
        }

        boxBuscarCep.removeClass("fa-search").addClass("fa-spinner fa-sync");

        var callback = function (data) {

            if (typeof (data.id) != 'undefined' && data.id != '0') {

                campoLogradoudo.val(data.tipoLogradouro + " " + data.logradouro);
                campoBairro.val(data.bairroIni);
                campoIdCidade.val(data.idCidade);
                campoNomeCidade.val(data.nomeCidade);
                Localizacao.selecionarEstado(data.idEstado, sufixElement);
                Localizacao.carregarCidades(sufixElement, data.idCidade);

                if (typeof funcSuccess !== 'undefined' && $.isFunction(funcSuccess)) {
                    funcSuccess(data);
                }

            } else {
                campoLogradoudo.val('');
                campoBairro.val('');
                campoIdCidade.val('');
                campoNomeCidade.val('');
                jM.error("CEP n&atilde;o localizado!");
            }

            boxBuscarCep.removeClass("fa-spinner fa-sync").addClass("fa-search");
        }

        var url = this.baseUrl + 'localizacao/buscar-endereco';

        $.get(url, { cep : cep }, callback);

    };
    
    //Carregamento de cidades de acordo com o estado escolhido
    this.carregarCidades = function (sufixElement, selectedCity) {
        var codState = $("select[rel=idEstado" + sufixElement + "]").val();

        var callback = function (data) {
            if (data.length > 0) {
                console.log(sufixElement);
                var combo = $("select[rel=idCidade" + sufixElement + "]");
                //console.log(combo);
            	//$("select[data-toggle=select2]").select2('destroy');

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

                combo.select2("");
            }
        }

        var url = this.baseUrl + 'localizacao/carregar-cidades';

        $.get(url, { idEstado: codState }, callback);

    };
    
    // Faz a selec��o de um estado dentro de um combo pre carregado
    this.selecionarEstado = function (idState, sufixElement) {
        var combo = $("select[rel=idEstado"+sufixElement+"]");
       
        $(combo).find("option").each(function(){
            if($(this).val() == idState){
                $(this).attr("selected", "selected");
                return;
            }
        });
        
    };
    
    
    //Caso o elemento esteja configurado com plugin selectbox, faz a reinicializa��o do mesmo para pegar as mudan�as do combo
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

var Localizacao = new ClassLocalizacao();

$(document).ready(function(){
    Localizacao.init();
});
