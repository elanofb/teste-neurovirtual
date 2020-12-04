using System;
using System.Linq;
using DAL.Financeiro;
using DAL.Repository.Base;
using EntityFramework.Extensions;
using System.Json;
using BLL.Caches;
using BLL.Impostos;
using BLL.Services;
using DAL.Impostos;
using DAL.Permissao.Security.Extensions;
using UTIL.Resources;

namespace BLL.Financeiro {

	public class CalculadorImpostoTituloBL : DefaultBL, ICalculadorImpostoTituloBL {

		// Atributos
		private ITituloReceitaConsultaBL _TituloReceitaConsultaBL;
		private ITituloDespesaBL _TituloDespesaBL;
		private ITabelaImpostoConsultaBL _TabelaImpostoConsultaBL;
		private ITituloImpostoBL _TituloImpostoBL;

		// Propriedades
		private ITituloReceitaConsultaBL OTituloReceitaConsultaBL => _TituloReceitaConsultaBL = _TituloReceitaConsultaBL ?? new TituloReceitaConsultaBL();
		private ITituloDespesaBL OTituloDespesaBL => _TituloDespesaBL = _TituloDespesaBL ?? new TituloDespesaPadraoBL();
		private ITabelaImpostoConsultaBL OTabelaImpostoConsultaBL => _TabelaImpostoConsultaBL = _TabelaImpostoConsultaBL ?? new TabelaImpostoConsultaBL();
		private ITituloImpostoBL OTituloImpostoBL => _TituloImpostoBL = _TituloImpostoBL ?? new TituloImpostoBL();
		
		//
		public CalculadorImpostoTituloBL(){
		}

		public UtilRetorno calcularImpostoTitulo(int idTabelaImposto, TituloImposto OTituloImposto) {

			var ORetorno = UtilRetorno.newInstance(false);
			
			decimal valorBase = 0;
			
			if (OTituloImposto.idTituloDespesa > 0) {
				valorBase = OTituloDespesaBL.carregar(OTituloImposto.idTituloDespesa.toInt()).valorTotalComDesconto();
				OTituloImposto.idTituloReceita = null;
			}
			
			if (OTituloImposto.idTituloReceita > 0) {
				valorBase = OTituloReceitaConsultaBL.carregar(OTituloImposto.idTituloReceita.toInt()).valorTotalComDesconto();
				OTituloImposto.idTituloDespesa = null;
			}

			if (valorBase <= 0) {

				ORetorno.flagError = true;
				ORetorno.listaErros.Add("É necessário que o valor do título seja maior que zero.");
				
				return ORetorno;
			}

			var OTabela = OTabelaImpostoConsultaBL.carregar(idTabelaImposto);

			if (OTabela == null) {
				
				ORetorno.flagError = true;
				ORetorno.listaErros.Add("Não foi possível encontrar a tabela de imposto");
				
				return ORetorno;
			}

			OTituloImposto.percentualCOFINS = OTabela.percentualCOFINS;
			OTituloImposto.valorCOFINS = (OTabela.percentualCOFINS / 100) * valorBase;

			OTituloImposto.percentualICMS = OTabela.percentualICMS;
			OTituloImposto.valorICMS = (OTabela.percentualICMS / 100) * valorBase;

			OTituloImposto.percentualISS = OTabela.percentualISS;
			OTituloImposto.valorISS = (OTabela.percentualISS / 100) * valorBase;

			OTituloImposto.percentualPIS = OTabela.percentualPIS;
			OTituloImposto.valorPIS = (OTabela.percentualPIS / 100) * valorBase;

			var flagSucesso = OTituloImpostoBL.salvar(OTituloImposto);

			if (flagSucesso) {
				ORetorno.flagError = false;
				ORetorno.listaErros.Add("Dados de impostos do titulo preenchidos com sucesso.");
				
				return ORetorno;
			}

			ORetorno.flagError = true;
			ORetorno.listaErros.Add("Erro ao preencher dados de impostos do titulo.");
				
			return ORetorno;
		}
		
    }
}