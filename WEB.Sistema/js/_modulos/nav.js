function NavClass() {

    var keyNav = "lcNav";
    var keySelectedMenu = "lcMSel";

    //
    this.init = function () {

    };

    //Definir o conteudo de um menu
    this.criarMenu = function (menu) {
        localStorage.setItem(keyNav, menu);
    };


    //Inserir o html do menu na página
    this.iniciarMenu = function () {

        var htmlMenu = localStorage.getItem(keyNav);

        $("#boxMenu").html(htmlMenu);

        $(".sidebar .treeview").tree();

        this.marcarMenu();
    };


    //Excluir menu
    this.zerarMenu = function () {

        localStorage.setItem(keyNav, null);

        localStorage.setItem(keySelectedMenu, null);
    };


    //Salvar o último menu clicado para manter a marcacao e aberto se houver submenus
    this.selectMenu = function (elemento) {

        var idGrupo = $(elemento).data("id-grupo");

        localStorage.setItem(keySelectedMenu, idGrupo);
    }


    //Deixar o último menu visitado marcado
    this.marcarMenu = function () {

        var idGrupo = localStorage.getItem(keySelectedMenu);

        if (idGrupo === "null") {

            $("#menu_0").addClass("active");

            return;
        }

        var item = $("#menu_" + idGrupo);

        item.addClass("active");

        item.children('ul').slideToggle('fast');
    }
};

var Nav = new NavClass();

$(document).ready(function () {
    Nav.init();
});
