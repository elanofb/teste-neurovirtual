using System;
using System.Linq;
using System.Web;

namespace DAL.Permissao.Security {

	public class SecurityCookie {

		public static string userId {
			get { return GetValue("suixass"); }
			set { SetValue("suixass", value, DateTime.Now.AddHours(12)); }
		}

		public static string idPerfil {
			get { return GetValue("suipass"); }
			set { SetValue("suipass", value, DateTime.Now.AddHours(12)); }
		}
		public static string descricaoPerfil {
			get { return GetValue("sdpcass"); }
			set { SetValue("sdpcass", value, DateTime.Now.AddHours(12)); }
		}

		public static string userName {
			get { return GetValue("sunxass"); }
			set { SetValue("sunxass", value, DateTime.Now.AddHours(12)); }
		}

		public static string userEmail {
			get { return GetValue("sumxass"); }
			set { SetValue("sumxass", value, DateTime.Now.AddHours(12)); }
		}

		public static string dtCadastro {
			get { return GetValue("sdtcaxass"); }
			set { SetValue("sdtcaxass", value, DateTime.Now.AddHours(12)); }
		}

		public static string flagAlterarSenha {
			get { return GetValue("sfasass"); }
			set { SetValue("sfasass", value, DateTime.Now.AddHours(12)); }
		}

		public static string flagCliente {
			get { return GetValue("sflcliass"); }
			set { SetValue("sflcliass", value, DateTime.Now.AddHours(12)); }
		}

		public static string flagTodasUnidades {
			get { return GetValue("sflalluniass"); }
			set { SetValue("sflalluniass", value, DateTime.Now.AddHours(12)); }
		}

		public static string flagOrganizacao {
			get { return GetValue("sflallorgsns"); }
			set { SetValue("sflallorgsns", value, DateTime.Now.AddHours(12)); }
		}

        public static string flagSomenteCadastroProprio  {
			get { return GetValue("sflonselfass"); }
			set { SetValue("sflonselfass", value, DateTime.Now.AddHours(12)); }
		}

		public static string idUnidade {
			get { return GetValue("siduniass"); }
			set { SetValue("siduniass", value, DateTime.Now.AddHours(12)); }
		}

		public static string nomeUnidade {
			get { return GetValue("snomuniass"); }
			set { SetValue("snomuniass", value, DateTime.Now.AddHours(12)); }
		}

		public static string idOrganizacao {
			get { return GetValue("sidorgnass"); }
			set { SetValue("sidorgnass", value, DateTime.Now.AddDays(5)); }
		}

		public static string nomeOrganizacao {
			get { return GetValue("snomorgnass"); }
			set { SetValue("snomorgnass", value, DateTime.Now.AddHours(12)); }
		}

        public static string idsClientes {
            get { return GetValue("iedecdlciaass"); }
            set { SetValue("iedecdlciaass", value, DateTime.Now.AddHours(12)); }
        }

        public static string idsLojas {
			get { return GetValue("iljsaceass"); }
			set { SetValue("iljsaceass", value, DateTime.Now.AddHours(12)); }
		}

        public static string idsUnidades {
            get { return GetValue("iuusheass"); }
            set { SetValue("iuusheass", value, DateTime.Now.AddHours(12)); }
        }

        public static string idsOrganizacoes {
            get { return GetValue("sdkfjhdsf"); }
            set { SetValue("sdkfjhdsf", value, DateTime.Now.AddHours(12)); }
        }

        public static string latitude {
            get { return GetValue("lgaetaiass"); }
            set { SetValue("lgaetaiass", value, DateTime.Now.AddHours(12)); }
        }

        public static string longitude {
            get { return GetValue("leonegbass"); }
            set { SetValue("leonegbass", value, DateTime.Now.AddHours(12)); }
        }

        private static string GetValue(string key) {

            if (HttpContextFactory.Current.Request.Cookies.AllKeys.Contains(key)) {

                HttpCookie cookie = HttpContextFactory.Current.Request.Cookies.Get(key);

                return HttpUtility.UrlDecode(cookie?.Value ?? "");
			}

			return null;
		}

		private static void SetValue(string key, string value, DateTime expires) {

			var httpCookie = HttpContextFactory.Current.Response.Cookies[key];

            if (httpCookie != null) {

                httpCookie.Value = HttpUtility.UrlEncode(value ?? "");

                httpCookie.Expires = expires;

            }

		}

	}

}