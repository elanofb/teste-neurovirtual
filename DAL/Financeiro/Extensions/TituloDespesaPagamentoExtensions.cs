using System;

namespace DAL.Financeiro {

	public static class TituloDespesaPagamentoExtensions {
		
		//Calcular valor total das tarifas
		public static decimal valorTotalAcrescimos(this TituloDespesaPagamento OPagamento) {

			decimal valorTotal = new decimal(0);

			if (OPagamento == null) {
				return valorTotal;
			}
			if (OPagamento.valorMulta > 0) {
				valorTotal = decimal.Add(valorTotal, OPagamento.valorMulta.toDecimal());
			}
			if (OPagamento.valorJuros > 0) {
				valorTotal = decimal.Add(valorTotal, OPagamento.valorJuros.toDecimal());
			}

			return valorTotal;
		}

        //Calcular valor total das tarifas
        public static decimal valorTotalComAcrescimos(this TituloDespesaPagamento OPagamento) {

            decimal valorTotal = new decimal(0);

            if (OPagamento == null) {
                return valorTotal;
            }

            valorTotal = OPagamento.valorOriginal;

            if (OPagamento.valorMulta > 0) {
                valorTotal = decimal.Add(valorTotal, OPagamento.valorMulta.toDecimal());
            }
            if (OPagamento.valorJuros > 0) {
                valorTotal = decimal.Add(valorTotal, OPagamento.valorJuros.toDecimal());
            }
            if (OPagamento.valorOutrasTarifas > 0) {
                valorTotal = decimal.Add(valorTotal, OPagamento.valorOutrasTarifas.toDecimal());
            }
            return valorTotal;
        }
        

        //Calcular valor total das tarifas
        public static decimal valorFinalCalculado(this TituloDespesaPagamento OPagamento) {

            decimal valorTotal = OPagamento.valorTotalComAcrescimos();

            decimal valorDescontos = OPagamento.valorDesconto.toDecimal();

            decimal valorFinal = decimal.Subtract(valorTotal, valorDescontos);
            
            if (valorFinal < 0) {
                return new decimal(0);
            }
            
            return valorFinal;
        }        


        //Definir forma de pagamento
        public static byte? definirFormaPagamento(this TituloDespesaPagamento OPagamento) {

            if(OPagamento.idMeioPagamento == MeioPagamentoConst.BOLETO_BANCARIO) {
                return FormaPagamentoConst.BOLETO_BANCARIO;
            }

            if(OPagamento.idMeioPagamento == MeioPagamentoConst.DINHEIRO) {
                return FormaPagamentoConst.DINHEIRO;
            }

            if(OPagamento.idMeioPagamento == MeioPagamentoConst.DEPOSITO_BANCARIO) {
                return FormaPagamentoConst.DEPOSITO_BANCARIO;
            }

            if(OPagamento.idMeioPagamento == MeioPagamentoConst.CHEQUE) {
                return FormaPagamentoConst.CHEQUE;
            }

            if(OPagamento.idMeioPagamento == MeioPagamentoConst.TRANSFERENCIA_ELETRONICA) {
                return FormaPagamentoConst.TRANSFERENCIA_BANCARIA;
            }

            if(OPagamento.idMeioPagamento == MeioPagamentoConst.GUIA) {
                return FormaPagamentoConst.GUIA;
            }

            if(OPagamento.idMeioPagamento == MeioPagamentoConst.DEBITO_CONTA) {
                return FormaPagamentoConst.DEBITO_CONTA;
            }

            if(OPagamento.idMeioPagamento == MeioPagamentoConst.CARTAO_CREDITO || OPagamento.idMeioPagamento == MeioPagamentoConst.CARTAO_DEBITO) {
                return Convert.ToByte(OPagamento.idFormaPagamento);
            }

            return 0;
        }

        //Borda
        public static string exibirBordaStatus(this TituloDespesaPagamento OPagamento) {

            if (OPagamento.dtPagamento == null && OPagamento.dtVencimento < DateTime.Today) {
                return "border-red";
            }

            string descricaoAtivo = (OPagamento.idStatusPagamento == StatusPagamentoConst.PAGO ? "border-green" : (OPagamento.idStatusPagamento == StatusPagamentoConst.CANCELADO || OPagamento.idStatusPagamento == StatusPagamentoConst.ESTORNADO ? "border-red" : "border-yellow"));

            return descricaoAtivo;
        }

