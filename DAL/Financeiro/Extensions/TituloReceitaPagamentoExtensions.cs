using System;
using DAL.Bancos;
using System.Linq;

namespace DAL.Financeiro {

    public static class TituloReceitaPagamentoExtensions {

        //
        public static string descricaoFormaPagamento(this TituloReceitaPagamento OPagamento) {

            if (OPagamento.idMeioPagamento == MeioPagamentoConst.CARTAO_CREDITO) {
                return "Cartão de Crédito";
            }

            if (OPagamento.idMeioPagamento == MeioPagamentoConst.BOLETO_BANCARIO) {
                return "Boleto Bancário";
            }

            if (OPagamento.idMeioPagamento == MeioPagamentoConst.DEPOSITO_BANCARIO) {
                return "Depósito Bancário";
            }

            if (OPagamento.idMeioPagamento == MeioPagamentoConst.DINHEIRO) {
                return "Dinheiro";
            }

            return "Nenhuma";
        }

        //Definir meio de pagamento
        public static byte? definirMeioPagamento(this TituloReceitaPagamento OPagamento) {

            if (OPagamento.idMeioPagamento.HasValue) {
                return OPagamento.idMeioPagamento.Value;
            }

            if (OPagamento.idFormaPagamento == FormaPagamentoConst.CHEQUE) {
                return MeioPagamentoConst.CHEQUE;
            }

            if (OPagamento.idFormaPagamento == FormaPagamentoConst.BOLETO_BANCARIO) {
                return MeioPagamentoConst.BOLETO_BANCARIO;
            }

            if (OPagamento.idFormaPagamento == FormaPagamentoConst.DEPOSITO_BANCARIO) {
                return MeioPagamentoConst.DEPOSITO_BANCARIO;
            }

            if (OPagamento.idMeioPagamento == MeioPagamentoConst.CARTAO_CREDITO) {
                return MeioPagamentoConst.CARTAO_CREDITO;
            }


            return OPagamento.idMeioPagamento;
        }

        //Definir forma de pagamento
        public static byte? definirFormaPagamento(this TituloReceitaPagamento OPagamento) {

            if (OPagamento.idMeioPagamento == MeioPagamentoConst.BOLETO_BANCARIO) {
                return FormaPagamentoConst.BOLETO_BANCARIO;
            }

            if (OPagamento.idMeioPagamento == MeioPagamentoConst.DINHEIRO) {
                return FormaPagamentoConst.DINHEIRO;
            }

            if (OPagamento.idMeioPagamento == MeioPagamentoConst.DEPOSITO_BANCARIO) {
                return FormaPagamentoConst.DEPOSITO_BANCARIO;
            }

            if (OPagamento.idMeioPagamento == MeioPagamentoConst.CHEQUE) {
                return FormaPagamentoConst.CHEQUE;
            }

            if (OPagamento.idMeioPagamento == MeioPagamentoConst.TRANSFERENCIA_ELETRONICA) {
                return FormaPagamentoConst.TRANSFERENCIA_BANCARIA;
            }

            if (OPagamento.idMeioPagamento == MeioPagamentoConst.CARTAO_CREDITO || OPagamento.idMeioPagamento == MeioPagamentoConst.CARTAO_DEBITO) {
                return Convert.ToByte(OPagamento.idFormaPagamento);
            }

            return 0;
        }

        //Calcular valor total das tarifas
        public static decimal valorTotalTarifas(this TituloReceitaPagamento OPagamento) {

            decimal valorTotal = new decimal(0);

            if (OPagamento == null) {
                return valorTotal;
            }

            if (OPagamento.valorTarifasBancarias > 0) {
                valorTotal = Decimal.Add(valorTotal, OPagamento.valorTarifasBancarias);
            }

            if (OPagamento.valorTarifasTransacao > 0) {
                valorTotal = Decimal.Add(valorTotal, OPagamento.valorTarifasTransacao);
            }

            if (OPagamento.valorOutrasTarifas > 0) {
                valorTotal = Decimal.Add(valorTotal, OPagamento.valorOutrasTarifas);
            }

            return valorTotal;
        }

