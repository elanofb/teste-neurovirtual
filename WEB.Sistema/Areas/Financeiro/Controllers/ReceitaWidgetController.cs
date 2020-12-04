using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;
using BLL.Services;
using DAL.Financeiro;
using DAL.Permissao.Security.Extensions;
using WEB.Extensions;

namespace WEB.Areas.Financeiro.Controllers {

	public class ReceitaWidgetController : Controller {
        //Constantes

		//Atributos
		private IReceitaConsultaBL _ReceitaConsultaBL;
		private ITituloReceitaPagamentoConsultaBL _TituloReceitaPagamentoConsultaBL;

		//Propriedades
		private IReceitaConsultaBL OReceitaConsultaBL => _ReceitaConsultaBL = this._ReceitaConsultaBL ?? new ReceitaConsultaBL();
		private ITituloReceitaPagamentoConsultaBL OTituloReceitaPagamentoConsultaBL => _TituloReceitaPagamentoConsultaBL = this._TituloReceitaPagamentoConsultaBL ?? new TituloReceitaPagamentoConsultaBL();


		[ActionName("widget-ultimas-transacoes")]
		public PartialViewResult ultimasTransacoes() {

			var listaPagamentosPager = this.OReceitaConsultaBL.listarPagamentos(0)
															.Where(x => x.idStatusPagamento != StatusPagamentoConst.ABERTO)
															.Select(x => new {
																x.idTituloReceitaPagamento, 
																x.idTituloReceita,
																x.idTipoReceita,
																x.descricaoTipoReceita,
																x.idStatusPagamento,
																x.descricaoStatusPagamento,
																x.dtCadastro,
																x.nomePessoa,
																x.valorOriginal,
															})
															.OrderByDescending(x => x.idTituloReceitaPagamento)
															.ToPagedListJsonObject<TituloReceitaPagamentoVW>(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());
			 
			return PartialView("widget-ultimas-transacoes-conteudo", listaPagamentosPager);
		}

        [ActionName("widget-ultimos-pagamentos")]
        public PartialViewResult ultimosPagamentos() {

            var listaPagamentos = this.OTituloReceitaPagamentoConsultaBL.query(User.idOrganizacao())
                                                        .Where(x => x.idStatusPagamento == StatusPagamentoConst.PAGO)
														.Select(x => new {
															x.id,
															x.nroParcela,
															x.dtExclusao,
															x.valorOriginal,
															x.valorJuros,
															x.valorDesconto,
															x.valorDescontoAntecipacao,
															x.valorDescontoCupom,
															x.valorOutrasTarifas,
															x.valorTarifasBancarias,
															x.valorTarifasTransacao,
															x.valorRecebido,
															x.dtPagamento,
															x.descricaoParcela,
															MeioPagamento = new {
																x.MeioPagamento.descricao
															},
															TituloReceita = new {
																x.TituloReceita.nomePessoa,
																x.TituloReceita.descricao
															}
                                                        }).OrderByDescending(x => x.dtPagamento).Take(10).ToListJsonObject<TituloReceitaPagamento>();
	        
            return PartialView("widget-ultimos-pagamentos-conteudo", listaPagamentos);
        }


    }
}