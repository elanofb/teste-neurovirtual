function DadosBancariosCadastroClass() {
    
    var baseUrl;
    
    this.init = function () {

        this.baseUrl = $("#baseUrlGeral").val();
        
    };

    //Abrir modal para cadastrar novo local de evento
    this.modalNovoDadoBancarioCredor = function () {

        var url = $("#baseUrlGeral").val() + "DadosBancarios/DadosBancariosCadastro/modal-cadastro";

        var idReferencia = $("#idReferenciaPessoa").val();
        
        if (idReferencia == ""){
            jM.error("Para cadastrar uma nova conta bancária é necessário selecionar o credor.");
            return;
        }

        var idPessoa = idReferencia.split("#");

        $.get(url, {idPessoa : idPessoa[1]}, function (response) {

            var Modal = $(response).modal({ title: 'Cadastrar Conta Bancaria' });

            $(Modal).on("shown.bs.modal", function (e) {
                DefaultSistema.iniciarPluginsAposAjax($("#boxFormContaBancaria"));
            });

            $(Modal).on("hidden.bs.modal", function (e) {
                $(this).remove();
            });

        });
    }
    
    this.onSuccessForm = function (response) {

        DefaultSistema.reiniciarBotao();
        
        if (response.error == false){
            
            if ($("#selectDadoBancario").length > 0){
                var newOption = new Option(response.text, response.id, true);
                $("#selectDadoBancario").append(newOption);

                $("#selectDadoBancario").val(response.id);

                DefaultSistema.removerModais();
                
                return;
            } 
            
            location.reload();
            
        }
        
    }
    
};

var DadosBancariosCadastro = new DadosBancariosCadastroClass();

$(document).ready(function () {
    DadosBancariosCadastro.init();
});