        //Calcular valor total das tarifas
        public static decimal valorTotalTarifas(this TituloReceitaPagamentoVW OPagamento) {

            decimal valorTotal = new decimal(0);

            if (OPagamento == null) {
                return valorTotal;
            }

            if (OPagamento.valorTarifasBancarias > 0) {
                valorTotal = Decimal.Add(valorTotal, UtilNumber.toDecimal(OPagamento.valorTarifasBancarias));
            }

            if (OPagamento.valorTarifasTransacao > 0) {
                valorTotal = Decimal.Add(valorTotal, UtilNumber.toDecimal(OPagamento.valorTarifasTransacao));
            }

            if (OPagamento.valorOutrasTarifas > 0) {
                valorTotal = Decimal.Add(valorTotal, UtilNumber.toDecimal(OPagamento.valorOutrasTarifas));
            }

            return valorTotal;
        }

        //Calcular valor total das tarifas
        public static decimal valorTotalTarifas(this TituloReceitaPagamentoResumoVW OPagamento) {

            decimal valorTotal = new decimal(0);

            if (OPagamento == null) {
                return valorTotal;
            }

            if (OPagamento.valorTarifasBancarias > 0) {
                valorTotal = Decimal.Add(valorTotal, UtilNumber.toDecimal(OPagamento.valorTarifasBancarias));
            }

            if (OPagamento.valorTarifasTransacao > 0) {
                valorTotal = Decimal.Add(valorTotal, UtilNumber.toDecimal(OPagamento.valorTarifasTransacao));
            }

            if (OPagamento.valorOutrasTarifas > 0) {
                valorTotal = Decimal.Add(valorTotal, UtilNumber.toDecimal(OPagamento.valorOutrasTarifas));
            }

            return valorTotal;
        }

        //Calcular valor total das tarifas
        public static decimal valorTotalTarifas(this ReceitaDespesaVW OPagamento) {

            decimal valorTotal = new decimal(0);

            if (OPagamento == null) {
                return valorTotal;
            }

            if (OPagamento.valorTarifasBancarias > 0) {
                valorTotal = Decimal.Add(valorTotal, UtilNumber.toDecimal(OPagamento.valorTarifasBancarias));
            }

            if (OPagamento.valorTarifasTransacao > 0) {
                valorTotal = Decimal.Add(valorTotal, UtilNumber.toDecimal(OPagamento.valorTarifasTransacao));
            }

            if (OPagamento.valorOutrasTarifas > 0) {
                valorTotal = Decimal.Add(valorTotal, UtilNumber.toDecimal(OPagamento.valorOutrasTarifas));
            }

            return valorTotal;
        }
        
        //Calcular valor total das tarifas
        public static decimal valorTotalDescontos(this TituloReceitaPagamento OPagamento) {

            decimal valorTotalDescontos = new decimal(0);

            if (OPagamento == null) {
                return valorTotalDescontos;
            }

            if (OPagamento.valorDesconto > 0) {

                valorTotalDescontos = decimal.Add(valorTotalDescontos, OPagamento.valorDesconto.toDecimal());

            }

            if (OPagamento.valorDescontoCupom > 0) {

                valorTotalDescontos = decimal.Add(valorTotalDescontos, OPagamento.valorDescontoCupom.toDecimal());

            }

            if (OPagamento.valorDescontoAntecipacao > 0) {

                valorTotalDescontos = decimal.Add(valorTotalDescontos, OPagamento.valorDescontoAntecipacao.toDecimal());

            }


            return valorTotalDescontos;
        }
        
