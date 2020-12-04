using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;
using BLL.LogsAlteracoes;
using BLL.Services;
using DAL.Entities;
using DAL.Financeiro;
using MvcFlashMessages;
using WEB.Areas.Financeiro.ViewModels;

namespace WEB.Areas.Financeiro.Controllers {

    [OrganizacaoFilter]
	public class ReceitaDetalheController : Controller {

        //Atributos
        private TituloReceitaPadraoBL _TituloReceitaPadraoBL;
	    private ILogAlteracaoBL _LogAlteracaoBL;

        //Propriedades
        private TituloReceitaPadraoBL OTituloReceitaBL => this._TituloReceitaPadraoBL = this._TituloReceitaPadraoBL ?? new TituloReceitaPadraoBL();
	    private ILogAlteracaoBL OLogAlteracaoBL => _LogAlteracaoBL = _LogAlteracaoBL ?? new LogAlteracaoBL();

        [HttpGet]
        public ActionResult editar(int? id) {

            var ViewModel = new ReceitaForm();

            ViewModel.TituloReceita = this.OTituloReceitaBL.listar(0, 0, 0, "", null)
                .Select(x => new {
                    x.id,
                    x.idTipoReceita,
                    x.descricao,
                    x.dtQuitacao,
                    x.dtExclusao
                }).FirstOrDefault(x => x.id == id).ToJsonObject<TituloReceita>();

            if (ViewModel.TituloReceita == null) {
                return RedirectToAction("index", "ReceitaCadastro", new { area = "FinanceiroLancamentos", urlRetorno = UtilRequest.getString("urlRetorno") });
            }

            ViewModel.urlRetorno = UtilRequest.getString("urlRetorno");

            
            return View(ViewModel);
        }

        //Carrega as informações do titulo por AJAX
        [HttpGet, ActionName("partial-info-titulo")]
        public ActionResult partialInfoTitulo(int? id) {

            var ViewModel = new ReceitaForm();

            ViewModel.TituloReceita = this.OTituloReceitaBL.listar(0, 0, 0, "", null)
                .Select(x => new {
                    x.id,
                    x.idReceita,
                    x.idTipoReceita,
                    x.descricao,
                    x.nroNotaFiscal,
                    x.nroContabil,
                    x.nomePessoa,
                    x.documentoPessoa,
                    x.nroTelPrincipal,
                    x.nroTelSecundario,
                    x.emailPrincipal,
                    x.nroDocumento,
                    x.nomeRecibo,
                    x.documentoRecibo,
                    x.flagCartaoCreditoPermitido,
                    x.flagBoletoBancarioPermitido,
                    x.flagDepositoPermitido,
                    x.idGatewayPermitido,
                    x.observacao,
                    x.dtExclusao,
                    x.motivoExclusao,
                    x.dtQuitacao,
                    x.idContaBancaria,
                    x.idCentroCusto,
                    x.idMacroConta,
                    x.idCategoria,
                    x.idTabelaImposto,
                    x.valorTotal,
                    x.dtCadastro,
                    x.limiteParcelamento,
                    x.dtLimitePagamento,

                    x.idTituloReceitaOrigem,
                        TituloReceitaOrigem = new {
                        x.TituloReceitaOrigem.descricao
                    },

                    TipoReceita = new { x.TipoReceita.descricao },
                    UsuarioCadastro = new { x.UsuarioCadastro.nome },
                    UsuarioExclusao = new { x.UsuarioExclusao.nome },
                    ContaBancaria = new { x.ContaBancaria.descricao },
                    CentroCusto = new { x.CentroCusto.descricao },
                    MacroConta = new { x.MacroConta.descricao },
                    Categoria = new { x.Categoria.descricao },
                    GatewayPermitido = new { x.GatewayPermitido.descricao },
                    Pessoa = new {
                        x.Pessoa.nome, x.Pessoa.nroDocumento, x.Pessoa.nroTelPrincipal
                    },
                    listaTituloReceitaPagamento = x.listaTituloReceitaPagamento.Where(y => y.dtExclusao == null)
                        .Select(y => new {
                            y.dtPagamento,
                            y.dtExclusao,
                            y.valorOriginal,
                            y.valorRecebido,
                            y.valorDesconto,
                            y.valorDescontoAntecipacao,
                            y.valorDescontoCupom,
                            y.valorJuros,
                            y.valorTarifasBancarias,
                            y.valorTarifasTransacao,
                            y.valorOutrasTarifas
                        }),
                }).FirstOrDefault(x => x.id == id).ToJsonObject<TituloReceita>();


            return View(ViewModel);
        }
        
        //Carrega a lista de pagamento do titulo
        [HttpGet, ActionName("modal-log-receita")]
        public PartialViewResult modalLogDespesa(int? id) {
            var ViewModel = new ReceitaLogVM();

            ViewModel.TituloReceita = this.OTituloReceitaBL.carregar(UtilNumber.toInt32(id), null);
            if (ViewModel.TituloReceita == null) {
                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, "O titulo não pode ser encontrado");
                return PartialView(ViewModel);
            }

            ViewModel.listaLogAlteracao = OLogAlteracaoBL.listar(EntityTypesConst.TITULO_RECEITA, ViewModel.TituloReceita.id, "").OrderByDescending(x => x.dtAlteracao).ThenByDescending(x => x.id).ToList();
            return PartialView(ViewModel);
        }
    }
}