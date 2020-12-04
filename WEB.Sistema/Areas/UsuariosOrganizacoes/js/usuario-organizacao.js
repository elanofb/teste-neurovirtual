function UsuarioOrganizacaoClass() {

    var idBoxOrganizacaos = "#boxOrganizacoesVinculadas";

    this.init = function () {
    };

    this.vincularOrganizacao = function (elemento) {

        var idOrganizacao = $("#idOrganizacaoVinculo").val();
        var idUsuario = $(elemento).data("idusuario");

        var url = $(elemento).data("url");

        $.post(url, { idUsuario: idUsuario, idOrganizacao: idOrganizacao }, function (response) {
            $(idBoxOrganizacaos).html(response);

            DefaultSistema.reiniciarBotao();
        });

    };

    this.excluirOrganizacaoVinculada = function (elemento) {

        var id = $(elemento).data("id");
        var idUsuario = $(elemento).data("idusuario");
        
        var url = $(elemento).data("url");

        jM.confirmation("Tem certeza que deseja desvincular a organiza&ccedil;&aacute;o?", function () {

            $.post(url, {id : id , idUsuario: idUsuario }, function (response) {
                $(idBoxOrganizacaos).html(response);
            });

        });
    };

};

var UsuarioOrganizacao = new UsuarioOrganizacaoClass();
$(document).ready(function(){
    UsuarioOrganizacao.init();
});