        //Calcular valor total das tarifas
        public static decimal valorTotalDescontos(this TituloReceitaPagamentoResumoVW OPagamento) {

            decimal valorTotalDescontos = new decimal(0);

            if (OPagamento == null) {
                return valorTotalDescontos;
            }

            if (OPagamento.valorDesconto > 0) {

                valorTotalDescontos = decimal.Add(valorTotalDescontos, OPagamento.valorDesconto.toDecimal());

            }

            if (OPagamento.valorDescontoCupom > 0) {

                valorTotalDescontos = decimal.Add(valorTotalDescontos, OPagamento.valorDescontoCupom.toDecimal());

            }

            if (OPagamento.valorDescontoAntecipacao > 0) {

                valorTotalDescontos = decimal.Add(valorTotalDescontos, OPagamento.valorDescontoAntecipacao.toDecimal());

            }


            return valorTotalDescontos;
        }        

        //Calcular valor total das tarifas
        public static decimal valorTotalDescontos(this ReceitaDespesaVW OPagamento) {

            decimal valorTotalDescontos = new decimal(0);

            if (OPagamento == null) {
                return valorTotalDescontos;
            }

            if (OPagamento.valorDesconto > 0) {

                valorTotalDescontos = decimal.Add(valorTotalDescontos, OPagamento.valorDesconto.toDecimal());

            }

            if (OPagamento.valorDescontoCupom > 0) {

                valorTotalDescontos = decimal.Add(valorTotalDescontos, OPagamento.valorDescontoCupom.toDecimal());

            }

            if (OPagamento.valorDescontoAntecipacao > 0) {

                valorTotalDescontos = decimal.Add(valorTotalDescontos, OPagamento.valorDescontoAntecipacao.toDecimal());

            }


            return valorTotalDescontos;
        }        

        //Calcular valor líquido a receber
        public static decimal valorLiquido(this TituloReceitaPagamento OPagamento) {

            decimal valorCreditos = new decimal(0);
            
            decimal valorDebitos = new decimal(0);

            if (OPagamento == null) {
                return new decimal(0);
            }

            valorCreditos = OPagamento.valorOriginal;

            valorDebitos = decimal.Add(OPagamento.valorTotalTarifas(), OPagamento.valorTotalDescontos());
            
            decimal saldoFinal = decimal.Subtract(valorCreditos, valorDebitos);

            if (OPagamento.dtPagamento != null) {

                saldoFinal = decimal.Subtract(OPagamento.valorRecebido.toDecimal(), OPagamento.valorTotalTarifas());

            } else{
                
                saldoFinal = decimal.Add(saldoFinal, OPagamento.valorJuros.toDecimal());
            }

            return saldoFinal;
        }
     
        //Calcular valor líquido a receber
        public static decimal valorLiquido(this TituloReceitaPagamentoResumoVW OPagamento) {

            decimal valorCreditos = new decimal(0);
            
            decimal valorDebitos = new decimal(0);

            if (OPagamento == null) {
                return new decimal(0);
            }

            valorCreditos = OPagamento.valorOriginal.toDecimal();

            valorDebitos = decimal.Add(OPagamento.valorTotalTarifas(), OPagamento.valorTotalDescontos());
            
            decimal saldoFinal = decimal.Subtract(valorCreditos, valorDebitos);

            if (OPagamento.dtPagamento != null) {

                saldoFinal = decimal.Subtract(OPagamento.valorRecebido.toDecimal(), OPagamento.valorTotalTarifas());

            } else{
                
                saldoFinal = decimal.Add(saldoFinal, OPagamento.valorJuros.toDecimal());
            }

            return saldoFinal;
        }

        public static decimal valorLiquido(this ReceitaDespesaVW OPagamento) {

            decimal valorCreditos = new decimal(0);
            
            decimal valorDebitos = new decimal(0);

            if (OPagamento == null) {
                return new decimal(0);
            }

            valorCreditos = OPagamento.valor;

            valorDebitos = decimal.Add(OPagamento.valorTotalTarifas(), OPagamento.valorTotalDescontos());
            
            decimal saldoFinal = decimal.Subtract(valorCreditos, valorDebitos);

            if (OPagamento.dtPagamento != null) {

                saldoFinal = decimal.Subtract(OPagamento.valorRealizado.toDecimal(), OPagamento.valorTotalTarifas());

            } else{
                
                saldoFinal = decimal.Add(saldoFinal, OPagamento.valorJuros.toDecimal());
            }

            return saldoFinal;
        }
               
