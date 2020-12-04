function ProfissaoCadastroClass() {
    
    var baseUrl;
    
    this.init = function () {
        
        this.baseUrl = $("#baseUrlGeral").val();
                
    };
    
    this.onSuccessForm = function (response) {
        
        var func = function () {
            location.reload();
        }
        
        if (response.error == false){
            jM.success("Cadastro realizado com sucesso!",func);
        } 
        
    }

    this.enviarForm = function() {

        var form = $("#formProfissao");

        var data = new FormData(form[0]);

        console.log(data);

        var urlUpload = this.baseUrl + "Profissoes/ProfissaoCadastro/salvar-profissao";

        $.ajax({

            type: "POST",
            url: urlUpload,
            enctype: 'multipart/form-data',
            contentType: false,
            processData: false,
            data: data,
            success: function (response) {

                if (response.error === false) {

                    location.reload();

                    return;
                }

                $("#boxFormProfissao").html(response);

                DefaultSistema.reiniciarBotao();

                DefaultSistema.iniciarPlugins($("#boxFormProfissao"));

            },

            error: function (error) {

                DefaultSistema.reiniciarBotao();

                console.log(error);

            }

        });

    }
    
    
};

var ProfissaoCadastro = new ProfissaoCadastroClass();

$(document).ready(function () {
    ProfissaoCadastro.init();
});
