function MacroContaCentroCustoClass() {

    this.init = function () {

        this.listenerMarcarTodos();
    };

    //Ouvinte do evento click para o checkbox que marca/desmarca todas as opções
    this.listenerMarcarTodos = function() {

        $("#toggleCheckbox").on("click", function () {

            var elemento = $(this);

            if (elemento.is(":checked")) {
                $(".item-check").prop("checked", "checked");
            } else {
                $(".item-check").removeAttr("checked");
            }
        });
    }
}

var MacroContaCentroCusto = new MacroContaCentroCustoClass();

$(document).ready(function () {
    MacroContaCentroCusto.init();
});