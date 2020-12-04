using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;
using MvcFlashMessages;
using WEB.Areas.Financeiro.ViewModels;
using BLL.LogsAlteracoes;
using BLL.Services;
using DAL.Entities;
using DAL.Financeiro;

namespace WEB.Areas.Financeiro.Controllers {

	public class ReceitaDetalhePagamentosController : Controller {

        //Atributos
	    private ILogAlteracaoBL _LogAlteracaoBL;
	    private ITituloReceitaPagamentoBL _TituloReceitaPagamentoBL;

        //Propriedades
        private ILogAlteracaoBL OLogAlteracaoBL => _LogAlteracaoBL = _LogAlteracaoBL ?? new LogAlteracaoBL();
	    private ITituloReceitaPagamentoBL OTituloReceitaPagamentoBL => _TituloReceitaPagamentoBL = _TituloReceitaPagamentoBL ?? new TituloReceitaPagamentoBL();

        //Carrega a lista de pagamento do titulo
        [HttpGet, ActionName("partial-lista-pagamentos")]
        public PartialViewResult partialListaPagamentos(int? idTituloReceita) {

            var listaTituloDespesaPagamento = this.OTituloReceitaPagamentoBL
                                                .listar(idTituloReceita.toInt())
                                                  .Select(x => new {
                                                    x.dtPagamento, x.dtExclusao,
                                                    x.descricaoParcela, x.idStatusPagamento, x.qtdeParcelas, x.nroParcela,
                                                    x.dtVencimento, x.id, x.dtBaixa, x.dtCredito, x.dtPrevisaoCredito, x.dtCompetencia, x.dtFinalizacaoCheckout, x.idContaBancaria,
                                                    x.idCentroCusto, x.idMacroConta, x.idCategoria, x.nroDocumento, x.valorOriginal, x.valorRecebido, x.valorDesconto, x.valorDescontoCupom, x.valorDescontoAntecipacao, x.valorJuros,
                                                    x.valorTarifasBancarias, x.valorTarifasTransacao, x.valorOutrasTarifas, x.idMeioPagamento, x.idFormaPagamento, x.nroBanco, x.nroAgencia,
                                                    x.nroDigitoAgencia, x.nroConta, x.nroDigitoConta, 
                                                    x.codigoAutorizacao,
                                                    x.tokenTransacao,
                                                    x.flagParcelamento,
                                                    ContaBancaria = new { x.ContaBancaria.descricao, x.ContaBancaria.nroAgencia, x.ContaBancaria.nroConta, OBanco = new { x.ContaBancaria.OBanco.descricao } },
                                                    CentroCusto = new { x.CentroCusto.descricao },
                                                    MacroConta = new { x.MacroConta.descricao },
                                                    Categoria = new { x.Categoria.descricao },
                                                    StatusPagamento = new { x.StatusPagamento.descricao },
                                                    MeioPagamento = new { x.MeioPagamento.descricao },
                                                    FormaPagamento = new { x.FormaPagamento.descricao },
                                                    UsuarioBaixa = new { x.UsuarioBaixa.nome },
                                                    TituloReceita = new { x.TituloReceita.idTipoReceita },

                                            }).OrderBy(x => x.dtVencimento).ToListJsonObject<TituloReceitaPagamento>();

            int contParcelas = 1;
                
            foreach (var OParcela in listaTituloDespesaPagamento){
                
                OParcela.descricaoParcela = !OParcela.descricaoParcela.isEmpty() ? OParcela.descricaoParcela : $"Parcela {contParcelas}";                

                contParcelas++;
            }
            
            return PartialView(listaTituloDespesaPagamento);
        }

        //Carrega a lista de pagamento do titulo
        [HttpGet, ActionName("partial-lista-pagamentos-excluidos")]
        public PartialViewResult partialListaPagamentosExcluidos(int? idTituloReceita){

            var listaTituloDespesaPagamento = this.OTituloReceitaPagamentoBL
                                                .listar(idTituloReceita.toInt(), true)
                                                .Select(x => new {
                                                    x.dtPagamento, x.dtExclusao, x.descricaoParcela, x.idStatusPagamento, x.dtVencimento,
                                                    x.id, x.dtBaixa, x.dtCredito, x.dtCompetencia, x.dtFinalizacaoCheckout,
                                                    x.idCentroCusto, x.idMacroConta, x.idCategoria, x.nroDocumento,
                                                    x.valorOriginal, x.valorRecebido, x.valorDesconto, x.valorJuros,
                                                    x.valorTarifasBancarias, x.valorTarifasTransacao, x.valorOutrasTarifas,
                                                    x.idMeioPagamento, x.idFormaPagamento, x.nroBanco, x.nroAgencia,
                                                    x.nroDigitoAgencia, x.nroConta, x.nroDigitoConta, x.codigoAutorizacao, x.motivoExclusao,

                    CentroCusto = new { x.CentroCusto.descricao },
                    MacroConta = new { x.MacroConta.descricao },
                    Categoria = new { x.Categoria.descricao },
                    StatusPagamento = new { x.StatusPagamento.descricao },
                    MeioPagamento = new { x.MeioPagamento.descricao },
                    FormaPagamento = new { x.FormaPagamento.descricao },
                    UsuarioBaixa = new { x.UsuarioBaixa.nome },
                    TituloReceita = new { x.TituloReceita.idTipoReceita },
                    UsuarioExclusao = new { x.UsuarioExclusao.nome }

                }).OrderBy(x => x.dtVencimento).ToListJsonObject<TituloReceitaPagamento>();

            return PartialView(listaTituloDespesaPagamento);
        }

        //Carrega a lista de pagamento do titulo
        [HttpGet, ActionName("modal-log-receita-pagamento")]
        public PartialViewResult modalLogReceitaPagamento(int? id) {

            var ViewModel = new ReceitaLogVM();

            ViewModel.TituloReceitaPagamento = this.OTituloReceitaPagamentoBL.carregar(UtilNumber.toInt32(id), null);

            if (ViewModel.TituloReceitaPagamento == null) {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não foi possível localizar o titulo");
                return PartialView(ViewModel);
            }

            ViewModel.listaLogAlteracao = OLogAlteracaoBL.listar(EntityTypesConst.TITULO_RECEITA_PAGAMENTO, ViewModel.TituloReceitaPagamento.id, "").OrderByDescending(x => x.dtAlteracao).ThenByDescending(x => x.id).ToList();
            return PartialView(ViewModel);
        }

    }
}