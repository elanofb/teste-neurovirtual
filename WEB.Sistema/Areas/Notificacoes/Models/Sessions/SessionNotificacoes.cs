using System.Web;
using System.Collections.Generic;
using DAL.Notificacoes;
using DAL.Permissao;

namespace System{

    public static class SessionNotificacoes {

        private static readonly string prefixSession = "notificacoes_";


        public static void setListAssociadosEspecificos(object listAssociadosEspecificos) { SessionNotificacoes.setSession("listAssociadosEspecificos", listAssociadosEspecificos); }

        public static List<NotificacaoSistemaEnvio> getListAssociadosEspecificos(){
            var listAssociadosEspecificos = new List<NotificacaoSistemaEnvio>();
            if (SessionNotificacoes.getSession("listAssociadosEspecificos") != null) {
                listAssociadosEspecificos = SessionNotificacoes.getSession("listAssociadosEspecificos") as List<NotificacaoSistemaEnvio>;
            }
            return listAssociadosEspecificos;
        }



        public static void setListUsuariosEspecificos(object listUsuariosEspecificos) { SessionNotificacoes.setSession("listUsuariosEspecificos", listUsuariosEspecificos); }

        public static List<UsuarioSistema> getListUsuariosEspecificos(){
            var listUsuariosEspecificos = new List<UsuarioSistema>();
            if (SessionNotificacoes.getSession("listUsuariosEspecificos") != null) {
                listUsuariosEspecificos = SessionNotificacoes.getSession("listUsuariosEspecificos") as List<UsuarioSistema>;
            }
            return listUsuariosEspecificos;
        }


        public static void setListPerfisEspecificos(object listPerfisEspecificos) { SessionNotificacoes.setSession("listPerfisEspecificos", listPerfisEspecificos); }

        public static List<PerfilAcesso> getListPerfisEspecificos(){
            var listPerfisEspecificos = new List<PerfilAcesso>();
            if (SessionNotificacoes.getSession("listPerfisEspecificos") != null) {
                listPerfisEspecificos = SessionNotificacoes.getSession("listPerfisEspecificos") as List<PerfilAcesso>;
            }
            return listPerfisEspecificos;
        }



        /**** Método genéricos de criação e recuperação de sessoes **/
        private static void setSession(string key, object value) {
            string definitiveKey = string.Concat(SessionNotificacoes.prefixSession, key);
            HttpContext.Current.Session[definitiveKey] = value;
        }

        private static object getSession(string key) {
            string definitiveKey = string.Concat(SessionNotificacoes.prefixSession, key);
            return (HttpContext.Current == null || HttpContext.Current.Session[definitiveKey] == null)? null: HttpContext.Current.Session[definitiveKey];
        }


    }
}
