using System.Configuration;

namespace Textos {

    public static class RsCheckout {

		public static string padraoTitulo => ConfigurationManager.AppSettings["padraoTitulo"];
        public static string padraoMsgNroAcompanhamento => ConfigurationManager.AppSettings["padraoMsgNroAcompanhamento"];

        public static string boletoTitulo => ConfigurationManager.AppSettings["boletoTitulo"];
		public static string boletoMsgNroAcompanhamento => ConfigurationManager.AppSettings["boletoMsgNroAcompanhamento"];
		public static string boletoMsgNroAcompanhamentoObservacao => ConfigurationManager.AppSettings["boletoMsgNroAcompanhamentoObservacao"];               
    }
}