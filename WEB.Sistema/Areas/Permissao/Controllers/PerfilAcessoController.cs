using System;
using System.Linq;
using System.Web.Mvc;
using WEB.Areas.Permissao.ViewModels;
using PagedList;
using BLL.Permissao;
using DAL.Permissao;
using MvcFlashMessages;
using System.Json;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.Permissao.Controllers {

    public class PerfilAcessoController : Controller {

		//Constantes

		//Atributos
        private IPerfilAcessoBL _PerfilAcessoBL;

        //Propriedades
        private IPerfilAcessoBL OPerfilAcessoBL => this._PerfilAcessoBL = this._PerfilAcessoBL ?? new PerfilAcessoBL();

        //Eventos

        //
        public PerfilAcessoController() { } 

		//
        public ActionResult listar() {

			var idOrganizacao = UtilRequest.getInt32("idOrganizacao");
			var descricao = UtilRequest.getString("valorBusca");
            var ativo = UtilRequest.getString("flagAtivo");

            var listaPerfis = this.OPerfilAcessoBL.listar(idOrganizacao, descricao, ativo)
                                                    .Where(x => x.id != PerfilAcessoConst.DESENVOLVEDOR)
                                                    .OrderBy(x => x.descricao);

            return View(listaPerfis.ToPagedList(UtilRequest.getNroPagina(), 10));
        }

		//
		[HttpGet]
        public ActionResult editar(int? id) {

            var ViewModel = new PerfilAcessoForm();
			var OPerfilAcesso = this.OPerfilAcessoBL.carregar(UtilNumber.toInt32(id)) ?? new PerfilAcesso();
            ViewModel.PerfilAcesso = OPerfilAcesso;
            ViewModel.PerfilAcesso.idOrganizacao = UtilNumber.toInt32(OPerfilAcesso.idOrganizacao) > 0 ? OPerfilAcesso.idOrganizacao : User.idOrganizacao();
            return View(ViewModel);
        }

        //
		[HttpPost]
        public ActionResult editar(PerfilAcessoForm ViewModel) {

            if (!ModelState.IsValid){
				return View(ViewModel);
			}

			var flagSucesso = this.OPerfilAcessoBL.salvar(ViewModel.PerfilAcesso);

            if (flagSucesso) {
                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados do perfil foram salvos com sucesso."));
                return RedirectToAction("listar");
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Houve um problema ao salvar o registro. Tente novamente."));
            return View(ViewModel);
        }    
	
        //
        [HttpPost]
        public ActionResult excluir(int[] id) {

            var Retorno = new JsonMessage();

            Retorno.error = false;

            Retorno.message = "Os registros informados foram removidos com sucesso.";

            foreach (var idPerfil in id) {

                var RetornoItem = this.OPerfilAcessoBL.excluir(idPerfil, User.id());

                if (RetornoItem.flagError) {
                    Retorno.error = true;
                    Retorno.message = "Não foi possível remover todos os registros.";
                }

            }

            return Json(Retorno);
        }

        //
		public JsonResult autocompletar(string term) {
			var listPerfis = this.OPerfilAcessoBL.getAutoComplete(term);
			return Json(listPerfis, JsonRequestBehavior.AllowGet);
		}
    }
}