function AtendimentoCadastroClass() {

    var baseUrl;

    this.init = function () {
        
        this.baseUrl = $("#baseUrlGeral").val();

        AtendimentoCadastro.iniciarAutoCompleteAssociado();
        AtendimentoCadastro.mudarForm();

    }
    
    this.mudarForm = function () {
        $("#flagTipoPessoa").change(function () {
            if ($(this).val() == "J"){
                $("#boxDocumentoPF").addClass("hide");
                $("#boxDocumentoPJ").removeClass("hide");
            }else{
                $("#boxDocumentoPF").removeClass("hide");
                $("#boxDocumentoPJ").addClass("hide");
            }
        });
    }
    
    //Busca por membros
    this.iniciarAutoCompleteAssociado = function () {
        
        AppAutoComplete.title = "Selecione...";

        AppAutoComplete.url = AtendimentoCadastro.baseUrl + "Pessoas/PessoaAutoComplete/listar-json-associados-nao-associados";

        AppAutoComplete.quantityItems = 2;

        var combo = $("#idPessoaAtendimento");

        console.log(combo);

        AppAutoComplete.loadSelect2(combo);

    }

    this.carregarDadosAssociado = function () {
        
        var id = $("#idPessoaAtendimento").val();

        if (!id > 0) {

            toastr.options.positionClass = "toast-top-right";

            toastr.error("Para preencher os dados automaticamente voc&ecirc; deve selecionar um associado ou não associado.", 'Erro!', { timeOut: "5000" });

            DefaultSistema.reiniciarBotao();

            return;

        }

        var url = this.baseUrl + "Pessoas/PessoaAutoComplete/carregar-associados-nao-associados";
        
        $.get(url, { id : id }, function (response) {
            
            DefaultSistema.reiniciarBotao();
            
            if (response.error == true) {

                toastr.options.positionClass = "toast-top-right";
                
                toastr.error(response.message, 'Erro!', { timeOut: "5000" });

                return;

            }
            
            if (response.DadosPessoa.flagCategoriaPessoa == "AS"){
                $("#idAssociado").val(response.DadosPessoa.idReferencia);
                $("#idNaoAssociado").val("");
            }

            if (response.DadosPessoa.flagCategoriaPessoa == "NAS"){
                $("#idNaoAssociado").val(response.DadosPessoa.idReferencia);
                $("#idAssociado").val("");
            }
            
            $("#nome").val(response.DadosPessoa.nome);
            
            $("#emailPrincipal").val(response.DadosPessoa.email);

            $("#telPrincipal").val(response.DadosPessoa.telefone);
            
            var tipoPessoa = response.DadosPessoa.flagTipoPessoa;
            
            if (tipoPessoa == "F"){
                $("#boxDocumentoPF").removeClass("hide");
                $("#boxDocumentoPJ").addClass("hide");
                $("#documentoPF").val(response.DadosPessoa.nroDocumento);
            }
            
            if (tipoPessoa == "J"){
                $("#boxDocumentoPF").removeClass("hide");
                $("#boxDocumentoPJ").addClass("hide");
                $("#documentoPJ").val(response.DadosPessoa.nroDocumento);
            }
            
            $("#flagTipoPessoa").val(tipoPessoa);
            
            $('input:text').setMask();

        });

    }

};


var AtendimentoCadastro = new AtendimentoCadastroClass();

$(document).ready(function () {
    AtendimentoCadastro.init();
});