        /// <summary>
        /// Considera valor original e abate os descontos
        /// </summary>
        public static decimal valorTotalComDesconto(this TituloReceitaPagamento OPagamento) {

            decimal valorTotal = new decimal(0);
            
            decimal valorTotalDescontos = new decimal(0);

            if (OPagamento == null) {
                return valorTotal;
            }

            valorTotal = OPagamento.valorOriginal;

            valorTotalDescontos = OPagamento.valorTotalDescontos();

            if (valorTotalDescontos > 0) {

                valorTotal = decimal.Subtract(valorTotal, valorTotalDescontos);

            }

            valorTotal = valorTotal < 0 ? 0 : valorTotal;

            valorTotal = Math.Round(valorTotal, 2);
            
            return valorTotal;
        }

        /// <summary>
        /// Considera valor original + juros e abate os descontos
        /// </summary>
        public static decimal valorComJurosEDescontos(this TituloReceitaPagamento OPagamento) {

            decimal valorTotal = new decimal(0);
            
            decimal valorTotalDescontos = new decimal(0);

            if (OPagamento == null) {
                return valorTotal;
            }

            valorTotal = decimal.Add(OPagamento.valorOriginal, OPagamento.valorJuros.toDecimal());

            valorTotalDescontos = OPagamento.valorTotalDescontos();

            if (valorTotalDescontos > 0) {

                valorTotal = decimal.Subtract(valorTotal, valorTotalDescontos);

            }

            valorTotal = valorTotal < 0 ? 0 : valorTotal;

            valorTotal = Math.Round(valorTotal, 2);
            
            return valorTotal;
        }    
        
        /// <summary>
        /// Considera valor original + juros e abate os descontos
        /// </summary>
        public static decimal valorComJurosEDescontos(this TituloReceitaPagamentoResumoVW OPagamento) {

            decimal valorTotal = new decimal(0);
            
            decimal valorTotalDescontos = new decimal(0);

            if (OPagamento == null) {
                return valorTotal;
            }

            valorTotal = decimal.Add(OPagamento.valorOriginal.toDecimal(), OPagamento.valorJuros.toDecimal());

            valorTotalDescontos = OPagamento.valorTotalDescontos();

            if (valorTotalDescontos > 0) {

                valorTotal = decimal.Subtract(valorTotal, valorTotalDescontos);

            }

            valorTotal = valorTotal < 0 ? 0 : valorTotal;

            valorTotal = Math.Round(valorTotal, 2);
            
            return valorTotal;
        }    
        
        /// <summary>
        /// Considera valor original + juros e abate os descontos
        /// </summary>
        public static decimal valorComJurosSemDescontos(this TituloReceitaPagamento OPagamento) {

            decimal valorTotal = new decimal(0);
            
            if (OPagamento == null) {
                return valorTotal;
            }

            valorTotal = decimal.Add(OPagamento.valorOriginal, OPagamento.valorJuros.toDecimal());

            valorTotal = valorTotal < 0 ? 0 : valorTotal;

            valorTotal = Math.Round(valorTotal, 2);
            
            return valorTotal;
        }    

        
        //
        public static string descricaoPagamento(this TituloReceitaPagamento OPagamento) {

            if (OPagamento?.TituloReceita == null) {
                return "-";
            }

            string descricaoPagamento = OPagamento.TituloReceita.descricao;

            if (!string.IsNullOrEmpty(OPagamento.descricaoParcela)) {
                descricaoPagamento = String.Concat(descricaoPagamento, " (", OPagamento.descricaoParcela, ")");
            }

            return descricaoPagamento;
        }

