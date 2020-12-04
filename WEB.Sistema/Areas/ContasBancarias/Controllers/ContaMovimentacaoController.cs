using System;
using System.Json;
using System.Linq;
using System.Web.Mvc;
using BLL.ContasBancarias;
using BLL.Services;
using PagedList;
using DAL.ContasBancarias;
using MvcFlashMessages;
using WEB.App_Infrastructure;
using WEB.Areas.ContasBancarias.ViewModels;

namespace WEB.Areas.ContasBancarias.Controllers {

    public class ContaMovimentacaoController : BaseSistemaController {
        //Constantes

        //Atributos
        private IContaBancariaBL _ContaBancariaBL;
        private IContaBancariaMovimentacaoBL _IContaBancariaMovimentacaoBL;

        //Propriedades
        private IContaBancariaBL OContaBancariaBL => _ContaBancariaBL = _ContaBancariaBL ?? new ContaBancariaBL();
        private IContaBancariaMovimentacaoBL OContaBancariaMovimentacaoBL => _IContaBancariaMovimentacaoBL = _IContaBancariaMovimentacaoBL ?? new ContaBancariaMovimentacaoBL();

        //
        public ContaMovimentacaoController() {
        }

        //
        [ActionName("partial-lista-movimentacoes")]
        public PartialViewResult partialListaMovimentacoes(int idContaBancaria) {

            var ViewModel = new ListaMovimentacaoContaVM();

            ViewModel.idContaBancaria = idContaBancaria;
            
            ViewModel.listaMovimentacoes = this.OContaBancariaMovimentacaoBL.listar("", "", 0, 0, null, null)
                                                                             .Where(x => 
                                                                                x.idContaBancariaOrigem == idContaBancaria || 
                                                                                x.idContaBancariaDestino == idContaBancaria
                                                                            )
                                                                             .Select(x => new { 
                                                                                    x.id, 
                                                                                    x.dtOperacao, 
                                                                                    x.valor,
                                                                                    x.idContaBancariaOrigem,
                                                                                    x.idContaBancariaDestino,
                                                                                    ContaBancariaOrigem = new { x.ContaBancariaOrigem.descricao, x.ContaBancariaOrigem.nroAgencia, x.ContaBancariaOrigem.nroConta },
                                                                                    ContaBancariaDestino = new { x.ContaBancariaDestino.descricao, x.ContaBancariaDestino.nroAgencia, x.ContaBancariaDestino.nroConta }
                                                                            })
                                                                            .ToListJsonObject<ContaBancariaMovimentacao>()
                                                                            .OrderByDescending(x => x.dtOperacao)
                                                                            .ThenByDescending(x => x.id)
                                                                            .ToList();

            return PartialView(ViewModel);
        }

        //
        public ActionResult listar() {

            var descricao = UtilRequest.getString("valorBusca");
            var idContaBancariaOrigem = UtilRequest.getInt32("idContaBancariaOrigem");
            var idTipoOperacao = UtilRequest.getInt32("idTipoOperacao");
            var dtOperacao = UtilRequest.getString("dtOperacao");
            var dtInicio = DateTime.MinValue;
            var dtFim = DateTime.MinValue;

            if (String.IsNullOrEmpty(dtOperacao)) {
                int ultimoDia = DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month);
                dtOperacao = "01/" + UtilString.acertaCasas(DateTime.Now.Month.ToString(), 2, "0") + "/" + DateTime.Now.Year + " - " + new DateTime(DateTime.Today.Year, DateTime.Today.Month, ultimoDia).ToShortDateString();
            }
            
            if (!String.IsNullOrEmpty(dtOperacao)) {
                dtInicio = DateTime.Parse(dtOperacao.Split('-')[0]);
                dtFim = DateTime.Parse(dtOperacao.Split('-')[1]);
            }

            var listaContaMovimentacao = this.OContaBancariaMovimentacaoBL.listar(descricao, "S", idContaBancariaOrigem, idTipoOperacao, dtInicio, dtFim).OrderByDescending(x => x.dtOperacao);

            var OContaBancaria = OContaBancariaBL.carregar(idContaBancariaOrigem);

            ViewBag.nomeConta =OContaBancaria != null ? OContaBancariaBL.carregar(idContaBancariaOrigem).descricao : "Não encontrado";
            ViewBag.dtOperacao = dtOperacao;

            return View(listaContaMovimentacao.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros()));
        }

        //
        [HttpGet]
        public ActionResult editar(int? id, short? idContaBancariaOrigem, int? idTipoOperacao) {

            var ViewModel = new ContaMovimentacaoForm();
            var oContaMovimentacao = this.OContaBancariaMovimentacaoBL.carregar(UtilNumber.toInt32(id)) ?? new ContaBancariaMovimentacao();

            ViewModel.ContaMovimentacao = oContaMovimentacao;

            if (oContaMovimentacao.id == 0) {
                ViewModel.ContaMovimentacao.idTipoOperacao = Convert.ToInt32(idTipoOperacao);
                ViewModel.ContaMovimentacao.idContaBancariaOrigem = Convert.ToInt16(idContaBancariaOrigem);
            }

            ViewModel.descricaoConta = this.OContaBancariaBL.carregar(ViewModel.ContaMovimentacao.idContaBancariaOrigem).descricao;
            ViewModel.urlRetorno = UtilRequest.getString("urlRetorno");

            if (ViewModel.ContaMovimentacao.id == 0) {
                ViewModel.ContaMovimentacao.dtOperacao = DateTime.Now;
            }

            return View(ViewModel);
        }

        //
        [HttpPost]
        public ActionResult editar(ContaMovimentacaoForm ViewModel) {

            if (!ModelState.IsValid) {
                return View(ViewModel);
            }

            bool flagSucesso = this.OContaBancariaMovimentacaoBL.salvar(ViewModel.ContaMovimentacao);

            if (flagSucesso) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso", "Os dados foram salvos com sucesso."));

            } else {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Falha", "Não foi possível salvar as informações, tente novamente."));

            }
            if (ViewModel.ContaMovimentacao.id > 0) {
                return RedirectToAction("editar", new { ViewModel.ContaMovimentacao.id, ViewModel.ContaMovimentacao.idContaBancariaOrigem, ViewModel.ContaMovimentacao.idTipoOperacao, urlRetorno = ViewModel.urlRetorno });
            }

            return View(ViewModel);
        }

        //
        [HttpPost]
        public ActionResult excluir(int[] id) {
            JsonMessage Retorno = new JsonMessage();
            Retorno.error = false;

            foreach (int idExclusao in id) {
                bool flagSucesso = this.OContaBancariaMovimentacaoBL.excluir(idExclusao);

                if (!flagSucesso) {
                    Retorno.error = true;
                    Retorno.message = "Alguns registros não puderam ser excluídos.";
                }
            }

            return Json(Retorno);
        }
    }
}