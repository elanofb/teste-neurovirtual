using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;
using BLL.LogsAlteracoes;
using MvcFlashMessages;
using BLL.Core.Events;
using BLL.Financeiro.Events;
using DAL.Financeiro;
using WEB.Areas.Financeiro.ViewModels;
using MoreLinq;
using BLL.Services;
using UTIL.UtilClasses;

namespace WEB.Areas.Financeiro.Controllers {

    [OrganizacaoFilter]
	public class ReceitaDetalhePagamentosOperacaoController : Controller {

        //Atributos
        private TituloReceitaPadraoBL _TituloReceitaPadraoBL;
        private ITituloReceitaPagamentoBL _TituloReceitaPagamentoBL;
        private ILogTituloReceitaPagamentoBL _LogTituloReceitaPAgamentoBL;
        private ITituloReceitaPagamentoExclusaoBL _TituloReceitaPagamentoExclusaoBL;
        private ITituloReceitaPagamentoCancelamentoBL _TituloReceitaPagamentoCancelamentoBL;

        //Propriedades
        private ITituloReceitaPagamentoBaixaBL OTituloReceitaPagamentoBaixaBL { get; }
        private TituloReceitaPadraoBL OTituloReceitaBL => this._TituloReceitaPadraoBL = this._TituloReceitaPadraoBL ?? new TituloReceitaPadraoBL();
        private ITituloReceitaPagamentoBL OTituloReceitaPagamentoBL => this._TituloReceitaPagamentoBL = this._TituloReceitaPagamentoBL ?? new TituloReceitaPagamentoBL();
        private ILogTituloReceitaPagamentoBL OLogTituloReceitaPagamentoBL => _LogTituloReceitaPAgamentoBL = _LogTituloReceitaPAgamentoBL ?? new LogTituloReceitaPagamentoBL();
        private ITituloReceitaPagamentoExclusaoBL OTituloReceitaPagamentoExclusaoBL => _TituloReceitaPagamentoExclusaoBL = _TituloReceitaPagamentoExclusaoBL ?? new TituloReceitaPagamentoExclusaoBL();
        private ITituloReceitaPagamentoCancelamentoBL OTituloReceitaPagamentoCancelamentoBL => _TituloReceitaPagamentoCancelamentoBL = _TituloReceitaPagamentoCancelamentoBL ?? new TituloReceitaPagamentoCancelamentoBL();
        
        /// <summary>
        /// Construtor
        /// </summary>
        public ReceitaDetalhePagamentosOperacaoController(ITituloReceitaPagamentoBaixaBL _TituloReceitaPagamentoBaixaBL) {

            this.OTituloReceitaPagamentoBaixaBL = _TituloReceitaPagamentoBaixaBL;

        }
        
        //Events
        private readonly EventAggregator onAtualizarValorTituloReceita = OnAtualizarValorTituloReceita.getInstance;

        /// <summary>
        /// Altera os campos dos pagamentos individualmente, gravando o log do mesmo
        /// </summary>
        [HttpPost, ActionName("alterar-dados-pagamentos")]
        public ActionResult alterarDadosPagamentos(EditableItem ViewModel) {
            
            var Retorno = OLogTituloReceitaPagamentoBL.alterarCampo(ViewModel.pk.toInt(), ViewModel.name, ViewModel.value, ViewModel.nomeCampoDisplay, ViewModel.oldValue, ViewModel.newValue);
            
            if (!Retorno.flagError) {

                if (ViewModel.name == "valorOriginal" && Retorno.flagError != true) {

                    this.onAtualizarValorTituloReceita.subscribe(new OnAtualizarValorTituloReceitaHandler());

                    this.onAtualizarValorTituloReceita.publish(Retorno.info);

                }

                var listaOutrosPagamentos = this.OTituloReceitaPagamentoBL.listar(Retorno.info.toInt()).Select(x => new { x.dtVencimento, x.id }).ToList();

                var id = ViewModel.pk.toInt();

                var OPagamento = listaOutrosPagamentos.FirstOrDefault(y => y.id == id);

                var flagHabilitarAtualizarTodos = listaOutrosPagamentos.Count() > 1;
                var flagHabilitarAtualizarProximos = listaOutrosPagamentos.Any(x => x.dtVencimento > OPagamento.dtVencimento || (OPagamento.id > x.id && OPagamento.dtVencimento == x.dtVencimento));

                return Json(new {
                    Retorno.flagError,
                    Retorno.listaErros,
                    flagHabilitarAtualizarTodos,
                    flagHabilitarAtualizarProximos,
                    ViewModel.nomeCampoDisplay,
                    ViewModel.oldValue,
                    ViewModel.newValue
                });

            }

            return Json(new { Retorno.flagError, Retorno.listaErros});
        }

