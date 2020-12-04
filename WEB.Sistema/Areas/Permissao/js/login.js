function LoginClass() {

    //
    this.init = function () {
        Nav.zerarMenu();
        Login.carregarLocalizacao()
    };

    //Onbegin no envio do form de login
    this.onBeginForm = function (response) {

        $(".login-processando").show();

        $(".login-processando label").html("Validando acesso...").show();

    };

    //Onsuccess no envio do form de login
    this.onSuccessForm = function (response) {

        if (response.error !== false) {

            Login.reiniciarBotao();

            $(".login-processando").hide();

            return;
        }

        $(".login-processando label").html("Login validado com sucesso...");

        CarregadorPermissao.carregarPermissoes();

        CarregadorPermissao.carregarPermissoesTopo();

        CarregadorPermissao.redirecionar(response.urlRedirecionamento);

    };

    //Restart da posi��o dos botoes
    this.reiniciarBotao = function () {
        setTimeout(function () {
            $(".link-loading").button('reset');
            $(".link-loading").on('click', function () {
                var btn = $(this).button().data('loading-text', 'Processando...');
                btn.button('loading');
            });
        }, 500);
    }

    this.carregarLocalizacao = function(){
        if (!navigator.geolocation) {
            return;
        }

        navigator.geolocation.getCurrentPosition(function (posicao) {
            $("#latitude").val(posicao.coords.latitude);
            $("#longitude").val(posicao.coords.longitude);
        });
    }
};

var Login = new LoginClass();

$(document).ready(function () {
    Login.init();
});