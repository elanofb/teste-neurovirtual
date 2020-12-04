function AssociadoPreAtualizacaoCadastroClass(){
    
    //Metodo de inicializacao dos plugins
    this.init = function () {
        
    };
    
    //Evento Pos-Submit do cadastro de associado
	this.onSuccessForm = function (response) {
        
	    if (response.error == false) {

            AssociadoPreAtualizacaoCadastro.finalizarAnalise(response.id, true);
            
            return;
        }
        
	    /*$("#tab-dados-cadastrais input").setMask();
        
	    $("#tab-dados-cadastrais").find(".link-loading").on('click', function () {

	        var btn = $(this).button().data('loading-text', 'Processando...');

	        btn.button('loading');

	    });
        
	    DefaultSistema.zerarErros();*/
	    
	    jM.error(response.message);
        
        DefaultSistema.reiniciarBotao();

	};
    
    this.finalizarAnalise = function (idAssociado, flagAprovado) {
        
        var url = $("#baseUrlGeral").val() + "Associados/PreAtualizacaoFinalizacao/finalizar-analise";
        
        $.post(url, { idAssociado : idAssociado, flagAprovado : flagAprovado }, function (response) {
            
            if (response.error == true){
                                    
                jM.error(response.message);
                DefaultSistema.reiniciarBotao();
                
                return;
            }
            
            if (response.urlRedirecionamento != "" && response.urlRedirecionamento != null){
                location.href = response.urlRedirecionamento;
                return;
            }
            
            jM.success(response.message);
            return;
            
        });

    };

    this.reprovarAtualizacoes = function (elem) {
        
        var url = $(elem).data("url");

        if ($('input.idsHistoricoAtualizacao:checked').length <= 0) {
            toastr.options.positionClass = "toast-top-right";
            toastr.error("Selecione pelo menos um item para descartar!", 'Erro!', { timeOut: "8000" });
            return;
        }

        var itens = new Array;
        
        $('input.idsHistoricoAtualizacao:checked').each(function () {
            itens.push(this.value);
        });

        var dadosIds = {
            ids: itens
        };
        
        var funcYes = function () {
            $.post(url, dadosIds, function (response) {

                if (response.error == true){

                    jM.error(response.message);
                    DefaultSistema.reiniciarBotao();

                    return;
                }

                if (response.urlRedirecionamento != "" && response.urlRedirecionamento != null){
                    location.href = response.urlRedirecionamento;
                    return;
                }

                jM.success(response.message);
                return;

            }); 
        };
        
        jM.confirmation("Deseja reprovar as atualizações selecionadas?", funcYes);        
        
    };
};

var AssociadoPreAtualizacaoCadastro = new AssociadoPreAtualizacaoCadastroClass();

$(document).ready(function(){

    AssociadoPreAtualizacaoCadastro.init();

});