        /// <summary>
        /// Altera os campos dos pagamentos em lote de acordo com a opção selecionada
        /// Quando é realizado a alteração de um pagamento é questionado se deve alterar todos
        ///     somente aquele ou aquele e os posteriores a ele 
        /// </summary>
        [HttpPost, ActionName("alterar-dados-outros-pagamentos")]
        public ActionResult alterarDadosOutrosPagamentos() {

            var id = UtilRequest.getInt32("id");
            var tipoEdit = UtilRequest.getString("tipoEdit");
            var nomeCampo = UtilRequest.getString("campo").Trim();
            var valorCampo = UtilRequest.getString("value").Trim();
            var nomeCampoDisplay = UtilRequest.getString("nomeCampoDisplay").Trim();
            var oldValue = UtilRequest.getString("oldValue").Trim();
            var newValue = UtilRequest.getString("newValue").Trim();

            var OTituloReceita = OTituloReceitaBL.listar(0, 0, 0, "").Where(x => x.listaTituloReceitaPagamento.Any(y => y.id == id && y.dtExclusao == null)).Select(x => new {
                x.id,
                listaTituloReceitaPagamento = x.listaTituloReceitaPagamento.Where(y => y.dtExclusao == null).Select(y => new { y.id, y.dtVencimento })
            }).FirstOrDefault().ToJsonObject<TituloReceita>();

            if (OTituloReceita == null) {
                return Json(UtilRetorno.newInstance(true, "Registro não localizado"));
            }

            var pagamentoAtual = OTituloReceita.listaTituloReceitaPagamento.FirstOrDefault(x => x.id == id);
            var listaPagamentos = OTituloReceita.listaTituloReceitaPagamento.Where(x => x.id != id).ToList();

            if (tipoEdit == "NEXT") {
                listaPagamentos = listaPagamentos.Where(x => x.dtVencimento > pagamentoAtual.dtVencimento || (pagamentoAtual.id > x.id && pagamentoAtual.dtVencimento == x.dtVencimento)).ToList();
            }

            if (!listaPagamentos.Any()) {
                return Json(UtilRetorno.newInstance(true, "Nenhum registro para ser alterado"));
            }

            var ORetorno = UtilRetorno.newInstance(false);

            foreach (var OPagamento in listaPagamentos) {
                var Retorno = OLogTituloReceitaPagamentoBL.alterarCampo(OPagamento.id, nomeCampo, valorCampo, "", nomeCampoDisplay, oldValue, newValue);

                if (Retorno.flagError) {
                    ORetorno.flagError = true;
                    ORetorno.listaErros.Add("Pagamento #" + OPagamento.id + " - " + Retorno.listaErros.FirstOrDefault());
                }
            }

            if (nomeCampo == "valorOriginal") {
                this.onAtualizarValorTituloReceita.subscribe(new OnAtualizarValorTituloReceitaHandler());
                this.onAtualizarValorTituloReceita.publish(OTituloReceita.id as object);
            }

            return Json(ORetorno);
        }

        
        /// <summary>
        /// Abre a modal pra registrar o pagamento da parcela
        /// </summary>
        [HttpPost, ActionName("modal-registrar-pagamento")]
        public ActionResult modelRegistrarPagamento() {

            var ViewModel = new BaixaTituloReceitaPagamentoForm();

            List<int> ids = UtilRequest.getListInt("id");

            ViewModel.TituloReceitaPagamento = new TituloReceitaPagamento();
            
            ViewModel.TituloReceitaPagamento.TituloReceita = new TituloReceita();

            ViewModel.listaTituloReceitaPagamento = this.OTituloReceitaPagamentoBL.listar(0)
                                                            .Where(x => ids.Contains(x.id) && 
                                                                        x.dtPagamento == null && 
                                                                        x.TituloReceita.dtExclusao == null
                                                            ).Select(x => new {
                                                                                 x.id, 
                                                                                 x.idTituloReceita,
                                                                                  x.valorOriginal,
                                                                                  x.valorRecebido,
                                                                                  x.descricaoParcela,
                                                                                  x.dtVencimento,
                                                                                  x.dtVencimentoOriginal,
                                                                                 TituloReceita = new {
                                                                                                     x.TituloReceita.id,
                                                                                                     x.TituloReceita.idTipoReceita,
                                                                                                     x.TituloReceita.idReceita,
                                                                                                     x.TituloReceita.dtQuitacao,
                                                                                                     x.TituloReceita.descricao
                                                                                 }
                                                             }).ToListJsonObject<TituloReceitaPagamento>();

            if (!ViewModel.listaTituloReceitaPagamento.Any()) {
                
                this.Flash(UtilMessage.TYPE_MESSAGE_INFO, "Nenhum registro localizado");
                
                return PartialView(ViewModel);
            }

            var qtdeTituloReceita = ViewModel.listaTituloReceitaPagamento.DistinctBy(x => x.idTituloReceita).Count(); 
            
            var OTituloReceita = qtdeTituloReceita == 1 ? ViewModel.listaTituloReceitaPagamento.Select(x => x.TituloReceita).FirstOrDefault() : new TituloReceita();

            ViewModel.TituloReceitaPagamento.TituloReceita = OTituloReceita;

            return PartialView(ViewModel);
        }

