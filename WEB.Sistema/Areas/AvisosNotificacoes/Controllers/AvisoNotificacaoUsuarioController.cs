using System;
using System.Linq;
using System.Web.Mvc;
using DAL.Permissao;
using MvcFlashMessages;

namespace WEB.Areas.AvisosNotificacoes.Controllers {

    public class AvisoNotificacaoUsuarioController : Controller {

        // Atributos

        // Propriedades

        public AvisoNotificacaoUsuarioController() {

        }

        [ActionName("partialUsuariosEspecificos")]
        public PartialViewResult index() { 
            return PartialView("partialUsuariosEspecificos");
        }

        [HttpPost]
        public PartialViewResult adicionarUsuarioEspecifico() {

            int idUsuario = UtilRequest.getInt32("idUsuarioEspecifico");
            string nomeUsuario = UtilRequest.getString("nomeUsuarioEspecifico");
            string loginUsuario = UtilRequest.getString("loginUsuarioEspecifico");
            string emailUsuario = UtilRequest.getString("emailUsuarioEspecifico");

            if (idUsuario > 0) {
                var listUsuariosEspecificos = SessionNotificacoes.getListUsuariosEspecificos();

                if (!listUsuariosEspecificos.Any(x => x.id == idUsuario)) {
                    var OAssociado = new UsuarioSistema() { id = idUsuario, nome = nomeUsuario, login = loginUsuario, email = emailUsuario };
                    listUsuariosEspecificos.Add(OAssociado);
                    SessionNotificacoes.setListUsuariosEspecificos(listUsuariosEspecificos);
                } else {
                    this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Esse usuário já está adicionado na sua lista.");
                }
            } else {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Selecione um usuário para adicionar à lista.");
            }

            return PartialView("partialUsuariosEspecificos");
        }

        //
        [HttpPost]
        public JsonResult excluirUsuarioEspecifico(int id) {
            var list = SessionNotificacoes.getListUsuariosEspecificos();
            list.Remove(list.Where(x => x.id == id).FirstOrDefault());
            return Json(true);
        }

    }
}