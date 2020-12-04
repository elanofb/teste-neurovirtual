using System.Web;
using System.Collections.Generic;
using DAL.Financeiro;

namespace System {

    public static class SessionContrato {

        private static readonly string prefixSession = "contrato_";

        public static void setListPagamentos(object listPagamentos) { SessionContrato.setSession("listPagamentos", listPagamentos); }

        public static List<TituloPagamento> getListPagamentos(){
            List<TituloPagamento> listPagamentos = new List<TituloPagamento>();
            if (SessionContrato.getSession("listPagamentos") != null) {
                listPagamentos = SessionContrato.getSession("listPagamentos") as List<TituloPagamento>;
            }
            return listPagamentos;
        }


        /**** Método genéricos de criação e recuperação de sessoes **/
        private static void setSession(string key, object value) {
            string definitiveKey = string.Concat(SessionContrato.prefixSession, key);
            HttpContext.Current.Session[definitiveKey] = value;
        }

        private static object getSession(string key) {
            string definitiveKey = string.Concat(SessionContrato.prefixSession, key);
            return (HttpContext.Current == null || HttpContext.Current.Session[definitiveKey] == null)? null: HttpContext.Current.Session[definitiveKey];
        }


    }
}
