using System.Configuration;

namespace Textos {

    public static class RsAplicacao {

		public static string nomeOrganizacao => ConfigurationManager.AppSettings["nomeOrganizacao"];

		public static string siglaOrganizacao => ConfigurationManager.AppSettings["siglaOrganizacao"];

    }
}