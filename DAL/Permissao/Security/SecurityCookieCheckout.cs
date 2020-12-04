using System;
using System.Linq;
using System.Web;

namespace DAL.Permissao.Security {

	public class SecurityCookieCheckout {

        public static string idOrganizacao {
			get { return GetValue("checkidorgnass"); }
			set { SetValue("checkidorgnass", value, DateTime.Now.AddHours(1)); }
		}

        private static string GetValue(string key) {

            if (HttpContext.Current.Request.Cookies.AllKeys.Contains(key)) {

                HttpCookie cookie = HttpContext.Current.Request.Cookies.Get(key);

                return HttpUtility.UrlDecode(cookie?.Value ?? "");
			}

			return null;
		}

		private static void SetValue(string key, string value, DateTime expires) {

			var httpCookie = HttpContext.Current.Response.Cookies[key];

		    if (httpCookie != null){

		        httpCookie.Value = HttpUtility.UrlEncode(value ?? "");

		        httpCookie.Expires = expires;
		    }
            
		}

	}

}