        /// <summary>
        /// Registra o pagamento da parcela
        /// </summary>
        [HttpPost, ActionName("registrar-pagamento")]
        public ActionResult registrarPagamento(BaixaTituloReceitaPagamentoForm ViewModel) {

            if (!ModelState.IsValid) {
                
                return PartialView("modal-registrar-pagamento", ViewModel);
            }

            foreach (var Item in ViewModel.listaTituloReceitaPagamento) {
                                                              
                Item.dtPagamento = ViewModel.TituloReceitaPagamento.dtPagamento;
                
                Item.dtPrevisaoCredito = ViewModel.TituloReceitaPagamento.dtCredito;
                
                Item.dtCredito = ViewModel.TituloReceitaPagamento.dtCredito;
                
                Item.idMeioPagamento = ViewModel.TituloReceitaPagamento.idMeioPagamento;
                
                Item.idFormaPagamento = ViewModel.TituloReceitaPagamento.idFormaPagamento;
                
                Item.valorJuros = ViewModel.TituloReceitaPagamento.valorJuros;
                
                Item.valorRecebido = ViewModel.TituloReceitaPagamento.valorRecebido;
                
                Item.valorOutrasTarifas = ViewModel.TituloReceitaPagamento.valorOutrasTarifas;
                
                Item.nroBanco = ViewModel.TituloReceitaPagamento.nroBanco;
                
                Item.nroDocumento = ViewModel.TituloReceitaPagamento.nroDocumento;
                
                Item.nroAgencia = ViewModel.TituloReceitaPagamento.nroAgencia;
                
                Item.nroDigitoAgencia = ViewModel.TituloReceitaPagamento.nroDigitoAgencia;
                
                Item.nroConta = ViewModel.TituloReceitaPagamento.nroConta;
                
                Item.nroDigitoConta = ViewModel.TituloReceitaPagamento.nroDigitoConta;
                
                Item.codigoAutorizacao = ViewModel.TituloReceitaPagamento.codigoAutorizacao;
                
                Item.valorTarifasBancarias = ViewModel.TituloReceitaPagamento.valorTarifasBancarias;
                
                Item.flagBaixaAutomatica = false;
            }

            var ORetorno = UtilRetorno.newInstance(false);

            foreach (var OTituloReceitaPagamento in ViewModel.listaTituloReceitaPagamento) {
                var Retorno = this.OTituloReceitaPagamentoBaixaBL.registrarPagamento(OTituloReceitaPagamento);

                if (Retorno.flagError) {
                    ORetorno.flagError = true;
                }

                ORetorno.listaErros.Add($"Pagamento #{OTituloReceitaPagamento.id} - " + Retorno.listaErros.FirstOrDefault());
            }

            if (ORetorno.flagError) {
                this.Flash(UtilMessage.TYPE_MESSAGE_INFO, string.Join("<br/>", ORetorno.listaErros));
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, string.Join("<br/>", ORetorno.listaErros));

            return PartialView("modal-registrar-pagamento", ViewModel);
        }