        //Icone FA situacao financeira Associado
        public static string exibirIconeStatus(this TituloReceitaPagamento OPagamento) {

            if (OPagamento.dtPagamento == null && OPagamento.dtVencimento < DateTime.Today) {
                return "fa-times-circle";
            }

            string descricaoAtivo = (OPagamento.idStatusPagamento == StatusPagamentoConst.PAGO ? "fa-check" : (OPagamento.idStatusPagamento == StatusPagamentoConst.CANCELADO || OPagamento.idStatusPagamento == StatusPagamentoConst.ESTORNADO ? "fa-times-circle" : "fa-exclamation"));

            return descricaoAtivo;
        }

        //Classes CSS situacao financeira Associado
        public static string exibirClasseStatus(this TituloReceitaPagamento OPagamento) {

            if (OPagamento.dtPagamento == null && OPagamento.dtVencimento < DateTime.Today) {
                return "text-red";
            }

            string descricaoAtivo = (OPagamento.idStatusPagamento == StatusPagamentoConst.PAGO ? "text-green" : (OPagamento.idStatusPagamento == StatusPagamentoConst.CANCELADO || OPagamento.idStatusPagamento == StatusPagamentoConst.ESTORNADO ? "text-red" : "text-yellow"));

            return descricaoAtivo;
        }

        //Borda
        public static string exibirBordaStatus(this TituloReceitaPagamento OPagamento) {

            if (OPagamento.dtPagamento == null && OPagamento.dtVencimento < DateTime.Today) {
                return "border-red";
            }

            string descricaoAtivo = (OPagamento.idStatusPagamento == StatusPagamentoConst.PAGO ? "border-green" : (OPagamento.idStatusPagamento == StatusPagamentoConst.CANCELADO || OPagamento.idStatusPagamento == StatusPagamentoConst.ESTORNADO ? "border-red" : "border-yellow"));

            return descricaoAtivo;
        }

        //Borda
        public static string exibirBordaStatus(this TituloReceitaPagamentoResumoVW OPagamento) {

            if (OPagamento.dtPagamento == null && OPagamento.dtVencimentoRecebimento < DateTime.Today) {
                return "border-red";
            }

            string descricaoAtivo = (OPagamento.idStatusPagamento == StatusPagamentoConst.PAGO ? "border-green" : (OPagamento.idStatusPagamento == StatusPagamentoConst.CANCELADO || OPagamento.idStatusPagamento == StatusPagamentoConst.ESTORNADO ? "border-red" : "border-yellow"));

            return descricaoAtivo;
        }

        //Icone FA situacao financeira Associado
        public static string exibirIconeStatus(this TituloReceitaPagamentoResumoVW OPagamento) {

            if (OPagamento.dtPagamento == null && OPagamento.dtVencimentoRecebimento < DateTime.Today) {
                return "fa-ban";
            }

            string descricaoAtivo = (OPagamento.idStatusPagamento == StatusPagamentoConst.PAGO ? "fa-check" : (OPagamento.idStatusPagamento == StatusPagamentoConst.CANCELADO || OPagamento.idStatusPagamento == StatusPagamentoConst.ESTORNADO ? "fa-ban" : "fa-exclamation"));

            return descricaoAtivo;
        }

        //Classes CSS situacao financeira Associado
        public static string exibirClasseStatus(this TituloReceitaPagamentoResumoVW OPagamento) {

            if (OPagamento.dtPagamento == null && OPagamento.dtVencimentoRecebimento < DateTime.Today) {
                return "text-red";
            }

            string descricaoAtivo = (OPagamento.idStatusPagamento == StatusPagamentoConst.PAGO ? "text-green" : (OPagamento.idStatusPagamento == StatusPagamentoConst.CANCELADO || OPagamento.idStatusPagamento == StatusPagamentoConst.ESTORNADO ? "text-red" : "text-yellow"));

            return descricaoAtivo;
        }


