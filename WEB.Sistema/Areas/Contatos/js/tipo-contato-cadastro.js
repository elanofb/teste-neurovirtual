function TipoContatoCadastroClass() {
    
    // É possível receber alguns parametros de retorno em caso de sucesso: 
    // - flagRecarregar: define se a página será recarregada
    // - idComboSelecionar: id da combo que receberá a nova opção de tipo de contato cadastrada
    // - id: value da opção da combo que será adicionada
    // - descricao: texto da opção da combo que será adicionada
    this.onSuccessForm = function (response) {
        
        if (response.error == false) {
            
            DefaultSistema.removerModais();
            
            if (response.flagRecarregar == true) {
                location.reload();
            }
            
            if (response.flagRecarregar == false){
                TipoContatoCadastro.preencherCombos(response);
            }
            
            return false;
            
        }
        
        DefaultSistema.iniciarPluginsAposAjax($("#boxFormTipoContato"));
        
    }
    
    this.preencherCombos = function (response) {

        var combos = $(".combo-tipo-contato");

        $(combos).each(function () {

            var combo = $(this);

            var flagJaExiste = combo.find("option[value=" + response.id + "]").length > 0;

            if (flagJaExiste === false) {

                combo.append($("<option></option>").val(response.id).html(response.descricao));

            }

        })

        var comboSelecionar = $("#" + response.idComboSelecionar);

        comboSelecionar.val(response.id);
        
    }
    
}

var TipoContatoCadastro = new TipoContatoCadastroClass();

$(document).ready(function () {
    
})