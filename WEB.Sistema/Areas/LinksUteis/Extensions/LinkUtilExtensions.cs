using DAL.LinksUteis;

namespace WEB.Areas.LinksUteis.Extensions{
    public static class LinkUtilExtensions {

        //Status Veiculo
        public static string exibirStatus(this LinkUtil OLinkUtil) {

			string descricaoAtivo = "Desativado";

            switch (OLinkUtil.ativo){
                case true:
                    descricaoAtivo = "Ativo";
                    break;
            }


            return descricaoAtivo;
        }
        
        //
        public static string exibirClasseStatus(this LinkUtil OLinkUtil) {

			string descricaoAtivo = (OLinkUtil.ativo == true ? "text-green": "text-red");

            return descricaoAtivo;
        }
        
        //Status Veiculo
        public static string exibirIconeStatus(this LinkUtil OLinkUtil) {

			string descricaoAtivo = (OLinkUtil.ativo == true ? "fa-check" : "fa-times");

            return descricaoAtivo;
        }
    }
}