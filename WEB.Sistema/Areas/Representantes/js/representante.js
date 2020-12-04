function ObjRepresentante() {

    var baseUrl;
    
    //
    this.init = function () {
        
        this.baseUrl = $("#baseUrlGeral").val();
        
    };
    
    //
    this.abrirModal = function (url) {
              
        DefaultSistema.showModal(url, function () {

            DefaultSistema.iniciarLinksAcao($("#boxFormRepresentante"));

            Representante.onChangeTipoPessoa();
            
        })
        
    };
    
    this.enviarForm = function(funcSuccess) {

        var form = $("#formRepresentante");

        var data = new FormData(form[0]);

        var urlUpload = this.baseUrl + "Representantes/RepresentanteCadastro/salvar";

        $.ajax({
        
            type: "POST",
            url: urlUpload,
            enctype: 'multipart/form-data',
            contentType: false,
            processData: false,
            data: data,
            success: function (response) {
                
                if (response.error === false) {
                    
                    DefaultSistema.removerModais();
                        
                    location.reload();
                       
                    return;
                    
                }

                $("#boxFormRepresentante").html(response);
                
                DefaultSistema.reiniciarBotao();
                
                DefaultSistema.iniciarPlugins($("#boxFormRepresentante"));

                Representante.onChangeTipoPessoa();
                
            },

            error: function (error) {

                DefaultSistema.reiniciarBotao();

                console.log(error);

            }

        });

    }
    
    //
    this.onChangeTipoPessoa = function() {
        
        var flagTipoPessoa = $("#flagTipoPessoa").val();

        if (flagTipoPessoa == "J") {
            $(".box-pf").addClass("hide");
            $(".box-pj").removeClass("hide");
            $("#Representante_Pessoa_nroDocumento").setMask("cnpj");
            $(".info-documento").html("CNPJ");
        } 
        
        if (flagTipoPessoa == "F") {
            $(".box-pf").removeClass("hide");
            $(".box-pj").addClass("hide");
            $("#Representante_Pessoa_nroDocumento").setMask("cpf");
            $(".info-documento").html("CPF");
        }
        
    }
    
};

var Representante = new ObjRepresentante();

$(document).ready(function () {
    Representante.init();
});
