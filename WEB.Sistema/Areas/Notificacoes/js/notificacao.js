function NotificacaoClass() {

    this.init = function () {

        this.carregarWidgetsTopo();

    };


    //Carregar os dados widgets do topo
    this.carregarWidgetsTopo = function () {

        var idBoxTarefa = $("#widgetTarefas");

        var urlTarefa = idBoxTarefa.data("url");

        $.get(urlTarefa, {}, function (response) {
            idBoxTarefa.html(response);
        });

        var idBoxAvisos = $("#widgetNotificacoes");

        var urlAvisos = idBoxAvisos.data("url");

        $.get(urlAvisos, {}, function (response) {
            idBoxAvisos.html(response);
        });

    }

    //Registrar a leitura do aviso
    this.registrarLeitura = function (id) {

        var fYes = function () {

            var idBoxAvisos = $("#widgetNotificacoes");

            var url = $("#baseUrlGeral").val() + 'notificacoes/NotificacaoWidget/registrar-leitura';

            $.post(url, { 'id': id }, function (response) {

                idBoxAvisos.html(response);

            });

        };

        //
        var fNo = function () {
            return false;
        };

        jM.confirmation("A mensagem não será mais exibida. Confirma a leitura?", fYes, fNo);
       
    };

}

var Notificacao = new NotificacaoClass();

$(document).ready(function () {
    Notificacao.init();  
});

