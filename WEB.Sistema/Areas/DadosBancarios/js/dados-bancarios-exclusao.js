function DadosBancariosExclusaoClass() {

    var baseUrlGeral;

    this.init = function () {
        
        this.baseUrlGeral = $("#baseUrlGeral").val();
        
    };
    
    //
    this.excluir = function (element) {
        
        var funcYes = function () {
            
            try {
                
                var url = $(element).data("url");
                var id = $(element).data("id");

                var func = function (response) {
                    location.reload();
                };
                
                Ajax.init(url, { id: id }, func, null, 'post', 'json');

            } catch (e) {
                console.log(e);
            }

        };
        
        jM.confirmation("Voc&ecirc; deseja remover a conta selecionada?", funcYes, null);

    }
   


};

var DadosBancariosExclusao = new DadosBancariosExclusaoClass();

$(document).ready(function () {
    DadosBancariosExclusao.init();
});
