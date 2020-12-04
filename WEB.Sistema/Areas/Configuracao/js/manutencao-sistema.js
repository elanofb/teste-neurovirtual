function ManutencaoSistemaClass() {
    
    //Inicializador de metodos
    this.init = function() {

    };

    //Funcao para remover os caches de dados
    this.removerCacheDados = function () {
        var url = new String($("#baseUrlGeral").val()).concat("configuracao/operacoes/limpar-cache-dados");

        $.post(url, {},
            function (response) {
                if (response.error == false) {
                    jM.success(response.message);
                }
            }
        );
    };
}

var ManutencaoSistema = new ManutencaoSistemaClass();

$(document).ready(function () {

    ManutencaoSistema.init();
});