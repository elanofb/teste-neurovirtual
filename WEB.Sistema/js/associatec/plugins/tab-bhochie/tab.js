function TabBhochieClass() {

    var baseUrlGeral;

    this.init = function () {

        this.baseUrlGeral = $("#baseUrlGeral").val();

        this.ativarTabs();

    };

    this.ativarTabs = function () {

        $("div.bhoechie-tab-menu>div.list-group>a").click(function (e) {

            e.preventDefault();

            $(this).siblings('a.active').removeClass("active");

            $(this).addClass("active");

            var index = $(this).index();

            $("div.bhoechie-tab>div.bhoechie-tab-content").removeClass("active");

            $("div.bhoechie-tab>div.bhoechie-tab-content").eq(index).addClass("active");

        });


    }

};

var TabBhochie = new TabBhochieClass();

$(document).ready(function () {
    TabBhochie.init();
});
