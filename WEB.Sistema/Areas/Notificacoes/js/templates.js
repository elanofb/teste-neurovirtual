function TemplatesClass() {

    this.init = function () {

    };

    //Altera o status do template
    this.alterarStatus = function (url, status) {

        var fYes = function () {

            $.post(url, {}, function (response) {

                if (response.error == true){

                    jM.error("Erro ao alterar o status do template.");

                }

                var func = function(){
                    location.reload();
                }

                jM.success("Template " + (status == "D" ? "desativado" : "ativado") + " com sucesso!", func);

            });

        };
        
        jM.confirmation("Deseja realmente " + (status == "D" ? "desativar" : "ativar") + " o template?", fYes);

    }

}

var Templates = new TemplatesClass();

$(document).ready(function () {
    Templates.init();  
});

