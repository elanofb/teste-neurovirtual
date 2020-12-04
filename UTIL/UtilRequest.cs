using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace System {

	public static class UtilRequest {

		//

		//
		public static string getQueryString() {
			return HttpContext.Current.Request.Url.Query;
		}

		//
		public static string getString(string key) {
			string value = "";
			if (HttpContext.Current.Request[key] != null) value = HttpContext.Current.Request[key];
			return HttpUtility.UrlDecode(value);
		}
		
		//
		public static byte getByte(string key) {
			
			byte value = 0;
			
			if (HttpContext.Current.Request[key] != null && !String.IsNullOrEmpty(HttpContext.Current.Request[key])) {
				
				byte.TryParse(HttpContext.Current.Request[key], out value);
			}
			
			return value;
		}
		
		//
		public static Int32 getInt32(string key) {
			Int32 value = 0;
			if (HttpContext.Current.Request[key] != null && !String.IsNullOrEmpty(HttpContext.Current.Request[key])) {
				Int32.TryParse(HttpContext.Current.Request[key], out value);
			};
			return value;
		}

		//
		public static decimal getDecimal(string key) {
			decimal value = 0;
			
			if (HttpContext.Current.Request[key] != null && !String.IsNullOrEmpty(HttpContext.Current.Request[key])) {
				
				decimal.TryParse(HttpContext.Current.Request[key], out value);
				
			}
			
			return value;
		}

		//
		public static bool? getBool(string key) {
			if (HttpContext.Current.Request[key] != null && !HttpContext.Current.Request[key].isEmpty()) {
				bool value;
				Boolean.TryParse(HttpContext.Current.Request[key], out value);
				return value;
			};
			return null;
		}

		//
		public static Int32 getNroPagina() {
			Int32 value = 1;
			if (HttpContext.Current.Request["nroPagina"] != null && !String.IsNullOrEmpty(HttpContext.Current.Request["nroPagina"])) {
				Int32.TryParse(HttpContext.Current.Request["nroPagina"], out value);
			};
			return value;
		}

		//
		public static Int32 getNroRegistros() {
			Int32 value = 20;
			if (HttpContext.Current.Request["nroRegistros"] != null && !String.IsNullOrEmpty(HttpContext.Current.Request["nroRegistros"])) {
				Int32.TryParse(HttpContext.Current.Request["nroRegistros"], out value);
			};
			return value;
		}

		//
		public static DateTime? getDateTime(string key) {
			DateTime value = DateTime.MinValue;
			if (!String.IsNullOrEmpty(HttpContext.Current.Request[key])) {
				DateTime.TryParse(HttpContext.Current.Request[key], out value);
			};

			if (value == DateTime.MinValue) {
				return null;
			}
			return value;
		}
		

		/// <summary>
		/// 
		/// </summary>
		public static TimeSpan? getTime(string key) {
			
			TimeSpan value = TimeSpan.MinValue;
			
			if (!String.IsNullOrEmpty(HttpContext.Current.Request[key])) {
				TimeSpan.TryParse(HttpContext.Current.Request[key], out value);
			};

			if (value == TimeSpan.MinValue) {
				return null;
			}
			
			return value;
		}		

		//
		public static List<int> getListInt(string key) {
			List<int> retorno = new List<int>();

            if (!String.IsNullOrEmpty(HttpContext.Current.Request[key])) {
                string[] values = HttpContext.Current.Request[key].Split(',');
                retorno = values.Select(n => Convert.ToInt32(n)).ToList();
            }

            if (!String.IsNullOrEmpty(HttpContext.Current.Request[key + "[]"])) {
                string[] values = HttpContext.Current.Request[key + "[]"].Split(',');
                retorno = values.Select(n => Convert.ToInt32(n)).ToList();
            }

            return retorno;
        }
		
		//
		public static List<long> getListLong(string key) {
			
			List<long> retorno = new List<long>();

			if (!String.IsNullOrEmpty(HttpContext.Current.Request[key])) {
				string[] values = HttpContext.Current.Request[key].Split(',');
				retorno = values.Select(n => Convert.ToInt64(n)).ToList();
			}

			if (!String.IsNullOrEmpty(HttpContext.Current.Request[key + "[]"])) {
				string[] values = HttpContext.Current.Request[key + "[]"].Split(',');
				retorno = values.Select(n => Convert.ToInt64(n)).ToList();
			}

			return retorno;
		}		

		//
		public static List<string> getListString(string key) {
			List<string> retorno = new List<string>();

            if (!String.IsNullOrEmpty(HttpContext.Current.Request[key])) {
                string[] values = HttpContext.Current.Request[key].Split(',');
                retorno = values.Select(n => n).ToList();
            }

            if (!String.IsNullOrEmpty(HttpContext.Current.Request[key + "[]"])) {
                string[] values = HttpContext.Current.Request[key + "[]"].Split(',');
                retorno = values.Select(n => n).ToList();
            }

			return retorno;
		}

		//
		public static object setOrRemain(this object initialValue, string key) {

		    if (HttpContext.Current.Request[key] == null) {
		        return initialValue;
		    }

		    initialValue = HttpContext.Current.Request[key];

            return initialValue;
		}

		//Devolve um vazio caso a string seja nula
		public static string notNull(string str) {
			str = str ?? "";
			return str;
		}

		public static string linkPaginacao(object nroPagina, string action, string controller = null) {
			
			var ORequest = HttpContext.Current.Request;
			string[] keys = ORequest.QueryString.AllKeys.Where(x => !x.Equals("nroPagina")).ToArray();

			RouteValueDictionary RotaParametros = new RouteValueDictionary();
			RotaParametros.Add("nroPagina", nroPagina);

			foreach (string param in keys) {
				if (ORequest.QueryString[param] != null) {
					string valor = ORequest.QueryString[param];
					RotaParametros.Add(param, valor);
				}
			}

			var urlHelper = new UrlHelper(ORequest.RequestContext);
			string linkPaginacao = urlHelper.Action(action, controller, RotaParametros);
			return linkPaginacao;
		}

	}
}