        //Listar as anuidades pendentes do associado
        [HttpPost, ActionName("cancelar-pagamento")]
        public ActionResult cancelarPagamento() {

            var id = UtilRequest.getInt32("id");

            var ORetorno = this.OTituloReceitaPagamentoCancelamentoBL.cancelarPagamento(id);

            return Json(ORetorno);
        }

        [HttpGet, ActionName("modal-excluir-receita-pagamento")]
        public ActionResult modalExcluirDespesaPagamento(int? id) {

            var ViewModel = new ReceitaPagamentoExcluirForm();

            ViewModel.TituloReceitaPagamento = this.OTituloReceitaPagamentoBL.carregar(id.toInt());
            
            if (ViewModel.TituloReceitaPagamento == null) {
                return Json(new { flagErro = true, message = "O pagamento não pode ser localizada" }, JsonRequestBehavior.AllowGet);
            }

            if (ViewModel.TituloReceitaPagamento.TituloReceita.dtExclusao.HasValue) {
                return Json(new { flagErro = true, message = "Não é possível remover uma parcela de receita excluída" }, JsonRequestBehavior.AllowGet);
            }

            if (ViewModel.TituloReceitaPagamento.TituloReceita.dtQuitacao.HasValue) {
                return Json(new { flagErro = true, message = "Não é possível remover uma parcela de receita quitada" }, JsonRequestBehavior.AllowGet);
            }

            var listaOutrosPagamentos = this.OTituloReceitaPagamentoBL.listar(ViewModel.TituloReceitaPagamento.idTituloReceita)
                .Select(x => new { x.dtVencimento, x.id }).ToList();

            ViewModel.flagHabilitarAtualizarTodos = listaOutrosPagamentos.Count() > 1;
            ViewModel.flagHabilitarAtualizarProximos = listaOutrosPagamentos.Any(x => x.dtVencimento > ViewModel.TituloReceitaPagamento.dtVencimento || (x.dtVencimento == ViewModel.TituloReceitaPagamento.dtVencimento && x.id > ViewModel.TituloReceitaPagamento.id));

            return View(ViewModel);
        }


        [HttpPost, ActionName("excluir-receita-pagamento")]
        public ActionResult excluir(ReceitaPagamentoExcluirForm ViewModel) {

            if (!ModelState.IsValid) {
                return View("modal-excluir-receita-pagamento", ViewModel);
            }

            var ORetorno = this.OTituloReceitaPagamentoExclusaoBL.excluirPagamento(ViewModel.TituloReceitaPagamento.id, ViewModel.TituloReceitaPagamento.motivoExclusao, false, ViewModel.flagAtualizarOutros);
            
            if (!ORetorno.flagError) {

                this.onAtualizarValorTituloReceita.subscribe(new OnAtualizarValorTituloReceitaHandler());
                this.onAtualizarValorTituloReceita.publish(ViewModel.TituloReceitaPagamento.idTituloReceita as object);

                return Json(ORetorno);
            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, ORetorno.listaErros.FirstOrDefault());
            return View("modal-excluir-receita-pagamento", ViewModel);
        }
    }
}