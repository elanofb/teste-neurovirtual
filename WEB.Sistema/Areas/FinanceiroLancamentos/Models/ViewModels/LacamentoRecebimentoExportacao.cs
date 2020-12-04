using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using DAL.Financeiro;
using DAL.Documentos;

namespace WEB.Areas.FinanceiroLancamentos.ViewModels {

    public class LancamentoRecebimentoExportacao {
        
        //Atributos

        // Propriedades

        //
        public LancamentoRecebimentoExportacao() {
            
        }
        
        //
        public void baixarExcel(List<TituloReceitaPagamentoResumoVW> listaRecebimentos) {

            var OResponse = HttpContext.Current.Response;

            StringWriter sw = new StringWriter();

            sw.WriteLine(this.gerarCabecalhoExcel());

            foreach (var OItem in listaRecebimentos) {
                sw.WriteLine(this.gerarLinhaExcel(OItem));
            }

            OResponse.ClearContent();

            var nomeArquivo = String.Concat("relatorios-lancamentos-a-receber-", DateTime.Now.ToShortDateString().Replace("/", "-"), ".csv");
            OResponse.AddHeader("content-disposition", "attachment;filename="+nomeArquivo);
            OResponse.ContentType = "text/csv; charset=ISO-8859-1";
            OResponse.Charset = "ISO-8859-1";
            OResponse.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");

            OResponse.Write(sw.ToString());

            OResponse.End();

        }

        private string gerarCabecalhoExcel() {

            StringBuilder cabecalho = new StringBuilder();

            cabecalho.Append("Nº Titulo Receita;")
                .Append("Nº Recebimento;")
                .Append("Descrição;")
                
                .Append("Centro de Custo;")
                .Append("Macro Conta;")
                .Append("Sub Conta Pai;")
                .Append("Sub Conta;")
                
                .Append("Recebido de;")
                .Append("Data de Vencimento;")
                .Append("Data de Competência;")
                .Append("Data de Pagamento;")
                .Append("Data de Previsão Crédito;")
                .Append("Data de Crédito;")
                .Append("Valor Original;")
                .Append("Juros;")
                .Append("Descontos Cupom;")
                .Append("Descontos Antecipação;")
                .Append("Valor Recebido;")
                .Append("Tarifas Transação;")
                .Append("Tarifas Bancárias;")
                .Append("Outros Tarifas;")
                .Append("Valor Recebido;")
                .Append("Situação;")
                
                .Append("Gateway;")
                .Append("Meio Pagamento;")
                .Append("Forma Pagamento;")
                .Append("Data Baixa;")
                .Append("Tipo Baixa;")
                .Append("Usuário Baixa;")
                .Append("Token Transação;")

                .Append("Data Exclusão;")
                .Append("Motivo Exclusão;")

                .Append("Conta Bancária;")
                .Append("Nº Documento;")
                .Append("Nota Fiscal;")
                .Append("Obs;")
                .Append("Nº Contábil;")

                .Append("Qtde. de Pagamentos;")
                .Append("Período de Pagamento;");

            return cabecalho.ToString();
        }

        private string gerarLinhaExcel(TituloReceitaPagamentoResumoVW OTituloReceitaVW) {
            
            StringBuilder linha = new StringBuilder();

            linha.Append(OTituloReceitaVW.idTituloReceita).Append(";")
                .Append(OTituloReceitaVW.idTituloPagamento).Append(";")
                .Append(OTituloReceitaVW.descricao).Append(";")
                
                .Append(OTituloReceitaVW.descricaoCentroCusto).Append(";")
                .Append(OTituloReceitaVW.descricaoMacroConta).Append(";")
                .Append(OTituloReceitaVW.descricaoCategoriaPai).Append(";")
                .Append(OTituloReceitaVW.descricaoCategoria).Append(";")
                
                .Append(OTituloReceitaVW.nomePessoa).Append(";")
                .Append(OTituloReceitaVW.dtVencimentoRecebimento).Append(";")
                .Append(OTituloReceitaVW.dtCompetenciaTitulo).Append(";")
                .Append(OTituloReceitaVW.dtPagamento).Append(";")
                .Append(OTituloReceitaVW.dtPrevisaoCredito.exibirData()).Append(";")
                .Append(OTituloReceitaVW.dtCredito.exibirData()).Append(";")
                .Append(OTituloReceitaVW.valorOriginal).Append(";")                
                .Append(OTituloReceitaVW.valorJuros.toDecimal()).Append(";")
                .Append(OTituloReceitaVW.valorDescontoCupom.toDecimal()).Append(";")
                .Append(OTituloReceitaVW.valorDescontoAntecipacao.toDecimal()).Append(";")
                .Append(OTituloReceitaVW.valorRecebido).Append(";")                
                .Append(OTituloReceitaVW.valorTarifasTransacao.toDecimal()).Append(";")
                .Append(OTituloReceitaVW.valorTarifasBancarias.toDecimal()).Append(";")
                .Append(OTituloReceitaVW.valorOutrasTarifas.toDecimal()).Append(";")
                .Append(OTituloReceitaVW.valorLiquido()).Append(";")
                .Append(OTituloReceitaVW.dtPagamento == null && OTituloReceitaVW.dtVencimentoRecebimento < DateTime.Today ? "EM ATRASO" : (OTituloReceitaVW.idStatusPagamento > 0 ? OTituloReceitaVW.descricaoStatusPagamento : "EM ABERTO")).Append(";")

                .Append(OTituloReceitaVW.descricaoGatewayPagamento).Append(";")
                .Append(OTituloReceitaVW.descricaoMeioPagamento).Append(";")
                .Append(OTituloReceitaVW.descricaoFormaPagamento).Append(";")
                .Append(OTituloReceitaVW.dtBaixa.exibirData()).Append(";")
                .Append(OTituloReceitaVW.flagBaixaAutomatica == true ? "Automática" : "Manual").Append(";")
                .Append(OTituloReceitaVW.nomeUsuarioBaixa).Append(";")
                .Append(OTituloReceitaVW.tokenTransacao).Append(";")

                .Append(OTituloReceitaVW.dtExclusao.exibirData()).Append(";")
                .Append(OTituloReceitaVW.motivoExclusao).Append(";")

                .Append(OTituloReceitaVW.descricaoContaBancaria).Append(";")
                .Append(OTituloReceitaVW.idTipoDocumentoPessoa == TipoDocumentoConst.CNPJ || OTituloReceitaVW.idTipoDocumentoPessoa == TipoDocumentoConst.CPF ? UtilString.formatCPFCNPJ(OTituloReceitaVW.nroDocumentoPessoa) : "").Append(";")
                .Append(OTituloReceitaVW.nroNotaFiscal).Append(";")
                .Append(OTituloReceitaVW.observacao).Append(";")
                .Append(OTituloReceitaVW.nroContabil).Append(";")
                
                .Append(OTituloReceitaVW.qtdeRepeticao).Append(";")
                .Append(OTituloReceitaVW.descricaoPeriodoRepeticao).Append(";");

            return linha.ToString();
        }

    }
}