function ContribuicaoPainelClass() {


    //Metodo de inicializacao dos plugins
    this.init = function () {

        this.listenerComboVencimentos();

    };

    //Ouvinte do combo de vencimentos da contribuição
    this.listenerComboVencimentos = function () {

        $("#dtVencimentos").multiselect();

    };

    //Exibir o menu de context com as ações de cada bloco
    this.toogleMenuAcoes = function (elemento) {

        var divWrapper = $(elemento).parent(".dropdown-custom");

        var subMenu = $(divWrapper).find("div.dropdown-content");

        var flagIsVisible = subMenu.is(":visible");

        $("div.dropdown-content").removeClass("active-menu");

        if (!flagIsVisible) {

            subMenu.addClass("active-menu");

        }
    }



};

var ContribuicaoPainel = new ContribuicaoPainelClass();


$(document).ready(function () {

    ContribuicaoPainel.init();

});