        /// <summary>
        /// Transferir as configuracoes do checkout para o registro de pagamento
        /// </summary>
        public static TituloReceitaPagamento transferirDadosTitulo(this TituloReceitaPagamento OPagamento, TituloReceita OTitulo) {

            OPagamento.idTituloReceita = OTitulo.id;

            OPagamento.idOrganizacao = OTitulo.idOrganizacao;

            OPagamento.valorOriginal = UtilNumber.toDecimal(OTitulo.valorTotal);

            OPagamento.dtVencimentoOriginal = OTitulo.dtVencimento;

            OPagamento.dtVencimento = OPagamento.dtVencimentoOriginal;

            if (OPagamento.dtVencimentoOriginal.HasValue) {

                OPagamento.dtCompetencia = OPagamento.dtVencimentoOriginal;

                OPagamento.mesCompetencia = (byte?)OPagamento.dtVencimentoOriginal.Value.Month;

                OPagamento.anoCompetencia = (short)OPagamento.dtVencimentoOriginal.Value.Year;
            }

            OPagamento.idCentroCusto = OTitulo.idCentroCusto;

            OPagamento.idMacroConta = OTitulo.idMacroConta;

            OPagamento.idCategoria = OTitulo.idCategoria;

            OPagamento.idContaBancaria = OTitulo.idContaBancaria;

            OPagamento.nomeRecibo = OTitulo.nomeRecibo;
            
            OPagamento.documentoRecibo = OTitulo.documentoRecibo;

            OPagamento.cepRecibo = OTitulo.cepRecibo;

            OPagamento.logradouroRecibo = OTitulo.logradouroRecibo;

            OPagamento.numeroRecibo = OTitulo.numeroRecibo;

            OPagamento.complementoRecibo = OTitulo.complementoRecibo;

            OPagamento.bairroRecibo = OTitulo.bairroRecibo;

            OPagamento.idCidadeRecibo = OTitulo.idCidadeRecibo;

            OPagamento.nomeCidadeRecibo = OTitulo.nomeCidadeRecibo;

            OPagamento.telPrincipal = OTitulo.nroTelPrincipal;

            OPagamento.telSecundario = OTitulo.nroTelSecundario;

            OPagamento.email = OTitulo.emailPrincipal;

            return OPagamento;
        }



        /// <summary>
        /// Transferir as configuracoes do checkout para o registro de pagamento
        /// </summary>
        public static TituloReceitaPagamento limparDados(this TituloReceitaPagamento OPagamento) {

            OPagamento.codigoAutorizacao = OPagamento.codigoAutorizacao.stringOrEmpty();
            
            OPagamento.tid = OPagamento.tid.stringOrEmpty();
            
            OPagamento.nroBanco = OPagamento.nroBanco.stringOrEmpty();
            
            OPagamento.nroDocumento = OPagamento.nroDocumento.stringOrEmpty();
            
            OPagamento.nroAgencia = OPagamento.nroAgencia.stringOrEmpty();
            
            OPagamento.nroDigitoAgencia = OPagamento.nroDigitoAgencia.stringOrEmpty();
            
            OPagamento.nroConta = OPagamento.nroConta.stringOrEmpty();
            
            OPagamento.nroDigitoConta = OPagamento.nroDigitoConta.stringOrEmpty();

            OPagamento.valorRecebido = OPagamento.valorRecebido.toDecimal();
            
            OPagamento.valorJuros = OPagamento.valorJuros.toDecimal();
            
            OPagamento.valorTarifasBancarias = OPagamento.valorTarifasBancarias.toDecimal();
            
            OPagamento.valorTarifasTransacao = OPagamento.valorTarifasTransacao.toDecimal();

            OPagamento.idUsuarioBaixa = OPagamento.idUsuarioBaixa > 0 ? OPagamento.idUsuarioBaixa : null;
            
            OPagamento.idUsuarioAlteracao = OPagamento.idUsuarioAlteracao > 0 ? OPagamento.idUsuarioAlteracao : null;
            
            return OPagamento;
        }        
    }
}