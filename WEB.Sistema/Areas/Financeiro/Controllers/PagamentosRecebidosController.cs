using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;
using DAL.Financeiro;
using DAL.Permissao.Security.Extensions;
using PagedList;
using WEB.Areas.Financeiro.ViewModels;
using WEB.Helpers;

namespace WEB.Areas.Financeiro.Controllers {

	public class PagamentosRecebidosController : Controller {
        //Constantes

		//Atributos
		private ITituloReceitaBL _TituloReceitaBL;
		private IReceitaConsultaBL _ReceitaConsultaBL;

		//Propriedades
		private ITituloReceitaBL OTituloReceitaBL => _TituloReceitaBL = _TituloReceitaBL ?? new TituloReceitaPadraoBL();
		private IReceitaConsultaBL OReceitaConsultaBL => _ReceitaConsultaBL = _ReceitaConsultaBL ?? new ReceitaConsultaBL();


		[ActionName("listar")]
		public ActionResult listar() {

            var ViewModel = new PagamentosRecebidosVM();

			var queryReceitas = this.montarQuery();

            queryReceitas = queryReceitas.Where(x => x.dtPagamento != null);

			string orderCampo = UtilRequest.getString("orderCampo");
                
			IOrderedQueryable<TituloReceitaPagamentoVW> queryOrder;

			switch (orderCampo) {
				case  "idTituloPagamento":
					queryOrder = queryReceitas.OrderBy(x => x.idTituloReceitaPagamento);
				break;
				case  "idTituloPagamento_desc":
					queryOrder = queryReceitas.OrderByDescending(x => x.idTituloReceitaPagamento);
				break;
				case  "dtPagamento":
					queryOrder = queryReceitas.OrderBy(x => x.dtPagamento);
				break;
				case  "dtPagamento_desc":
					queryOrder = queryReceitas.OrderByDescending(x => x.dtPagamento);
				break;
				case  "dtVencimento":
					queryOrder = queryReceitas.OrderBy(x => x.dtVencimento);
				break;
				case  "dtVencimento_desc":
					queryOrder = queryReceitas.OrderByDescending(x => x.dtVencimento);
				break;
				default:
					queryOrder = queryReceitas.OrderByDescending(x => x.idTituloReceitaPagamento);
				break;
			}

            string idTipoSaida = UtilRequest.getString("idTipoSaida");

			if (idTipoSaida == TipoSaidaHelper.EXCEL) {
				return this.gerarExcel(queryOrder);
			}

            var listaValoresPagamentos = queryOrder.Select(x => new { x.valorOriginal, x.valorTotalTarifas }).ToList();

            ViewModel.valorTotalRecebido = listaValoresPagamentos.Sum(x => x.valorOriginal);

            ViewModel.valorTotalTarifas = listaValoresPagamentos.Sum(x => x.valorTotalTarifas);

			ViewModel.listaPagamentos =	queryOrder.ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());
			
			return View(ViewModel);
		}

        // Geração de Excel
        [ActionName("gerar-excel")]
        public ActionResult gerarExcel(IOrderedQueryable<TituloReceitaPagamentoVW> queryOrder) {

            StringWriter sw = new StringWriter();

            // Montar cabeçalho
	        string cabecalho =
				"Código;Data Transação;Tipo Receita;ID Receita;Descrição;Nome;Valor pagamento;Valor Tarifas;Data Vencimento;Data Pagamento;" +
				"Data Baixa;Data Crédito;Meio de Pagamento;Status;Comprovante";

	        string nomeArquivo = String.Concat("Receitas_", User.id(), "_", Session.SessionID, "_", UtilString.onlyNumber(DateTime.Now.ToString(CultureInfo.InvariantCulture)), ".csv");

            sw.WriteLine(cabecalho);

	        var listaRegistros = queryOrder.ToList();

            foreach (var OItem in listaRegistros) {

                string campos = "{0}";

                for (int i = 1;i < cabecalho.Split(';').Length; i++) {
                    campos = String.Concat(campos, ";{" + i + "}");
                }

				sw.WriteLine(campos, 
					OItem.idTituloReceitaPagamento, OItem.dtCadastro.exibirData(true), OItem.descricaoTipoReceita, OItem.idReceita, OItem.descricaoTitulo(), OItem.nomePessoa, 
					OItem.valorOriginal.ToString("C"), OItem.valorTotalTarifas().ToString("C"), OItem.dtVencimento.exibirData(), OItem.dtPagamento.exibirData(),
					OItem.dtBaixa.exibirData(true), OItem.dtCredito.exibirData(), OItem.descricaoFormaPagamento, OItem.descricaoStatusPagamento, OItem.codigoAutorizacao);
            }


            string urlArquivo = String.Concat(UtilConfig.pathAbsUploadFiles, nomeArquivo);

            if (!System.IO.File.Exists(urlArquivo)) {
                System.IO.File.Create(urlArquivo).Close();
            }

            System.IO.File.AppendAllText(urlArquivo, sw.ToString(), System.Text.Encoding.GetEncoding("iso8859-1"));

			return File(urlArquivo, "text/csv", nomeArquivo);
        }


