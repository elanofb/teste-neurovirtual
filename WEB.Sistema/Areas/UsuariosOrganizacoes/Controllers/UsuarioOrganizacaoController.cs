using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Organizacoes;
using WEB.App_Infrastructure;

namespace WEB.Areas.UsuariosOrganizacoes.Controllers {

    public class UsuarioOrganizacaoController : BaseSistemaController {

        //Atributos
        private IUsuarioOrganizacaoBL _UsuarioOrganizacaoBL;

        //Propriedades
        private IUsuarioOrganizacaoBL OUsuarioOrganizacaoBL => this._UsuarioOrganizacaoBL = this._UsuarioOrganizacaoBL ?? new UsuarioOrganizacaoBL();
	
        // 
        [ActionName("partial-form-vincular-organizacao")]
        public PartialViewResult partialFormVincularOrganizacao() {
            return PartialView();
        }

        //
        [HttpGet, ActionName("partial-lista-organizacoes-vinculadas")]
        public PartialViewResult partialListaOrganizacoesVinculadas(int? id) {

            var listaOrganizacaoVinculada = this.OUsuarioOrganizacaoBL.listar(UtilNumber.toInt32(id), 0).ToList();
            return PartialView(listaOrganizacaoVinculada);
        }

        //
        [HttpPost, ActionName("vincular-organizacao")]
        public ActionResult vincularOrganizacao(int idUsuario, int idOrganizacao) {

            this.OUsuarioOrganizacaoBL.salvar(idUsuario, idOrganizacao);

            return RedirectToAction("partial-lista-organizacoes-vinculadas", new { id = idUsuario });
        }

        //
        [ActionName("excluir-organizacao-vinculada")]
        public ActionResult excluirOrganizacaoVinculada(int id, int? idUsuario) {

            this.OUsuarioOrganizacaoBL.excluir(id);

            return RedirectToAction("partial-lista-organizacoes-vinculadas", new { id = idUsuario });
        }
    }
}