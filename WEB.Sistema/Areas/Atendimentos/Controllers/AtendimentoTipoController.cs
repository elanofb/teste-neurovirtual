using System;
using System.Web.Mvc;
using System.Json;
using System.Linq;
using BLL.Atendimentos;
using DAL.Atendimentos;
using MvcFlashMessages;
using PagedList;
using WEB.Areas.Atendimentos.ViewModels;

namespace WEB.Areas.Atendimentos.Controllers {

	public class AtendimentoTipoController : Controller {

		//Atributos
		private IAtendimentoTipoBL _AtendimentoTipoBL;

		//Propriedades
		private IAtendimentoTipoBL OAtendimentoTipoBL => _AtendimentoTipoBL = _AtendimentoTipoBL ?? new AtendimentoTipoBL();

		public ActionResult listar() {

			var descricao = UtilRequest.getString("valorBusca");
			var ativo = UtilRequest.getBool("flagAtivo");

			var listaAtendimentoTipo = this.OAtendimentoTipoBL.listar(descricao, ativo).OrderBy(x => x.descricao);

			return View(listaAtendimentoTipo.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
		}

		[HttpGet]
		public ActionResult editar(int? id) {

			var ViewModel = new AtendimentoTipoForm();
			var OAtendimentoTipo = this.OAtendimentoTipoBL.carregar(UtilNumber.toInt32(id)) ?? new AtendimentoTipo();

			ViewModel.AtendimentoTipo = OAtendimentoTipo;

			return View(ViewModel);
		}

		[HttpPost]
		public ActionResult editar(AtendimentoTipoForm ViewModel) {

			if (!ModelState.IsValid) {
				return View(ViewModel);
			}

			bool flagSucesso = this.OAtendimentoTipoBL.salvar(ViewModel.AtendimentoTipo);
			
            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

                return RedirectToAction("editar", new { ViewModel.AtendimentoTipo.id });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

			return View(ViewModel);
		}

		//
		[HttpPost]
		public ActionResult excluir(int[] id) {
		
			JsonMessage Retorno = new JsonMessage();
			Retorno.error = true;
			
			/*foreach (int idExclusao in id) { 

				bool flagSucesso = this.OAtendimentoTipoBL.excluir(idExclusao);

                if (!flagSucesso) {

                    Retorno.error = true;

                    Retorno.message = "Alguns registros não puderam ser excluídos.";

                }
            }

            Retorno.message = Retorno.message.isEmpty() ? "Os registros foram removidos com sucesso." : Retorno.message;*/

            return Json(Retorno);
		}

        [ActionName("alterar-status"), HttpPost]
        public ActionResult alterarStatus(int id) {
            return Json(this.OAtendimentoTipoBL.alterarStatus(id));
        }

	}
}