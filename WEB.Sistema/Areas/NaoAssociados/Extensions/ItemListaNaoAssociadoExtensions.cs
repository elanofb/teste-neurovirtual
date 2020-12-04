using WEB.Areas.NaoAssociados.ViewModels;

namespace WEB.Areas.NaoAssociados.Extensions {

    public static class ItemListaNaoAssociadoExtensions {


        //
        public static string exibirIconeStatus(this ItemListaNaoAssociado OItem) {

            string descricaoAtivo = (OItem.ativo == "E" ? "fa-clipboard" : (OItem.ativo == "S" ? "fa-check" : "fa-times"));

            return descricaoAtivo;
        }

        //
        public static string exibirClasseStatus(this ItemListaNaoAssociado OItem) {

            string descricaoAtivo = (OItem.ativo == "E" ? "text-yellow" : (OItem.ativo == "S" ? "text-green" : "text-red"));

            return descricaoAtivo;
        }

        //Status Não Associado
        public static string exibirStatus(this ItemListaNaoAssociado OItem) {

            string descricaoAtivo = "Desativado";

            switch (OItem.ativo) {
                case "E":
                    descricaoAtivo = "Em admissão";
                    break;

                case "S":
                    descricaoAtivo = "Ativo";
                    break;

                case "B":
                    descricaoAtivo = "Bloqueado";
                    break;
            }


            return descricaoAtivo;
        }
    }
}