        //Icone FA situacao financeira Associado
        public static string exibirIconeStatus(this TituloDespesaPagamento OPagamento) {

            if (OPagamento.dtPagamento == null && OPagamento.dtVencimento < DateTime.Today) {
                return "fa-times-circle";
            }

            string descricaoAtivo = (OPagamento.idStatusPagamento == StatusPagamentoConst.PAGO ? "fa-check" : (OPagamento.idStatusPagamento == StatusPagamentoConst.CANCELADO || OPagamento.idStatusPagamento == StatusPagamentoConst.ESTORNADO ? "fa-times-circle" : "fa-exclamation"));

            return descricaoAtivo;
        }

        //Classes CSS situacao financeira Associado
        public static string exibirClasseStatus(this TituloDespesaPagamento OPagamento) {

            if (OPagamento.dtPagamento == null && OPagamento.dtVencimento < DateTime.Today) {
                return "text-red";
            }

            string descricaoAtivo = (OPagamento.idStatusPagamento == StatusPagamentoConst.PAGO ? "text-green" : (OPagamento.idStatusPagamento == StatusPagamentoConst.CANCELADO || OPagamento.idStatusPagamento == StatusPagamentoConst.ESTORNADO ? "text-red" : "text-yellow"));

            return descricaoAtivo;
        }




        //Borda
        public static string exibirBordaStatus(this TituloDespesaPagamentoResumoVW OPagamento) {

            if (OPagamento.dtPagamento == null && OPagamento.dtVencimentoDespesa < DateTime.Today) {
                return "border-red";
            }

            return (OPagamento.idStatusPagamento == StatusPagamentoConst.PAGO ? "border-green" : (OPagamento.idStatusPagamento == StatusPagamentoConst.CANCELADO || OPagamento.idStatusPagamento == StatusPagamentoConst.ESTORNADO ? "border-red" : "border-yellow"));
        }

        //Borda
        public static string exibirCorBordaStatus(this TituloDespesaPagamentoResumoVW OPagamento) {

            if (OPagamento.dtPagamento == null && OPagamento.dtVencimentoDespesa < DateTime.Today) {
                return "red";
            }

            return (OPagamento.idStatusPagamento == StatusPagamentoConst.PAGO ? "green" : (OPagamento.idStatusPagamento == StatusPagamentoConst.CANCELADO || OPagamento.idStatusPagamento == StatusPagamentoConst.ESTORNADO ? "red" : "yellow"));
        }

        //Icone FA situacao financeira Associado
        public static string exibirIconeStatus(this TituloDespesaPagamentoResumoVW OPagamento) {

            if (OPagamento.dtPagamento == null && OPagamento.dtVencimentoDespesa < DateTime.Today) {
                return "fa-times-circle";
            }

            return (OPagamento.idStatusPagamento == StatusPagamentoConst.PAGO ? "fa-check" : (OPagamento.idStatusPagamento == StatusPagamentoConst.CANCELADO || OPagamento.idStatusPagamento == StatusPagamentoConst.ESTORNADO ? "fa-times-circle" : "fa-exclamation"));
        }

        //Classes CSS situacao financeira Associado
        public static string exibirClasseStatus(this TituloDespesaPagamentoResumoVW OPagamento) {

            if (OPagamento.dtPagamento == null && OPagamento.dtVencimentoDespesa < DateTime.Today) {
                return "text-red";
            }

            return (OPagamento.idStatusPagamento == StatusPagamentoConst.PAGO ? "text-green" : (OPagamento.idStatusPagamento == StatusPagamentoConst.CANCELADO || OPagamento.idStatusPagamento == StatusPagamentoConst.ESTORNADO ? "text-red" : "text-yellow"));
        }
        
        public static decimal valorLiquido(this TituloDespesaPagamentoResumoVW OTitulo) {

            decimal valorLiquido = Decimal.Subtract(UtilNumber.toDecimal(OTitulo.valorOriginal), UtilNumber.toDecimal(OTitulo.valorTotalTarifas()));

            return valorLiquido;
        }