		[ActionName("detalhe")]
		public ActionResult detalhe(int id) {

			var ViewModel = new ReceitaDetalhe();

			ViewModel.TituloReceita = this.OTituloReceitaBL.carregar(id);

			if (ViewModel.TituloReceita == null) {
				return HttpNotFound();
			}

			ViewModel.listaPagamentos = ViewModel.TituloReceita.listaTituloReceitaPagamento.Where(x => x.dtExclusao == null).ToList();

			ViewModel.listaPagamentosCancelados = ViewModel.TituloReceita.listaTituloReceitaPagamento.Where(x => x.dtExclusao != null).ToList();

			return View(ViewModel);
		}

        #region NONACTION

        [NonAction]
        private IQueryable<TituloReceitaPagamentoVW> montarQuery() {

            int idTipoReceita = UtilRequest.getInt32("idTipoReceita");

            int idMeioPagamento = UtilRequest.getInt32("idMeioPagamento");

			string valorBusca = UtilRequest.getString("valorBusca");

			DateTime? dtCadastroInicio = UtilRequest.getDateTime("dtCadastroInicio");
			
			DateTime? dtCadastroFim = UtilRequest.getDateTime("dtCadastroFim");
			
			DateTime? dtVencimentoInicio = UtilRequest.getDateTime("dtVencimentoInicio");
			
			DateTime? dtVencimentoFim = UtilRequest.getDateTime("dtVencimentoFim");
			
			DateTime? dtPagamentoInicio = UtilRequest.getDateTime("dtPagamentoInicio");
			
			DateTime? dtPagamentoFim = UtilRequest.getDateTime("dtPagamentoFim");
            
			var queryReceitas = OReceitaConsultaBL.listarPagamentos(idTipoReceita);

            if (idMeioPagamento > 0) {
                queryReceitas = queryReceitas.Where(x => x.idMeioPagamento == idMeioPagamento);
            }

			if (dtCadastroInicio.HasValue) {
				queryReceitas = queryReceitas.Where(x => x.dtCadastro >= dtCadastroInicio);
			}

			if (dtCadastroFim.HasValue) {
				DateTime dtFiltro = dtCadastroFim.Value.Date.AddDays(1);
				queryReceitas = queryReceitas.Where(x => x.dtCadastro < dtFiltro);
			}

			if (dtVencimentoInicio.HasValue) {
				queryReceitas = queryReceitas.Where(x => x.dtVencimento >= dtVencimentoInicio);
			}

			if (dtVencimentoFim.HasValue) {
				DateTime dtFiltro = dtVencimentoFim.Value.Date.AddDays(1);
				queryReceitas = queryReceitas.Where(x => x.dtVencimento < dtFiltro);
			}

			if (dtPagamentoInicio.HasValue) {
				queryReceitas = queryReceitas.Where(x => x.dtPagamento >= dtPagamentoInicio);
			}

			if (dtPagamentoFim.HasValue) {
				DateTime dtFiltro = dtPagamentoFim.Value.Date.AddDays(1);
				queryReceitas = queryReceitas.Where(x => x.dtPagamento < dtFiltro);
			}

			if (!String.IsNullOrEmpty(valorBusca)) {
				queryReceitas = queryReceitas.Where(x => x.nomePessoa.Contains(valorBusca) || x.codigoAutorizacao.Contains(valorBusca) || x.descricaoTitulo.Contains(valorBusca));
			}

            return queryReceitas;
        }

        #endregion

    }
}