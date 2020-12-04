using System;
using System.IO;
using System.Text;
using System.Web;
using DAL.Financeiro;

namespace WEB.Areas.FinanceiroLancamentos.ViewModels {

    public class ExtratoPorPeriodoExportacao {
        
        //
        public decimal saldoParcialPeriodo { get; set; }
        
        //
        public ExtratoPorPeriodoExportacao() {
            
        }
        
        //
        public void baixarExcel(ExtratoPorPeriodoVM ViewModel) {

            var OResponse = HttpContext.Current.Response;

            StringWriter sw = new StringWriter();

            sw.WriteLine(this.gerarCabecalhoExcel());

            this.saldoParcialPeriodo = ViewModel.saldoParcialPeriodo;

            foreach (var OItem in ViewModel.listaLancamentos) {
                sw.WriteLine(this.gerarLinhaExcel(OItem));
            }

            OResponse.ClearContent();

            var nomeArquivo = String.Concat("extrato-por-periodo-", DateTime.Now.ToShortDateString().Replace("/", "-"), ".csv");
            OResponse.AddHeader("content-disposition", "attachment;filename="+nomeArquivo);
            OResponse.ContentType = "text/csv; charset=ISO-8859-1";
            OResponse.Charset = "ISO-8859-1";
            OResponse.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");

            OResponse.Write(sw.ToString());

            OResponse.End();

        }

        private string gerarCabecalhoExcel() {

            StringBuilder cabecalho = new StringBuilder();

            cabecalho
                .Append("Cód Lançamento;")
                .Append("Cód Título;")
                .Append("Descrição do Lançamento;")
                
                .Append("Data de Competência;")
                .Append("Data de Vencimento;")
                
                .Append("Centro de Custo;")
                .Append("Macro Conta;")
                .Append("Sub Conta Pai;")
                .Append("Sub Conta;")
                
                .Append("Valor Desconto;")
                .Append("Valor Desconto Cupom;")
                .Append("Valor Desconto Antecipação;")
                .Append("Valor;")
                .Append("Valor Recebido/Pago;")
                .Append("Saldo;")
                
                .Append("Qtde. Parcelas;")
                .Append("Nº Parcela;")
                .Append("Parcela;")
                
                .Append("Forma de Pagamento;")
                .Append("Meio de Pagamento;")
                .Append("Gateway de Pagamento;")
                .Append("Token da Transação;")
                .Append("Conta Bancaria;")
                
                .Append("Situação;")
                
                //campos faltantes
                .Append("Tipo Titulo;")
                .Append("Pagador/Recebedor;")
                
                .Append("Data de Pagamento;")
                .Append("Data de Efetivação;")
                .Append("Data de Previsao de Efetivação;")
                .Append("Data de Cadastro;")
                
                .Append("Data de Conciliação;")
                .Append("Usuario de Conciliação;")
                
                
                ;

            return cabecalho.ToString();
        }

        private string gerarLinhaExcel(ReceitaDespesaVW OReceitaDespesaVW) {
            
            StringBuilder linha = new StringBuilder();
            
            if (OReceitaDespesaVW.flagTipoTitulo == "R" && OReceitaDespesaVW.dtPagamento.HasValue) {
                this.saldoParcialPeriodo += OReceitaDespesaVW.valor;
            }

            if (OReceitaDespesaVW.flagTipoTitulo == "D" && OReceitaDespesaVW.dtPagamento.HasValue) {
                this.saldoParcialPeriodo -= OReceitaDespesaVW.valor;
            }

            linha
                .Append(OReceitaDespesaVW.idPagamento).Append(";")
                .Append(OReceitaDespesaVW.idTitulo).Append(";")
                .Append(OReceitaDespesaVW.descricaoTitulo).Append(";")
                
                .Append(OReceitaDespesaVW.dtCompetencia.exibirData()).Append(";")
                .Append(OReceitaDespesaVW.dtVencimento.exibirData()).Append(";")
                
                .Append(OReceitaDespesaVW.descricaoCentroCusto).Append(";")
                .Append(OReceitaDespesaVW.descricaoMacroConta).Append(";")
                .Append(OReceitaDespesaVW.descricaoSubContaPai).Append(";")
                .Append(OReceitaDespesaVW.descricaoSubConta).Append(";")
                
                .Append(OReceitaDespesaVW.valorDesconto).Append(";")
                .Append(OReceitaDespesaVW.valorDescontoCupom).Append(";")
                .Append(OReceitaDespesaVW.valorDescontoAntecipacao).Append(";")
                .Append(OReceitaDespesaVW.valor.ToString("C")).Append(";")
                .Append(OReceitaDespesaVW.valorRealizado?.ToString("C")).Append(";")
                .Append(this.saldoParcialPeriodo.ToString("C")).Append(";")
                
                .Append(OReceitaDespesaVW.qtdeRepeticao).Append(";")
                .Append(OReceitaDespesaVW.nroParcela).Append(";")
                .Append(OReceitaDespesaVW.descricaoParcela).Append(";")
                
                .Append(OReceitaDespesaVW.formaPagamento).Append(";")
                .Append(OReceitaDespesaVW.meioPagamento).Append(";")
                .Append(OReceitaDespesaVW.gatewayPagamento).Append(";")
                .Append(OReceitaDespesaVW.tokenTransacao).Append(";")
                .Append(OReceitaDespesaVW.descricaoContaBancaria).Append(";")
                
                .Append(OReceitaDespesaVW.dtPagamento.HasValue ? "Liquidado" : "Em aberto").Append(";")
                
                //campos faltantes
                .Append(OReceitaDespesaVW.flagTipoTitulo == "R" ? "Receita" : "Despesa").Append(";")
                .Append(OReceitaDespesaVW.nomePessoa).Append(";")
                
                .Append(OReceitaDespesaVW.dtPagamento.exibirData()).Append(";")
                .Append(OReceitaDespesaVW.dtEfetivacao.exibirData()).Append(";")
                .Append(OReceitaDespesaVW.dtPrevisaoEfetivacao.exibirData()).Append(";")
                .Append(OReceitaDespesaVW.dtCadastro.exibirData()).Append(";")
                
                .Append(OReceitaDespesaVW.dtConciliacao.exibirData()).Append(";")
                .Append(OReceitaDespesaVW.nomeUsuarioConciliacao).Append(";")
                
                ;

            return linha.ToString();
        }

    }
}