        public static decimal valorTotalTarifas(this TituloDespesaPagamentoResumoVW OTitulo) {

            decimal valorDesconto = Decimal.Add(UtilNumber.toDecimal(OTitulo.valorTarifasBancarias), UtilNumber.toDecimal(OTitulo.valorOutrasTarifas));

            return valorDesconto;
        }

        public static string descricaoCategoriaPessoa(this TituloDespesaPagamentoResumoVW OTitulo) {

            switch (OTitulo.flagCategoriaPessoa) {
                case "AS":
                    return "Associado";
                case "FO":
                    return "Fornecedor";
                case "FU":
                    return "Funcionário";
                case "PA":
                    return "Patrocinador";
                default:
                    return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static TituloDespesaPagamento transferirDadosPadrao(this TituloDespesaPagamento OPagamento, TituloDespesa OTitulo) {

            OPagamento.idTituloDespesa = OTitulo.id;

            OPagamento.idOrganizacao = OTitulo.idOrganizacao;

            OPagamento.valorOriginal = OPagamento.valorOriginal > 0? OPagamento.valorOriginal.toDecimal() : OTitulo.valorTotal.toDecimal();

            OPagamento.dtVencimento = OPagamento.dtVencimento ?? OTitulo.dtVencimento;
            
            OPagamento.dtPrevisaoPagamento = OPagamento.dtPrevisaoPagamento ?? OTitulo.dtPrevisaoPagamento;

            OPagamento.idCentroCusto = OPagamento.idCentroCusto ?? OTitulo.idCentroCusto;
            
            OPagamento.idMacroConta = OPagamento.idMacroConta ?? OTitulo.idMacroConta;

            OPagamento.idCategoria = OPagamento.idCategoria ?? OTitulo.idCategoria;

            OPagamento.idContaBancaria = OPagamento.idContaBancaria ?? OTitulo.idContaBancaria;

            OPagamento.idModoPagamento = OPagamento.idModoPagamento ?? OTitulo.idModoPagamento;

            OPagamento.idContaBancaria = OPagamento.idContaBancaria ?? OTitulo.idContaBancaria;

            OPagamento.idContaBancariaFavorecida = OPagamento.idContaBancariaFavorecida ?? OTitulo.idContaBancariaFavorecida;

            OPagamento.nroNotaFiscal = OPagamento.nroNotaFiscal ?? OTitulo.nroNotaFiscal;

            OPagamento.nroDocumento = !OPagamento.nroDocumento.isEmpty() ? OPagamento.nroDocumento : OTitulo.nroDocumento;

            OPagamento.nroContrato = !OPagamento.nroContrato.isEmpty() ? OPagamento.nroContrato : OTitulo.nroContrato;
            
            OPagamento.codigoBoleto = !OPagamento.codigoBoleto.isEmpty() ? OPagamento.codigoBoleto : OTitulo.codigoBoleto;

            return OPagamento;
        }
        
        /// <summary>
        /// 
        /// </summary>
	    public static TituloDespesaPagamento transferirDadosTitulo(this TituloDespesaPagamento OPagamento, TituloDespesa OTitulo) {

	        OPagamento.idTituloDespesa = OTitulo.id;

	        OPagamento.idOrganizacao = OTitulo.idOrganizacao;

	        OPagamento.valorOriginal = UtilNumber.toDecimal(OTitulo.valorTotal);

	        OPagamento.dtVencimento = OTitulo.dtVencimento;

	        if (OPagamento.dtVencimento.HasValue) {

	            OPagamento.dtCompetencia = OPagamento.dtVencimento;

	            OPagamento.mesCompetencia = (byte?)OPagamento.dtVencimento.Value.Month;

	            OPagamento.anoCompetencia = (short)OPagamento.dtVencimento.Value.Year;
	        }

	        OPagamento.idCentroCusto = OTitulo.idCentroCusto;

	        OPagamento.idMacroConta = OTitulo.idMacroConta;

	        OPagamento.idCategoria = OTitulo.idCategoria;

	        OPagamento.idContaBancaria = OTitulo.idContaBancaria;

	        return OPagamento;
	    }

    }
}