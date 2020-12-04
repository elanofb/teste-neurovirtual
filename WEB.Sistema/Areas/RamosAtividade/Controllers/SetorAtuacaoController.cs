using System;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using BLL.RamosAtividade;
using DAL.Permissao.Security.Extensions;
using DAL.RamosAtividade;
using MvcFlashMessages;
using PagedList;
using WEB.Areas.RamosAtividade.ViewModels;

namespace WEB.Areas.RamosAtividade.Controllers {

	public class SetorAtuacaoController : Controller {

		//Constantes

		//Atributos
		private ISetorAtuacaoBL _SetorAtuacaoBL;

		//Propriedades
		private ISetorAtuacaoBL OSetorAtuacaoBL => _SetorAtuacaoBL = _SetorAtuacaoBL ?? new SetorAtuacaoBL();

	    //Construtor
		public SetorAtuacaoController() { 
				
		}


		//
		public ActionResult listar() {

			string descricao = UtilRequest.getString("valorBusca");

            bool? ativo = UtilRequest.getBool("flagAtivo");

            int idRamoAtividade = UtilRequest.getInt32("idRamoAtividade");

            var listaRegistros = this.OSetorAtuacaoBL.listar(idRamoAtividade, descricao, ativo)
                                                    .OrderBy(x => x.descricao);

            return View(listaRegistros.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
		}

		//
		[HttpGet]
		public ActionResult editar(int? id) {

			SetorAtuacaoForm ViewModel = new SetorAtuacaoForm();

			var Oregistro = this.OSetorAtuacaoBL.carregar(UtilNumber.toInt32(id)) ?? new SetorAtuacao();

		    ViewModel.SetorAtuacao = Oregistro;

			return View(ViewModel);
		}

		//
		[HttpPost]
		public ActionResult editar(SetorAtuacaoForm ViewModel) {

			if (!ModelState.IsValid) {
				return View(ViewModel);
			}

			bool flagSucesso = this.OSetorAtuacaoBL.salvar(ViewModel.SetorAtuacao);

			if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados foram salvos com sucesso."));

				return RedirectToAction("editar", new { id = ViewModel.SetorAtuacao.id });
			}

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View(ViewModel);
		}

        //
        [HttpPost, ActionName("alterar-status")]
        public ActionResult alterarStatus(int id) {
	        return Json(this.OSetorAtuacaoBL.alterarStatus(id));
        }

		//
		[HttpPost]
		public ActionResult excluir(int[] id) {

            var Retorno = new JsonMessage();
		    Retorno.error = false;
		    Retorno.message = "Os registros informados foram removidos com sucesso.";

		    foreach (var idItem in id) {

		        var RetornoItem = this.OSetorAtuacaoBL.excluir(idItem, User.id());

		        if (RetornoItem.flagError == true) {
		            Retorno.error = true;
		            Retorno.message = "Nem todos os registros puderam ser removidos. Tente novamente mais tarde.";
		        }
		    }

			return Json(Retorno);
		}
	}
}