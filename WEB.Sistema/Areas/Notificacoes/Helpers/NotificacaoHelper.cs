using System.Web.Mvc;
using DAL.Notificacoes;

namespace WEB.Helpers {
    public class NotificacaoHelper {

        //
        public static SelectList getTipoEnvioAssociado(string selected) {

            var list = new[] { 
					new{ value = TipoEnvioAssociadoConst.TODOS, text = "Todos" },
					new{ value = TipoEnvioAssociadoConst.INADIMPLENTES, text = "Inadimplentes" },
					new{ value = TipoEnvioAssociadoConst.ADIMPLENTES, text = "Adimplentes" },
					new{ value = TipoEnvioAssociadoConst.ESPECIFICOS, text = "Específicos" }
            };

            return new SelectList(list, "value", "text", selected);
        }

        public static SelectList getTipoEnvioUsuario(string selected) {

            var list = new[] { 
					new{ value = TipoEnvioUsuarioConst.TODOS, text = "Todos" },
					new{ value = TipoEnvioUsuarioConst.USUARIOS_ESPECIFICOS, text = "Usuarios Específicos" },
					new{ value = TipoEnvioUsuarioConst.PERFIS_ESPECIFICOS, text = "Perfis Específicos" }
            };

            return new SelectList(list, "value", "text", selected);
        }

    }
}