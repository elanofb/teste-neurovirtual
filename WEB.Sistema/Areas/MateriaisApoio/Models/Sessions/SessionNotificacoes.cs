using System.Web;
using System.Collections.Generic;
using DAL.Pessoas;

namespace System{

    public static class SessionMateriaisApoio {

        private static readonly string prefixSession = "materiaisapoio_";

        public static void setListAssociadosEspecificos(object listAssociadosEspecificos) { 
            SessionMateriaisApoio.setSession("listAssociadosEspecificos", listAssociadosEspecificos); 
        }

        public static List<Pessoa> getListAssociadosEspecificos(){
            var listAssociadosEspecificos = new List<Pessoa>();
            if (SessionMateriaisApoio.getSession("listAssociadosEspecificos") != null) {
                listAssociadosEspecificos = SessionMateriaisApoio.getSession("listAssociadosEspecificos") as List<Pessoa>;
            }
            return listAssociadosEspecificos;
        }

        /**** Método genéricos de criação e recuperação de sessoes **/
        private static void setSession(string key, object value) {
            string definitiveKey = string.Concat(SessionMateriaisApoio.prefixSession, key);
            HttpContext.Current.Session[definitiveKey] = value;
        }

        private static object getSession(string key) {
            string definitiveKey = string.Concat(SessionMateriaisApoio.prefixSession, key);
            return (HttpContext.Current == null || HttpContext.Current.Session[definitiveKey] == null)? null: HttpContext.Current.Session[definitiveKey];
        }


    }
}
