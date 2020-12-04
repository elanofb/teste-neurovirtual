function AssociacaoModulosContratadosClass() {

    var baseUrl;

    this.init = function () {

        this.baseUrl = $("#baseUrlGeral").val();

        DefaultSistema.carregarConteudo($("#boxModulosContratados"), this.iniciarPluginCheckBox);
        
    }

    this.iniciarPluginCheckBox = function () {

        $(".check-toggle").bootstrapToggle({

            on: "Desativar",
            off: "Ativar",
            onstyle: "success",
            offstyle: "danger",
            width: "100px"


        });

        $(".check-toggle").on("change", function () {

            $("#boxModulosContratados").loadingOverlay();

            var idOrganizacao = $(this).data("idorganizacao");

            var idRecursoGrupo = $(this).data("idrecursogrupo");

            var flagAtivar = $(this).prop("checked");
            
            var url = AssociacaoModulosContratados.baseUrl + "Associacoes/AssociacaoModulosContratados/registrar-modulo";
            
            $.post(url, { idOrganizacao : idOrganizacao, idRecursoGrupo : idRecursoGrupo, flagAtivar : flagAtivar }, function (response) {

                DefaultSistema.carregarConteudo($("#boxModulosContratados"), AssociacaoModulosContratados.iniciarPluginCheckBox);

                $("#boxModulosContratados").loadingOverlay('remove');

            });

        });

    }

}

var AssociacaoModulosContratados = new AssociacaoModulosContratadosClass();

$(document).ready(function () {

    AssociacaoModulosContratados.init();

})