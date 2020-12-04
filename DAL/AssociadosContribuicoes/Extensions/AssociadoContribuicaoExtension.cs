using System;
using System.Linq;
using DAL.AssociadosContribuicoes.DTO;

namespace DAL.AssociadosContribuicoes {

	public static class AssociadoContribuicaoExtension {

		//Verificar situacao atual da contribuicao
        public static SituacaoContribuicaoEnum situacaoAtual(this AssociadoContribuicao OContribuicao){
			
			SituacaoContribuicaoEnum situacao = SituacaoContribuicaoEnum.PENDENTE;

			if (OContribuicao.flagIsento == true) {
				return SituacaoContribuicaoEnum.ISENTO;
			}

			if (OContribuicao.dtPagamento.HasValue) { 
				return SituacaoContribuicaoEnum.PAGO;
			}

			return situacao;
        }

		//retornar texto de acordo com a situacao atuacao
        public static string descricaoSituacao(this AssociadoContribuicao OContribuicao){
			
			SituacaoContribuicaoEnum situacao = OContribuicao.situacaoAtual();

			if (situacao == SituacaoContribuicaoEnum.PENDENTE) {
				return "Pendente";
			}
	
			if (situacao == SituacaoContribuicaoEnum.ISENTO) {
				return "Isento";
			}

			if (situacao == SituacaoContribuicaoEnum.PAGO) {
				return "Pago";
			}
			

			return String.Empty;
        }

		//retornar texto de acordo com a situacao atuacao
        public static string detalheParcelamento(this AssociadoContribuicao OContribuicao){
			
			if (OContribuicao == null) {
				return "-";
			}
	
			if (OContribuicao.TituloReceita == null) {
				return "-";
			}

            var listaParcelas = OContribuicao.TituloReceita.listaTituloReceitaPagamento.Where(x => x.dtExclusao == null).ToList();

			if (!listaParcelas.Any()) {
				return "-";
			}

            int qtdePagas = listaParcelas.Count(x => x.dtPagamento != null);

            string detalhes = $"Pago <strong>{qtdePagas}</strong> de <strong>{listaParcelas.Count}</strong>";

            decimal? valorPago = listaParcelas.Where(x => x.dtPagamento != null).Sum(x => x.valorRecebido);

            decimal saldo = decimal.Subtract(UtilNumber.toDecimal(OContribuicao.valorAtual), UtilNumber.toDecimal(valorPago));

            if (saldo > 0) {
                detalhes = string.Concat(detalhes, " <span class='fs-10 text-italic'>Saldo: ", saldo.ToString("C"), "</span>");
            }

			return detalhes;
        }

        //retornar texto de acordo com a situacao atuacao
        public static bool flagParcelamento(this AssociadoContribuicao OContribuicao){
			
			if (OContribuicao == null) {
				return false;
			}
	
			if (OContribuicao.TituloReceita == null) {
				return false;
			}

            var listaPagamentos = OContribuicao.TituloReceita.listaTituloReceitaPagamento
                                                            .Where(x => x.dtExclusao == null)
                                                            .ToList();

            if (!listaPagamentos.Any()) {
                return false;
            }

            var valorParcelado = listaPagamentos.Sum(x => x.valorOriginal);

            if (valorParcelado < OContribuicao.TituloReceita.valorTotal) {
                return true;
            }

            if (listaPagamentos.Count == 1 && valorParcelado >= OContribuicao.TituloReceita.valorTotal) {
                return false;
            }

            return true;
        }

        //retornar texto de acordo com a situacao atuacao
        public static bool flagEmAtraso(this AssociadoContribuicao OContribuicao){
			
			if (OContribuicao == null) {
				return false;
			}
	
			if (OContribuicao.dtVencimentoAtual < DateTime.Today) {
				return true;
			}

            return false;
        }

        //retornar texto de acordo com a situacao atuacao
        public static bool flagEmAtraso(this AssociadoContribuicaoDadosBasicos OContribuicao){
			
			if (OContribuicao == null) {
				return false;
			}
	
			if (OContribuicao.dtPagamento.HasValue || OContribuicao.flagIsento == true) {
				return false;
			}
	
			if (OContribuicao.dtVencimentoAtual < DateTime.Today) {
				return true;
			}

            return false;
        }


        //retornar texto de acordo com a situacao atuacao
        public static bool flagEmAtraso(this AssociadoContribuicaoResumoVW OContribuicao){
			
			if (OContribuicao == null) {
				return false;
			}
	
			if (OContribuicao.dtPagamento.HasValue || OContribuicao.flagIsento == true) {
				return false;
			}
	
			if (OContribuicao.dtVencimentoAtual < DateTime.Today) {
				return true;
			}

            return false;
        }
	}
}
