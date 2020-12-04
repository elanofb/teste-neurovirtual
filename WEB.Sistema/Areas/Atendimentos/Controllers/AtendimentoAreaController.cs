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

	public class AtendimentoAreaController : Controller {

		//Atributos
		private IAtendimentoAreaBL _AtendimentoAreaBL;

		//Propriedades
		private IAtendimentoAreaBL OAtendimentoAreaBL => _AtendimentoAreaBL = _AtendimentoAreaBL ?? new AtendimentoAreaBL();

		public ActionResult listar() {

			var descricao = UtilRequest.getString("valorBusca");
			var ativo = UtilRequest.getBool("flagAtivo");

			var listaAtendimentoArea = this.OAtendimentoAreaBL.listar(descricao, ativo).OrderBy(x => x.descricao);

			return View(listaAtendimentoArea.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
		}

		[HttpGet]
		public ActionResult editar(int? id) {

			var ViewModel = new AtendimentoAreaForm();
			var OAtendimentoArea = this.OAtendimentoAreaBL.carregar(UtilNumber.toInt32(id)) ?? new AtendimentoArea();

			ViewModel.AtendimentoArea = OAtendimentoArea;

			return View(ViewModel);
		}

		[HttpPost]
		public ActionResult editar(AtendimentoAreaForm ViewModel) {

			if (!ModelState.IsValid) {
				return View(ViewModel);
			}

			bool flagSucesso = this.OAtendimentoAreaBL.salvar(ViewModel.AtendimentoArea);
			
            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

                return RedirectToAction("editar", new { ViewModel.AtendimentoArea.id });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

			return View(ViewModel);
		}

		//
		[HttpPost]
		public ActionResult excluir(int[] id) {

			JsonMessage Retorno = new JsonMessage();
			Retorno.error = false;

			foreach (int idExclusao in id) { 

				bool flagSucesso = this.OAtendimentoAreaBL.excluir(idExclusao);

                if (!flagSucesso) {

                    Retorno.error = true;

                    Retorno.message = "Alguns registros não puderam ser excluídos.";

                }
            }

            Retorno.message = Retorno.message.isEmpty() ? "Os registros foram removidos com sucesso." : Retorno.message;

            return Json(Retorno);
		}

        [ActionName("alterar-status"), HttpPost]
        public ActionResult alterarStatus(int id) {
            return Json(this.OAtendimentoAreaBL.alterarStatus(id));
        }

	}
}