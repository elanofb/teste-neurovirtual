using System;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using BLL.AreasAtuacao;
using DAL.AreasAtuacao;
using DAL.Permissao.Security.Extensions;
using WEB.Areas.AreasAtuacao.ViewModels;
using MvcFlashMessages;
using PagedList;

namespace WEB.Areas.AreasAtuacao.Controllers {

	public class AreaAtuacaoController : Controller {

		//Constantes

		//Atributos
		private IAreaAtuacaoBL _AreaAtuacaoBL;

		//Propriedades
		private IAreaAtuacaoBL OAreaAtuacaoBL => (this._AreaAtuacaoBL = this._AreaAtuacaoBL ?? new AreaAtuacaoBL());

	    //Construtor
		public AreaAtuacaoController() { 				
		}

		//
		public ActionResult listar() {

			string descricao = UtilRequest.getString("valorBusca");

            string ativo = UtilRequest.getString("flagAtivo");

            var listaAreaAtuacao = this.OAreaAtuacaoBL.listar(descricao, ativo).OrderBy(x => x.descricao);

            return View(listaAreaAtuacao.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
		}

		//
		[HttpGet]
		public ActionResult editar(int? id) {

			AreaAtuacaoForm ViewModel = new AreaAtuacaoForm();

			var OAreaAtuacao = this.OAreaAtuacaoBL.carregar(UtilNumber.toInt32(id)) ?? new AreaAtuacao();

		    ViewModel.AreaAtuacao = OAreaAtuacao;

			return View(ViewModel);
		}

		//
		[HttpPost]
		public ActionResult editar(AreaAtuacaoForm ViewModel) {

			if (!ModelState.IsValid) {
				return View(ViewModel);
			}

			bool flagSucesso = this.OAreaAtuacaoBL.salvar(ViewModel.AreaAtuacao);

			if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "Os dados forma salvos com sucesso."));

				return RedirectToAction("editar", new { id = ViewModel.AreaAtuacao.id });
			}

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve um problema ao salvar o registro. Tente novamente."));

            return View(ViewModel);
		}

        //
		[HttpPost, ActionName("alterar-status")]
		public ActionResult alterarStatus(int id) {
			return Json(this.OAreaAtuacaoBL.alterarStatus(id));
		}

		//
		[HttpPost]
		public ActionResult excluir(int[] id) {

            var Retorno = new JsonMessage();
		    Retorno.error = false;
		    Retorno.message = "Os registros informados foram removidos com sucesso.";

		    foreach (var idItem in id) {

		        var RetornoItem = this.OAreaAtuacaoBL.excluir(idItem, User.id());

		        if (RetornoItem.flagError == true) {
		            Retorno.error = true;
		            Retorno.message = "Algum(ns) registro(s) não pode(ram) ser removido(s).";
		        }
		    }

			return Json(Retorno);
		}
		
		[ActionName("listar-json")]
		public ActionResult listarJson(){

			string nome = UtilRequest.getString("valorBusca");

			string ativo = UtilRequest.getString("flagAtivo");

			var listaLocais = this.OAreaAtuacaoBL.listar(nome, ativo)
				.ToList()
				.Select(x => new{
					value = x.id,
					text = x.descricao
				}).ToList();

			return Json(listaLocais, JsonRequestBehavior.AllowGet);
		}
	}
}