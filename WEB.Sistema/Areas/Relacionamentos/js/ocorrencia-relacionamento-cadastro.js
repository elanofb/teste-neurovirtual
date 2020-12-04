function OcorrenciaRelacionamentoCadastroClass() {
    
    // É possível receber alguns parametros de retorno em caso de sucesso: 
    // - flagRecarregar: define se a página será recarregada
    // - idComboSelecionar: id da combo que receberá a nova opção de ocorrência cadastrada
    // - id: value da opção da combo que será adicionada
    // - descricao: texto da opção da combo que será adicionada
    this.onSuccessForm = function (response) {
        
        if (response.error == false) {

            $("#modalFormOcorrenciaRelacionamento").modal('toggle');
            
            if (response.flagRecarregar == true) {
                location.reload();
            }
            
            if (response.flagRecarregar == false){
                OcorrenciaRelacionamentoCadastro.preencherCombos(response);
            }
            
            return false;
            
        }
        
        DefaultSistema.iniciarPluginsAposAjax($("#modalFormOcorrenciaRelacionamento"));
        
    }
    
    this.preencherCombos = function (response) {

        var combos = $(".combo-ocorrencia-relacionamento");

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
    
    this.addOptionsUpload = function(){

        var nroCamposUpload = $(".input-file").length;
        
        var uploadPrincipal = "<label>Arquivo</label><div><input type=\"file\" class=\"input-file file\" name=\"Arquivos["+nroCamposUpload+"].FileUpload\" data-show-upload=\"false\" data-show-preview=\"false\" data-show-remove=\"false\" data-preview-settings=\"[width:'auto', height:'100px']\" data-browse-label=\"Procurar ...\" data-remove-label=\"Remover\" data-show-caption=\"true\" data-preview-file-type=\"image\" /></div>";
        var legendaPrincipal = "<label>Descrição do Arquivo</label><div><input class=\"arquivo-legenda form-control\" maxlength=\"100\" name=\"Arquivos["+nroCamposUpload+"].lengenda\" type=\"text\" value=\"\"></div>";
        
        $("#boxFilesUpload").append("<div class=\"col-md-6\">"+ legendaPrincipal +"</div>");
        $("#boxFilesUpload").append("<div class=\"col-md-6\">"+ uploadPrincipal +"</div>");

        DefaultSistema.iniciarPluginsAposAjax($("#boxFilesUpload"));
    }

    //Upload de arquivos assincrono
    this.salvarOcorrencia = function (form) {

        var form = $("#formHistorico");
        var urlUpload = form.attr("action");

        var data = new FormData(form[0]);
        
        $.each($(".input-file"), function(i, obj) {
            $.each(obj.files,function(j, file){
                data.append('Arquivos['+j+'].FileUpload', file);
            })
        });
        
        var ajaxRequest = $.ajax({
            type: "POST",
            url: urlUpload,
            contentType: false,
            enctype: 'multipart/form-data',
            processData: false,
            data: data,
            success: function (response) {
                
                if (response.flagRecarregar == true) {
                    location.reload();
                }

                $("#boxFormHistorico").html(response);
                DefaultSistema.iniciarBotoes(".modal-dialog");
                DefaultSistema.iniciarPluginsAposAjax($("#boxFormHistorico"));
            },
            error: function (error) {
                console.log(error);

            }
        });
    }
    
}

var OcorrenciaRelacionamentoCadastro = new OcorrenciaRelacionamentoCadastroClass();