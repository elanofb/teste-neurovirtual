using WEB.Areas.AssociadosDependentes.ViewModels;

namespace WEB.Areas.Associados.Extensions {

    public static class ItemListaDependenteExtensions {


        //Status Dependente
        public static string exibirStatus(this ItemListaDependente ODependente) {

			string descricaoAtivo = "Desativado";

            switch (ODependente.ativo) {

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

        //
		public static string exibirIconeStatus(this ItemListaDependente OAssociado) {

			string descricaoAtivo = (OAssociado.ativo == "E"? "fa-clipboard": (OAssociado.ativo == "S"? "fa-check": "fa-times"));

            return descricaoAtivo;
        }

        //
		public static string exibirClasseStatus(this ItemListaDependente ODependente) {

			string descricaoAtivo = (ODependente.ativo == "E"? "text-yellow" : (ODependente.ativo == "S"? "text-green": "text-red"));

            return descricaoAtivo;
        }

    }
}