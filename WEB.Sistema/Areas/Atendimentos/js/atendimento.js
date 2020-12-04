function AtendimentoClass() {

    var baseUrl;

    var id;

    this.init = function () {

        this.baseUrl = $("#baseUrlGeral").val();

    }

    this.iniciarAtendimento = function (id) {

        this.id = id;

        jM.confirmation("Voc&ecirc; deseja iniciar o atendimento?", Atendimento.funcYes);

    }

    this.retomarAtendimento = function (id) {

        this.id = id;

        jM.confirmation("Voc&ecirc; deseja retomar o atendimento?", Atendimento.funcYes);

    }

    this.funcYes = function () {

        var url = Atendimento.baseUrl + "Atendimentos/AtendimentoAcaoInicio/iniciar-atendimento";

        var dados = { id: Atendimento.id };

        $.post(url, dados, function (response) {

            if (response.error == true) {

                jM.error(response.message);

                return;

            }
            
            location.href = response.urlRedirect;

        });

    }

}

var Atendimento = new AtendimentoClass();

$(document).ready(function () {

    Atendimento.init();

});