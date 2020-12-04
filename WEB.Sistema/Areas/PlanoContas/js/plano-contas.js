function PlanoContasClass() {

    var baseUrl;

    this.init = function(){

        this.baseUrl = $("#baseUrlGeral").val();
        
    };

    //Carregamento de sub contas pai de acordo com a macro conta escolhida
    this.carregarSubContaPai = function (selectedCategoria, idExclude) {
        var idMacroConta = $("#idMacroContaForm").val();
        
        var callback = function (data) {            
            var combo = $("#idCategoriaPai");
            
            if (data.length > 0) {
                
                combo.find("option").not("option:first").remove();
                combo.find("option:first").html(Vocabulary.loading);
                combo.find("option:first").removeAttr("selected");

                var selected = false;
                $.each(data, function (key, item) {
                    selected = (item.value == selectedCategoria ? true : false);
                    combo.append(new Option(item.text, item.value, selected));

                });
                combo.find("option:first").html(Vocabulary.select);
                combo.val(selectedCategoria);

                combo.select2("");

                return;
            }

            combo.find("option").not("option:first").remove();
            combo.select2("");
        }

        var url = this.baseUrl + 'Financeiro/SubContaConsulta/listar-ajax';

        $.get(url, { idMacroConta: idMacroConta, flagSomentePai: true, idExclude: idExclude }, callback);

    };

    //Carregamento de sub contas de acordo com a macro conta escolhida
    this.carregarSubConta = function (selectedCategoria) {
        var idMacroConta = $("#idMacroContaForm").val();

        var callback = function (data) {            
            var combo = $("#idCategoriaNova");
            
            if (data.length > 0) {
                
                combo.find("option").not("option:first").remove();
                combo.find("option:first").html(Vocabulary.loading);
                combo.find("option:first").removeAttr("selected");

                var selected = false;
                $.each(data, function (key, item) {
                    selected = (item.value == selectedCategoria ? true : false);
                    combo.append(new Option(item.text, item.value, selected));

                });
                combo.find("option:first").html(Vocabulary.select);
                combo.val(selectedCategoria);

                combo.select2("");

                return;
            }

            combo.find("option").not("option:first").remove();
            combo.select2("");
        }

        var url = this.baseUrl + 'Financeiro/SubContaConsulta/listar-ajax';

        $.get(url, { idMacroConta: idMacroConta, flagSomentePai: false, idExclude: selectedCategoria }, callback);

    };
    
};

var PlanoContas = new PlanoContasClass();

$(document).ready(function () {
    PlanoContas.init();
});