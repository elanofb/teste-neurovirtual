using System;
using System.Linq;
using System.Web.Mvc;
using DAL.Permissao;
using MvcFlashMessages;

namespace WEB.Areas.AvisosNotificacoes.Controllers {

    public class AvisoNotificacaoPerfilController : Controller {

        // Atributos

        // Propriedades

        public AvisoNotificacaoPerfilController() {

        }

        [ActionName("partialPerfisEspecificos")]
        public PartialViewResult index() { 
            return PartialView("partialPerfisEspecificos");
        }

        [HttpPost]
        public PartialViewResult adicionarPerfilEspecifico() {

            int idPerfil = UtilRequest.getInt32("idPerfilEspecifico");
            string nomePerfil = UtilRequest.getString("nomePerfilEspecifico");

            if (idPerfil > 0) {
                var listPerfisEspecificos = SessionNotificacoes.getListPerfisEspecificos();

                if (!listPerfisEspecificos.Any(x => x.id == idPerfil)) {
                    var OAssociado = new PerfilAcesso() { id = idPerfil, descricao = nomePerfil };
                    listPerfisEspecificos.Add(OAssociado);
                    SessionNotificacoes.setListPerfisEspecificos(listPerfisEspecificos);
                } else {
                    this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Esse perfil já está adicionado na sua lista.");
                }
            } else {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Selecione um perfil para adicionar à lista.");
            }

            return PartialView("partialPerfisEspecificos");
        }

        //
        [HttpPost]
        public JsonResult excluirPerfilEspecifico(int id) {
            var list = SessionNotificacoes.getListPerfisEspecificos();
            list.Remove(list.Where(x => x.id == id).FirstOrDefault());
            return Json(true);
        }

    }
}