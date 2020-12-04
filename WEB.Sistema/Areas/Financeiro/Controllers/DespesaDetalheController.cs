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
using WEB.App_Infrastructure;

namespace WEB.Areas.Financeiro.Controllers {
    [OrganizacaoFilter]
    public class DespesaDetalheController : BaseSistemaController {

        private ITituloDespesaBL _TituloDespesaBL;
        private ITituloDespesaBL OTituloDespesaBL
            => _TituloDespesaBL = _TituloDespesaBL
            ?? new TituloDespesaPadraoBL();

        private ILogAlteracaoBL _LogAlteracaoBL;
        private ILogAlteracaoBL OLogAlteracaoBL
            => _LogAlteracaoBL = _LogAlteracaoBL
            ?? new LogAlteracaoBL();

        [HttpGet]
        public ActionResult editar(int? id) {
            var ViewModel = new DespesaForm();
            ViewModel.TituloDespesa = this.OTituloDespesaBL.listar("", null).Select(x => new { x.id, x.descricao, x.dtExclusao }).FirstOrDefault(x => x.id == id).ToJsonObject<TituloDespesa>();
            if (ViewModel.TituloDespesa == null) {
                return RedirectToAction("index", "DespesaCadastro", new {area = "FinanceiroLancamentos", urlRetorno = UtilRequest.getString("urlRetorno") });
            }
            ViewModel.urlRetorno = UtilRequest.getString("urlRetorno");
            return View(ViewModel);
        }

        ///Carrega as informações do titulo por AJAX
        [HttpGet, ActionName("partial-info-titulo")]
        public ActionResult partialInfoTitulo(int? id) {
            var ViewModel = new DespesaForm();
            ViewModel.TituloDespesa = this.OTituloDespesaBL.listar("", null).Select(x => new {
                x.id, x.nroNotaFiscal, x.nroContabil, x.nroContrato, x.nroDocumento,
                x.observacao, x.dtExclusao, x.motivoExclusao, x.dtQuitacao, x.idContaBancaria, x.idCentroCusto, x.idTipoDespesa,
                x.idMacroConta, x.idCategoria, x.valorTotal,x.idModoPagamento,x.idContaBancariaFavorecida,x.idPessoa,
                x.documentoPessoaCredor,
                x.nomePessoaCredor,
                x.nroTelPrincipalCredor,
                x.codigoBoleto,
                
                x.idTituloDespesaOrigem,    
                TituloDespesaOrigem = new {
                    x.TituloDespesaOrigem.descricao
                },

                TipoDespesa = new {x.TipoDespesa.descricao},
                
                UsuarioCadastro = new { x.UsuarioCadastro.nome },
                UsuarioExclusao = new { x.UsuarioExclusao.nome },
                ContaBancaria = new { x.ContaBancaria.descricao },
                CentroCusto = new { x.CentroCusto.descricao },
                MacroConta = new { x.MacroConta.descricao },
                Categoria = new { x.Categoria.descricao },
                ModoPagamentoDespesa = new { x.ModoPagamentoDespesa.descricao, x.ModoPagamentoDespesa.flagContaBancaria },
                
                listaTituloDespesaPagamento = x.listaTituloDespesaPagamento.Where(y => y.dtExclusao == null).Select(y => new {
                    y.dtPagamento, y.dtExclusao, y.valorOriginal, y.valorPago, y.valorJuros, y.valorMulta, y.valorDesconto
                }),
            }).FirstOrDefault(x => x.id == id).ToJsonObject<TituloDespesa>();

            return View(ViewModel);
        }

        //Carrega a lista de pagamento do titulo
        [HttpGet, ActionName("modal-log-despesa")]
        public PartialViewResult modalLogDespesa(int? id) {

            var ViewModel = new DespesaLogVM();

            ViewModel.TituloDespesa = this.OTituloDespesaBL.carregar(UtilNumber.toInt32(id), null);

            if (ViewModel.TituloDespesa == null) {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "Não foi possível localizar o pagamento");
                return PartialView(ViewModel);
            }

            ViewModel.listaLogAlteracao = OLogAlteracaoBL.listar(EntityTypesConst.TITULO_DESPESA, ViewModel.TituloDespesa.id, "").OrderByDescending(x => x.dtAlteracao).ThenByDescending(x => x.id).ToList();
            
            return PartialView(ViewModel);
        }
    }
}
