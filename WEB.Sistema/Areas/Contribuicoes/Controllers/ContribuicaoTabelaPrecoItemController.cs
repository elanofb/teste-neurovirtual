using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Contribuicoes;
using DAL.Contribuicoes;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.Areas.Contribuicoes.ViewModels;

namespace WEB.Areas.Contribuicoes.Controllers {
	public class ContribuicaoTabelaPrecoItemController : Controller {

		//Constantes

		//Atributos
	    private IContribuicaoTabelaPrecoBL _ContribuicaoTabelaPrecoBL;
        private IContribuicaoPrecoBL _ContribuicaoPrecoBL;

		//Propriedades
        private IContribuicaoTabelaPrecoBL OContribuicaoTabelaPrecoBL => _ContribuicaoTabelaPrecoBL = _ContribuicaoTabelaPrecoBL ?? new ContribuicaoTabelaPrecoBL();
        private IContribuicaoPrecoBL OContribuicaoPrecoBL => _ContribuicaoPrecoBL = _ContribuicaoPrecoBL ?? new ContribuicaoPrecoBL();

	    //GET
		[ActionName("modal-form-tabela-preco-item")]
		public ActionResult modalFormTabelaPrecoItem(int? id, int? idTabelaPreco) {

            var ViewModel = new ContribuicaoPrecoForm();

		    ViewModel.ContribuicaoPreco = this.OContribuicaoPrecoBL.carregar(UtilNumber.toInt32(id)) ?? new ContribuicaoPreco();

            ViewModel.carregarDados(UtilNumber.toInt32(idTabelaPreco));

			return PartialView(ViewModel);
		}

	    //POST
		[HttpPost, ActionName("salvar-tabela-preco-item")]
		public ActionResult salvarTabelaPrecoItem(ContribuicaoPrecoForm ViewModel) {

            if (!ModelState.IsValid) {

                ViewModel.carregarDados( UtilNumber.toInt32(ViewModel.ContribuicaoPreco.idTabelaPreco));

		        return PartialView("modal-form-tabela-preco-item", ViewModel);
		    }

		    var TabelaPreco = this.OContribuicaoTabelaPrecoBL.carregar(UtilNumber.toInt32(ViewModel.ContribuicaoPreco.idTabelaPreco));

            ViewModel.ContribuicaoPreco.idUsuarioAlteracao = User.id();

            ViewModel.ContribuicaoPreco.idUsuarioCadastro = User.id();

		    ViewModel.ContribuicaoPreco.idContribuicao = TabelaPreco.idContribuicao;

		    ViewModel.ContribuicaoPreco.listaDesconto = ViewModel.ContribuicaoPreco.listaDesconto ?? new List<ContribuicaoPrecoDesconto>();

		    ViewModel.ContribuicaoPreco.listaDesconto = ViewModel.ContribuicaoPreco.listaDesconto.Where(x => x.qtdeDiasAntecipacao > 0).ToList();

		    if (ViewModel.ContribuicaoPreco.flagIsento == true) {

                ViewModel.ContribuicaoPreco.valorFinal = 0;

                ViewModel.ContribuicaoPreco.listaDesconto = new List<ContribuicaoPrecoDesconto>();
		    }

            bool flagSucesso = this.OContribuicaoPrecoBL.salvar(ViewModel.ContribuicaoPreco);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "Os dados foram salvos com sucesso.");

                ViewModel = new ContribuicaoPrecoForm();

                ViewModel.carregarDados(TabelaPreco.id);

                return PartialView("modal-form-tabela-preco-item", ViewModel);

            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não foi possível salvar o registro.");

            return PartialView("modal-form-tabela-preco-item", ViewModel);
		}

		//Remover um registro logicamente
		public ActionResult excluir(int id) {

			int idUsuarioLogado = User.id();

		    var Retorno = this.OContribuicaoPrecoBL.excluir(id, idUsuarioLogado);

		    if (Retorno.flagError) {
		        return Json(new { error = true, message = "Não foi possível remover o registro informado."});
		    }

		    return Json(new { error = false, message = "O registro foi removido com sucesso."});
		}
	}
}
