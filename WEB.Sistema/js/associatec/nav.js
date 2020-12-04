function NavClass() {

    var keyNavMenuLateral = "lcNavsl_associatec";
    var keyNavMenuTopo = "lcNavTp_associatec";
    var keySelectedMenu = "lcMSelsl_associatec";
    this.flagMenuTopo = false;
    this.flagMenuLateral = false;

    //
    this.init = function () {
        $('.dropdown-menu .user-body').click(function (e) {
            e.stopPropagation();
        });
    };

    //Definir o conteudo de um menu
    this.criarMenuLateral = function (menu) {
        localStorage.setItem(keyNavMenuLateral, menu);
        Nav.flagMenuLateral = true;
    };

    //Definir o conteudo de um menu
    this.criarMenuTopo = function (menu) {
        localStorage.setItem(keyNavMenuTopo, menu);
        Nav.flagMenuTopo = true;
    };


    //Inserir o html do menu na pagina
    this.iniciarMenuLateral = function () {

        var htmlMenu = localStorage.getItem(keyNavMenuLateral);
        
        $("#boxMenu").html(htmlMenu);

        //Enable sidebar tree view controls
        $.AdminLTE.tree('.sidebar');

        var o = $.AdminLTE.options;

        //Enable control sidebar
        if (o.enableControlSidebar) {
            $.AdminLTE.controlSidebar.activate();
        }

        this.marcarMenu();

/*
        if (o.navbarMenuSlimscroll && typeof $.fn.slimscroll != 'undefined') {
            $(".navbar .menu").slimscroll({
                height: o.navbarMenuHeight,
                alwaysVisible: true,
                size: o.navbarMenuSlimscrollWidth
            }).css("width", "100%");
        }
*/

    };


    //Inserir o html do menu na pagina
    this.iniciarMenuTopo = function () {

        var htmlMenu = localStorage.getItem(keyNavMenuTopo);

        $("#boxMenuTopo").html(htmlMenu);

    };


    //Excluir menu
    this.zerarMenu = function () {

        localStorage.setItem(keyNavMenuLateral, null);

        localStorage.setItem(keyNavMenuTopo, null);

        localStorage.setItem(keySelectedMenu, null);
    };


    //Salvar o ultimo menu clicado para manter a marcacao e aberto se houver submenus
    this.selectMenu = function (elemento) {

        var idGrupo = $(elemento).data("id-grupo");

        localStorage.setItem(keySelectedMenu, idGrupo);
    };


    //Deixar o ultimo menu visitado marcado
    this.marcarMenu = function () {

        var idGrupo = localStorage.getItem(keySelectedMenu);

        if (idGrupo === "null") {

            $("#menu_0").addClass("active");

            return;
        }

        var item = $("#menu_" + idGrupo+" a");
        
        item.addClass("active");

        item.trigger('click');
    }
};

var Nav = new NavClass();

$(document).ready(function () {
    Nav.init();
});
