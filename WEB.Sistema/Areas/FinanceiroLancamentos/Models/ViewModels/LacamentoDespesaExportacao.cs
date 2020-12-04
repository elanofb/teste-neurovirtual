using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using DAL.Documentos;
using DAL.Financeiro;

namespace WEB.Areas.FinanceiroLancamentos.ViewModels {

    public class LancamentoDespesaExportacao {
        
        //Atributos

        // Propriedades

        //
        public LancamentoDespesaExportacao() {
            
        }
        
        //
        public void baixarExcel(List<TituloDespesaPagamentoResumoVW> listaDespesas) {

            var OResponse = HttpContext.Current.Response;

            StringWriter sw = new StringWriter();

            sw.WriteLine(this.gerarCabecalhoExcel());

            foreach (var OItem in listaDespesas) {
                sw.WriteLine(this.gerarLinhaExcel(OItem));
            }

            OResponse.ClearContent();

            var nomeArquivo = String.Concat("relatorios-lancamentos-a-pagar-", DateTime.Now.ToShortDateString().Replace("/", "-"), ".csv");
            OResponse.AddHeader("content-disposition", "attachment;filename="+nomeArquivo);
            OResponse.ContentType = "text/csv; charset=ISO-8859-1";
            OResponse.Charset = "ISO-8859-1";
            OResponse.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");

            OResponse.Write(sw.ToString());

            OResponse.End();

        }

        private string gerarCabecalhoExcel() {

            StringBuilder cabecalho = new StringBuilder();

            cabecalho.Append("Nº Titulo Despesa;")
                .Append("Nº Pagamento;")
                .Append("Descrição;")
                
                .Append("Centro de Custo;")
                .Append("Macro Conta;")
                .Append("Sub Conta Pai;")
                .Append("Sub Conta;")
                
                .Append("Pago a;")
                .Append("Data de Vencimento;")
                .Append("Data de Competência;")
                .Append("Data de Pagamento;")
                .Append("Valor Pago;")
                .Append("Valor Original;")
                .Append("Situação;")

                .Append("Despesa Fixa?;")
                .Append("Conta Bancária;")
                .Append("Nº Documento;")
                .Append("Nota Fiscal;")
                .Append("Obs;")
                .Append("Nº Contábil;")
                
                .Append("Forma de Pagamento;")
                .Append("Meio de Pagamento;")
                
                .Append("Qtde. de Pagamentos;")
                .Append("Período de Pagamento;");

            return cabecalho.ToString();
        }

        private string gerarLinhaExcel(TituloDespesaPagamentoResumoVW oTituloDespesaPagamentoResumoVw) {
            
            StringBuilder linha = new StringBuilder();

            linha.Append(oTituloDespesaPagamentoResumoVw.idTituloDespesa).Append(";")
                .Append(oTituloDespesaPagamentoResumoVw.idTituloPagamento).Append(";")
                .Append(oTituloDespesaPagamentoResumoVw.descricao).Append(";")
                
                .Append(oTituloDespesaPagamentoResumoVw.descricaoCentroCusto).Append(";")
                .Append(oTituloDespesaPagamentoResumoVw.descricaoMacroConta).Append(";")
                .Append(oTituloDespesaPagamentoResumoVw.descricaoCategoriaPai).Append(";")
                .Append(oTituloDespesaPagamentoResumoVw.descricaoCategoria).Append(";")
                
                .Append(oTituloDespesaPagamentoResumoVw.nomePessoa).Append(";")
                .Append(oTituloDespesaPagamentoResumoVw.dtVencimentoDespesa).Append(";")
                .Append(oTituloDespesaPagamentoResumoVw.dtCompetencia).Append(";")
                .Append(oTituloDespesaPagamentoResumoVw.dtPagamento).Append(";")
                .Append(oTituloDespesaPagamentoResumoVw.valorPago).Append(";")
                .Append(oTituloDespesaPagamentoResumoVw.valorOriginal).Append(";")
                .Append(oTituloDespesaPagamentoResumoVw.dtPagamento == null && oTituloDespesaPagamentoResumoVw.dtVencimentoDespesa < DateTime.Today ? "EM ATRASO" : (oTituloDespesaPagamentoResumoVw.idStatusPagamento > 0 ? oTituloDespesaPagamentoResumoVw.descricaoStatusPagamento : "EM ABERTO")).Append(";")

                .Append(oTituloDespesaPagamentoResumoVw.flagFixa == "S" ? "Sim" : "Não").Append(";")
                .Append(oTituloDespesaPagamentoResumoVw.descricaoContaBancaria).Append(";")
                .Append(oTituloDespesaPagamentoResumoVw.idTipoDocumentoPessoa == TipoDocumentoConst.CNPJ || oTituloDespesaPagamentoResumoVw.idTipoDocumentoPessoa == TipoDocumentoConst.CPF ? UtilString.formatCPFCNPJ(oTituloDespesaPagamentoResumoVw.nroDocumentoPessoa) : "").Append(";")
                .Append(oTituloDespesaPagamentoResumoVw.nroNotaFiscal).Append(";")
                .Append(oTituloDespesaPagamentoResumoVw.observacao).Append(";")
                .Append(oTituloDespesaPagamentoResumoVw.nroContabil).Append(";")
                
                .Append(oTituloDespesaPagamentoResumoVw.formaPagamento).Append(";")
                .Append(oTituloDespesaPagamentoResumoVw.meioPagamento).Append(";")

                .Append(oTituloDespesaPagamentoResumoVw.qtdeRepeticao).Append(";")
                .Append(oTituloDespesaPagamentoResumoVw.descricaoPeriodoRepeticao).Append(";");

            return linha.ToString();
        }

    }
}