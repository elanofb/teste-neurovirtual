function ProdutoRedeExclusaoClass() {

    this.init = function () {
        
    };

    //Retorno após submissoes do form no modal
    this.excluir = function (elemento) {
        
        var fYes = function () {
            
            var id = $(elemento).data("id");

            var url = $(elemento).data("url");
            
            $.post(url, { 'id': id },
                
                function (response) {
                
                    if (response.error){
                        jM.error(response.message);
                        return;
                    }

                    DefaultSistema.carregarConteudoElemento("boxListaRede");
                    
                    toastr.success("O registro foi removido com sucesso!", "Operação realizada!");
                }
            );
            
        };
        
        var fNo = function () {
            return false;
        };
        
        jM.confirmation("Confirma a exclusão do registro?", fYes,  fNo);
    }
};

var ProdutoRedeExclusao = new ProdutoRedeExclusaoClass();

$(document).ready(function () {
    ProdutoRedeExclusao.init();
});
