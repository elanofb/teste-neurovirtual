using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Contribuicoes;
using DAL.Contribuicoes;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.Areas.Contribuicoes.ViewModels;

namespace WEB.Areas.Contribuicoes.Controllers {
    public class ContribuicaoTabelaPrecoController : Controller {

        //Constantes

        //Atributos
        private IContribuicaoTabelaPrecoBL _ContribuicaoTabelaPrecoBL;
        private IContribuicaoBL _ContribuicaoBL;

        //Propriedades
        private IContribuicaoTabelaPrecoBL OContribuicaoTabelaPrecoBL => _ContribuicaoTabelaPrecoBL = _ContribuicaoTabelaPrecoBL ?? new ContribuicaoTabelaPrecoBL();
        private IContribuicaoBL OContribuicaoBL => _ContribuicaoBL = _ContribuicaoBL ?? new ContribuicaoPadraoBL();

        //GET
        [ActionName("partial-tabelas")]
        public ActionResult partialTabelas(int idContribuicao) {

            var ViewModel = new ContribuicaoTabelaPrecoLista();

            ViewModel.Contribuicao = this.OContribuicaoBL.carregar(idContribuicao);

            ViewModel.carregarDados(idContribuicao);

            return PartialView(ViewModel);
        }

        //GET
        [ActionName("modal-form-cadastro")]
        public ActionResult modalFormCadastro(int? id, int? idContribuicao) {

            var ViewModel = new ContribuicaoTabelaPrecoForm();

            ViewModel.ContribuicaoTabelaPreco = this.OContribuicaoTabelaPrecoBL.carregar(UtilNumber.toInt32(id)) ?? new ContribuicaoTabelaPreco();

            if (ViewModel.ContribuicaoTabelaPreco.id == 0) {

                ViewModel.ContribuicaoTabelaPreco.idContribuicao = UtilNumber.toInt32(idContribuicao);

            }

            return PartialView(ViewModel);
        }

        //POST
        [HttpPost, ActionName("salvar-tabela-preco")]
        public ActionResult salvarTabelaPreco(ContribuicaoTabelaPrecoForm ViewModel) {

            if (!ModelState.IsValid) {
                return PartialView("modal-form-cadastro", ViewModel);
            }

            ViewModel.ContribuicaoTabelaPreco.idUsuarioAlteracao = User.id();

            ViewModel.ContribuicaoTabelaPreco.idUsuarioCadastro = User.id();

            bool flagSucesso = this.OContribuicaoTabelaPrecoBL.salvar(ViewModel.ContribuicaoTabelaPreco);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, "A tabela de preço foi salva com sucesso.");

                return Json(new { error = false, message = "" });
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não foi possível salvar o registro.");

            return PartialView("modal-form-cadastro", ViewModel);
        }

        //GET
        [ActionName("modal-tabela-preco-detalhe")]
        public ActionResult modalTabelaPrecoDetalhe(int idTabelaPreco) {

            var TabelaPreco = this.OContribuicaoTabelaPrecoBL.carregar(idTabelaPreco) ?? new ContribuicaoTabelaPreco();

            TabelaPreco.listaPrecos = TabelaPreco.listaPrecos.Where(x => x.flagExcluido == "N").ToList();

            return PartialView(TabelaPreco);
        }

        //GET
        [ActionName("partial-detalhe-tabela")]
        public ActionResult partialDetalheTabela(int idTabelaPreco) {

            int idOrganizacao = User.idOrganizacao();

            var TabelaPreco = this.OContribuicaoTabelaPrecoBL.carregar(idTabelaPreco) ?? new ContribuicaoTabelaPreco();

            TabelaPreco.listaPrecos = TabelaPreco.listaPrecos.Where(x => x.idOrganizacao == idOrganizacao && x.flagExcluido == "N").ToList();

            return PartialView(TabelaPreco);
        }

        //Remover um registro logicamente
        public ActionResult excluir(int id) {

            int idUsuarioLogado = User.id();

            var Retorno = this.OContribuicaoTabelaPrecoBL.excluir(id, idUsuarioLogado);

            if (Retorno.flagError) {
                return Json(new { error = true, message = "Não foi possível remover o registro informado." });
            }

            return Json(new { error = false, message = "O registro foi removido com sucesso." });
        }
    }
}
