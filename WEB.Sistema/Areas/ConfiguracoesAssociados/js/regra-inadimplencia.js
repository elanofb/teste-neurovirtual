function RegraInadimplenciaClass() {

    this.init = function () {
        $(".rotina-inadimplencia").change(function () {
            RegraInadimplencia.changeRotinaInadimplencia();
        });

        $(".todos-pagamentos").change(function () {
            RegraInadimplencia.changeTodosPagamentos();
        });
    };

    this.changeRotinaInadimplencia = function () {
        var select = $(".rotina-inadimplencia select").val();

        if (select == "True") {
            $(".todos-pagamentos").removeClass("hidden");
        } else {
            $(".todos-pagamentos").addClass("hidden");
            $(".qtde-ultimos-pagamentos").addClass("hidden");
        }
    }

    this.changeTodosPagamentos = function () {
        var select = $(".todos-pagamentos select").val();

        if (select == "False") {
            $(".qtde-ultimos-pagamentos").removeClass("hidden");
        } else {
            $(".qtde-ultimos-pagamentos").addClass("hidden");
        }
    }
}


var RegraInadimplencia = new RegraInadimplenciaClass();

$(document).ready(function () {
    RegraInadimplencia.init();
});