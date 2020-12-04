using System.Configuration;
using System.Web.UI.WebControls;

namespace System {

    public static class UtilConfig {

        //App
        public static string conexaoDBAdm => ConfigurationManager.ConnectionStrings["STAdmConnection"].ToString();
        public static string flagProducao => ConfigurationManager.AppSettings["flagProducao"];
        public static string flagAmbiente => ConfigurationManager.AppSettings["flagAmbiente"];

        public static bool emProducao() {
            return (flagProducao == "S");
        }

        //Paths
        public static string pathAbsTempFiles => ConfigurationManager.AppSettings["pathAbsTempFiles"];

        public static string pathAbsRaiz => HttpContextFactory.Current.Server.MapPath("~");

        public static string pathAbsUploadFiles => HttpContextFactory.Current.Server.MapPath("~/upload/");

        public static string scheme => HttpContextFactory.Current.Request.Url?.Scheme;

        public static string host => HttpContextFactory.Current.Request.Url?.Host;

        public static int? port => HttpContextFactory.Current.Request.Url?.Port;

        public static string pathOrganizacao(int idOrganizacao){

            string pasta = $"organizacao_{idOrganizacao.ToString().PadLeft(15, '0')}";

            return pasta;
        }

        public static string pathAbsUpload(int idOrganizacao) {

            if (idOrganizacao == 0){

                return UtilConfig.pathAbsUploadFiles;
            }

            string url = $"{UtilConfig.pathAbsRaiz}upload/{pathOrganizacao(idOrganizacao)}/";

            return url;
        }


        //Links
        public static string linkAbsSistemaUpload(int idOrganizacao) {

            string url = $"{UtilConfig.linkAbsSistema}upload/{pathOrganizacao(idOrganizacao)}/";

            return url;
        }

        public static string linkAbsSistema => $"{scheme ?? "http"}://{host}{ (port > 0? (":"+port): "") }/";

        public static string linkAbsAreaAssociado => $"{(UtilConfig.emProducao()? "https": "http")}://{ConfigurationManager.AppSettings["linkAreaAssociado"]}";

        public static string linkPgto =>  $"{(UtilConfig.emProducao()? "https": "http")}://{HttpContextFactory.Current.Request.Url?.Host}/AreaCheckout/Checkout/InicioPagamento/preparar?";

        public static string linkAbsLogErro => ConfigurationManager.AppSettings["linkAbsLogErro"];



        /// <summary>
        /// Gerar link de pagamento checkout a partir do título receita de pagamento
        /// </summary>
        public static string linkPgtoTitulo(int id){

            return string.Concat(linkPgto, "t=", UtilCrypt.toBase64Encode(id));

        }
        
        /// <summary>
        /// Gerar link de pagamento checkout a partir da parcela de pagamento
        /// </summary>
        public static string linkPgtoParcela(int id){

            return string.Concat(linkPgto, "p=", UtilCrypt.toBase64Encode(id));

        }

        public static string linkResourses(string caminho = "") {

            var linkRetorno = "";

	        switch (flagAmbiente) {
                case "P":
                    linkRetorno = linkAbsSistema;//"https://cdn.associatec.com.br/";
                    break;
                case "H":
                    linkRetorno = linkAbsSistema;//"http://cdn.demonstracao.associatec.com.br/";
                    break;
                default:
                    linkRetorno = linkAbsSistema;
                    break;
	        }

            if(!caminho.isEmpty()) {
                linkRetorno = String.Concat(linkRetorno, caminho);
            }

            return linkRetorno;
	    }
    }
}