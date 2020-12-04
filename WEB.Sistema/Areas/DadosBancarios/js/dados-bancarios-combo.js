function DadosBancariosComboClass() {
    
    var baseUrl;
    
    this.init = function () {

        this.baseUrl = $("#baseUrlGeral").val();
        
    };
    
    this.carregarComboDadoBancario = function (idPessoa) {

        var url = $("#baseUrlGeral").val() + "DadosBancarios/DadosBancariosConsulta/listar-json/";

        $.get(url, { idPessoa : idPessoa }, function (response) {
            if (response.error != undefined && response.error == false) {
                
                var combo = $("#selectDadoBancario");

                var value = $("#selectDadoBancario").attr("datavalue");
                
                combo.find("option").not("option:first").remove();
                combo.find("option:first").html(Vocabulary.loading);
                
                $.each(response.lista, function (key, item) {
                    var selected = value == item.value;
                    combo.append(new Option(item.text, item.value, selected));
                });

                combo.val(value);
                
                return;
            }

            jM.error(response.message);
        });
        
    }
    
};

var DadosBancariosCombo = new DadosBancariosComboClass();

$(document).ready(function () {
    DadosBancariosCombo.init();
});
