using System;
using System.Linq;
using System.Web.Mvc;
using PagedList;
using BLL.Planos;
using DAL.Planos;
using WEB.Areas.Planos.ViewModels;
using System.Json;
using BLL.Pessoas;
using DAL.Pessoas;
using MvcFlashMessages;

namespace WEB.Areas.Planos.Controllers {
    public class PlanoContratacaoController : Controller {
        
        //Constantes

        //Atributos
        private IPlanoContratacaoBL _PlanoContratacaoBL;
        
        //Propriedades

        private IPlanoContratacaoBL OPlanoContratacaoBL { get { return (this._PlanoContratacaoBL = this._PlanoContratacaoBL ?? new PlanoContratacaoBL()); } }

        public PlanoContratacaoController() {
        }

        //
        public ActionResult listar() {
            string descricao = UtilRequest.getString("valorBusca");
            int idStatus = UtilRequest.getInt32("idStatus");
            int idPlano = UtilRequest.getInt32("idPlano");

            var lista = this.OPlanoContratacaoBL.listar(descricao, idPlano, idStatus).OrderByDescending(x => x.dtCadastro);

            return View(lista.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //
        [HttpGet]
        public ActionResult editar(int? id) {

            var ViewModel = new PlanoContratacaoForm();
            
			ViewModel.PlanoContratacao = this.OPlanoContratacaoBL.carregar(UtilNumber.toInt32(id)) ?? new PlanoContratacao();

            if (ViewModel.PlanoContratacao.id > 0) {
                var OPessoaVW = new PessoaVWBL().carregar(ViewModel.PlanoContratacao.idPessoa) ?? new PessoaVW();
                ViewModel.idContratante = OPessoaVW.flagCategoriaPessoa + "#" + OPessoaVW.idPessoa;
            }			

            return View(ViewModel);        
        }

        [HttpPost]
        public ActionResult editar(PlanoContratacaoForm ViewModel) {

            if (!ModelState.IsValid) {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não foi possível realizar a operação.");
                return View(ViewModel);
            }

            ViewModel.PlanoContratacao.idPessoa = UtilNumber.toInt32(UtilString.onlyNumber(ViewModel.idContratante));

            bool flagSucesso = this.OPlanoContratacaoBL.salvar(ViewModel.PlanoContratacao);

            if (flagSucesso) {
                 this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "Os dados foram salvos com sucesso.");
                return RedirectToAction("editar", new {ViewModel.PlanoContratacao.id });
            }

            return View(ViewModel);        
        }

        [HttpPost]
		public ActionResult excluir(int[] id) {

			JsonMessage Retorno = new JsonMessage();
			Retorno.error = false;

			foreach (int idExclusao in id) {
				var RetornoExclusao = this.OPlanoContratacaoBL.excluir(idExclusao);

				if (RetornoExclusao.flagError) {
					Retorno.error = true;
					Retorno.message = "Alguns registros não puderam ser excluídos.";
				}
			}

			return Json(Retorno);
		}
    }
}