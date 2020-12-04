using System.Configuration;

namespace Textos {

    public static class RsNaoAssociado {

		public static string labelBoxCadastroNaoSocio => ConfigurationManager.AppSettings["labelBoxCadastroNaoSocio"];        

    }
}