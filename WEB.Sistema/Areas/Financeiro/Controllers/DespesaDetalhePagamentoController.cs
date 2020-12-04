using System;
using System.Linq;
using System.Web.Mvc;
using WEB.Areas.Financeiro.ViewModels;
using BLL.Financeiro;
using BLL.LogsAlteracoes;
using BLL.Services;
using DAL.Entities;
using DAL.Financeiro;
using MvcFlashMessages;

namespace WEB.Areas.Financeiro.Controllers {

    [OrganizacaoFilter]
    public class DespesaDetalhePagamentoController : Controller {

        //Atributos
        private ILogAlteracaoBL           _LogAlteracaoBL;
        private ITituloDespesaPagamentoBL _ContasAPagarPagamentoBL;

        //Propriedades
        private ILogAlteracaoBL           OLogAlteracaoBL           => _LogAlteracaoBL = _LogAlteracaoBL ?? new LogAlteracaoBL();
        private ITituloDespesaPagamentoBL OTituloDespesaPagamentoBL => _ContasAPagarPagamentoBL = _ContasAPagarPagamentoBL ?? new TituloDespesaPagamentoBL();


        //Carrega a lista de pagamento do titulo
        [HttpGet, ActionName("partial-lista-pagamentos")]
        public PartialViewResult partialListaPagamentos(int? idTituloDespesa) {

            var listaTituloDespesaPagamento = this.OTituloDespesaPagamentoBL
                                                  .listar(idTituloDespesa.toInt())
                                                  .Select(x => new {
                                                                       x.dtPagamento,
                                                                       x.dtExclusao,
                                                                       x.descParcela,
                                                                       x.idStatusPagamento,
                                                                       x.dtVencimento,
                                                                       x.dtPrevisaoPagamento,
                                                                       x.id,
                                                                       x.dtBaixa,
                                                                       x.dtDebito,
                                                                       x.dtCompetencia,
                                                                       x.idContaBancaria,
                                                                       x.idCentroCusto,
                                                                       x.idMacroConta,
                                                                       x.idCategoria,
                                                                       x.nroNotaFiscal,
                                                                       x.nroDocumento,
                                                                       x.nroContrato,
                                                                       x.codigoBoleto,
                                                                       x.valorOriginal,
                                                                       x.valorPago,
                                                                       x.valorDesconto,
                                                                       x.valorOutrasTarifas,
                                                                       x.idMeioPagamento,
                                                                       x.idFormaPagamento,
                                                                       x.nroBanco,
                                                                       x.nroAgencia,
                                                                       x.nroDigitoAgencia,
                                                                       x.nroConta,
                                                                       x.nroDigitoConta,
                                                                       x.valorJuros,
                                                                       x.valorMulta,
                                                                       x.codigoAutorizacao,

                                                                       CentroCusto = new {x.CentroCusto.descricao},
                                                                       MacroConta = new {x.MacroConta.descricao},
                                                                       Categoria = new {x.Categoria.descricao},
                                                                       StatusPagamento = new {x.StatusPagamento.descricao},
                                                                       MeioPagamento = new {x.MeioPagamento.descricao},
                                                                       FormaPagamento = new {x.FormaPagamento.descricao},
                                                                       UsuarioBaixa = new {x.UsuarioBaixa.nome},
                                                                   })
                                                  .OrderBy(x => x.dtVencimento)
                                                  .ToListJsonObject<TituloDespesaPagamento>();

            return PartialView(listaTituloDespesaPagamento);
        }

        //Carrega a lista de pagamento do titulo que estão excluídos
        [HttpGet, ActionName("partial-lista-pagamentos-excluidos")]
        public PartialViewResult partialListaPagamentosExcluidos(int? idTituloDespesa) {

            var listaTituloDespesaPagamento = this.OTituloDespesaPagamentoBL
                                                  .listar(idTituloDespesa.toInt(), true)
                                                  .Select(x => new {
                                                                       x.dtPagamento,
                                                                       x.dtExclusao,
                                                                       x.descParcela,
                                                                       x.idStatusPagamento,
                                                                       x.dtVencimento,
                                                                       x.id,
                                                                       x.dtBaixa,
                                                                       x.dtDebito,
                                                                       x.dtCompetencia,
                                                                       x.idCentroCusto,
                                                                       x.idMacroConta,
                                                                       x.idCategoria,
                                                                       x.nroNotaFiscal,
                                                                       x.nroDocumento,
                                                                       x.nroContrato,
                                                                       x.valorOriginal,
                                                                       x.valorPago,
                                                                       x.valorOutrasTarifas,
                                                                       x.idMeioPagamento,
                                                                       x.idFormaPagamento,
                                                                       x.nroBanco,
                                                                       x.nroAgencia,
                                                                       x.nroDigitoAgencia,
                                                                       x.nroConta,
                                                                       x.nroDigitoConta,
                                                                       x.valorJuros,
                                                                       x.valorMulta,
                                                                       x.codigoAutorizacao,
                                                                       x.motivoExclusao,

                                                                       TituloDespesa = new {x.TituloDespesa.dtExclusao},
                                                                       CentroCusto = new {x.CentroCusto.descricao},
                                                                       MacroConta = new {x.MacroConta.descricao},
                                                                       Categoria = new {x.Categoria.descricao},
                                                                       StatusPagamento = new {x.StatusPagamento.descricao},
                                                                       MeioPagamento = new {x.MeioPagamento.descricao},
                                                                       FormaPagamento = new {x.FormaPagamento.descricao},
                                                                       UsuarioBaixa = new {x.UsuarioBaixa.nome},
                                                                       UsuarioExclusao = new {x.UsuarioExclusao.nome}
                                                                   })
                                                  .OrderBy(x => x.dtVencimento)
                                                  .ToListJsonObject<TituloDespesaPagamento>();

            return PartialView(listaTituloDespesaPagamento);
        }

        //Carrega a lista de pagamento do titulo
        [HttpGet, ActionName("modal-log-despesa-pagamento")]
        public PartialViewResult modalLogDespesaPagamento(int? id) {

            var ViewModel = new DespesaLogVM();

            ViewModel.TituloDespesaPagamento = this.OTituloDespesaPagamentoBL.carregar(UtilNumber.toInt32(id), null);

            if (ViewModel.TituloDespesaPagamento == null) {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não foi possível localizar o pagamento");

                return PartialView(ViewModel);
            }

            ViewModel.listaLogAlteracao = OLogAlteracaoBL.listar(EntityTypesConst.TITULO_DESPESA_PAGAMENTO, ViewModel.TituloDespesaPagamento.id, "")
                                                         .OrderByDescending(x => x.dtAlteracao)
                                                         .ThenByDescending(x => x.id)
                                                         .ToList();

            return PartialView(ViewModel);
        }
    }

}
