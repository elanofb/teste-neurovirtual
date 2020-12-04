using System.Configuration;

namespace Textos {

    public static class RsIntitucionalAreaAssociado {

		public static string textoRedeSocial => ConfigurationManager.AppSettings["textoRedeSocial"];
		public static string textoBeneficios => ConfigurationManager.AppSettings["textoNovoAssociadoBeneficios"];
    }
}