function CarregadorPermissaoClass() {

    //
    this.init = function () {
    };
    
    //Redirecionar
    this.redirecionar = function(urlRedirecionamento) {

        var timer = null;

        timer = window.setInterval(function () {

            if (Nav.flagMenuTopo == true && Nav.flagMenuLateral) {

                console.log("redirecionamento de usuario...");

                window.clearInterval(timer);

                if(urlRedirecionamento == '#'){
                    location.reload(true);                    
                }else{
                    location.href = urlRedirecionamento;
                }

                return;
            }
            console.log("Aguardando autorizacao de modulos...");

        }, 1000);        
    };

    //Carregar as opcoes para as quais o usu�rio tem acesso
    this.carregarPermissoes = function() {

        var url = new String($("#baseUrlGeral").val()).concat("permissao/menu/exibir-menu-principal");

        $(".login-processando label").html("Carregando permiss&otilde;es...");

        $.get(url, {}, function (response) {
            
            Nav.criarMenuLateral(response);

        });

    }

    //Carregar as opcoes para as quais o usu�rio tem acesso
    this.carregarPermissoesTopo = function () {

        var url = new String($("#baseUrlGeral").val()).concat("permissao/menu/exibir-menu-topo");

        $(".login-processando label").html("Liberando menus e m&oacute;dulos...");

        $.get(url, {}, function (response) {

            Nav.criarMenuTopo(response);

            return;

        });

    }


};

var CarregadorPermissao = new CarregadorPermissaoClass();

$(document).ready(function () {
    CarregadorPermissao.init();
});