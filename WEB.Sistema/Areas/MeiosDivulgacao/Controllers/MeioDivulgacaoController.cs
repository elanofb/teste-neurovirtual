using System;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using BLL.MeiosDivulgacao;
using DAL.MeiosDivulgacao;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using PagedList;
using WEB.Areas.MeiosDivulgacao.ViewModels;

namespace WEB.Areas.MeiosDivulgacao.Controllers {

	public class MeioDivulgacaoController : Controller {

		//Constantes

		//Atributos
		private IMeioDivulgacaoBL _MeioDivulgacaoBL;

		//Propriedades
		private IMeioDivulgacaoBL OMeioDivulgacaoBL => _MeioDivulgacaoBL = _MeioDivulgacaoBL ?? new MeioDivulgacaoBL();

	    //Construtor
		public MeioDivulgacaoController() { 
				
		}
        
		//
		public ActionResult listar() {

			string descricao = UtilRequest.getString("valorBusca");

            bool? ativo = UtilRequest.getBool("flagAtivo");

            var listaRegistros = this.OMeioDivulgacaoBL.listar(User.idOrganizacao(), descricao, ativo).OrderBy(x => x.descricao);

            return View(listaRegistros.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
		}

		//
		[HttpGet]
		public ActionResult editar(int? id) {

			MeioDivulgacaoForm ViewModel = new MeioDivulgacaoForm();

			var Oregistro = this.OMeioDivulgacaoBL.carregar(UtilNumber.toInt32(id)) ?? new MeioDivulgacao();

		    ViewModel.MeioDivulgacao = Oregistro;

			return View(ViewModel);
		}

		//
		[HttpPost]
		public ActionResult editar(MeioDivulgacaoForm ViewModel) {

			if (!ModelState.IsValid) {
				return View(ViewModel);
			}

			bool flagSucesso = this.OMeioDivulgacaoBL.salvar(ViewModel.MeioDivulgacao);

			if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

				return RedirectToAction("listar");
			}

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View(ViewModel);
		}

        //
		[HttpPost, ActionName("alterar-status")]
		public ActionResult alterarStatus(int id) {
			return Json(this.OMeioDivulgacaoBL.alterarStatus(id));
		}

		//
		[HttpPost]
		public ActionResult excluir(int[] id) {

            var Retorno = new JsonMessage();
		    Retorno.error = false;
		    Retorno.message = "Os registros informados foram removidos com sucesso.";

		    foreach (var idItem in id) {

		        var RetornoItem = this.OMeioDivulgacaoBL.excluir(idItem, User.id());

		        if (RetornoItem.flagError == true) {
		            Retorno.error = true;
		            Retorno.message = "Nem todos os registros puderam ser removidos. Tente novamente mais tarde.";
		        }
		    }

			return Json(Retorno);
		}
	}
}