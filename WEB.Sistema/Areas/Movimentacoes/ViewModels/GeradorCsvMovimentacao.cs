using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using DAL.Transacoes;

namespace WEB.Areas.Movimentacoes.ViewModels{

    public class GeradorCsvMovimentacao{
        //Atributos

        // Propriedades

        //
        public GeradorCsvMovimentacao() {
            
        }
        
        //
        public void baixarExcel(List<MovimentoResumoVW> listaVeiculos) {
            
            var OResponse = HttpContext.Current.Response;
            
            StringWriter sw = new StringWriter();
            
            sw.WriteLine(this.gerarCabecalhoExcel());
                
            foreach (var OItem in listaVeiculos) {
                sw.WriteLine(this.gerarLinhaExcel(OItem));
            }
        
            OResponse.ClearContent();
            
            var nomeArquivo = String.Concat("relatorios-movimentacoes-", DateTime.Now.ToShortDateString().Replace("/", "-"), ".csv");
            OResponse.AddHeader("content-disposition", "attachment;filename="+nomeArquivo);
            OResponse.ContentType = "text/csv; charset=ISO-8859-1";
            OResponse.Charset = "ISO-8859-1";
            OResponse.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");

            OResponse.Write(sw.ToString());

            OResponse.End();

        }

        private string gerarCabecalhoExcel() {

            StringBuilder cabecalho = new StringBuilder();

            cabecalho.Append("Código no Sistema;")
                .Append("Data do Movimento;")
                .Append("Conta de Origem;")
                .Append("Membro de Origem;")
                .Append("Conta de Destino;")
                .Append("Membro de Destino;")
                .Append("Descrição;")
                .Append("Crédito / Débito;")
                .Append("Valor;")
                .Append("Data de Atualização do Saldo;")
                .Append("Observações;");
                
            return cabecalho.ToString();
        }

        private string gerarLinhaExcel(MovimentoResumoVW OMovimento) {
            
            StringBuilder linha = new StringBuilder();
            
            linha.Append(OMovimento.idMovimento).Append(";")
                .Append(OMovimento.dtCadastro.exibirData(true)).Append(";")
                .Append(OMovimento.nroMembroOrigem).Append(";")
                .Append(OMovimento.nomeMembroOrigem).Append(";")
                .Append(OMovimento.nroMembroDestino).Append(";")
                .Append(OMovimento.nomeMembroDestino).Append(";")
                .Append(OMovimento.descricaoTipoTransacao).Append(";")
                .Append(OMovimento.flagCredito == true ? "Crédito" : "Débito").Append(";")
                .Append(OMovimento.valorOperacao.toDecimal().ToString("F4")).Append(";")
                .Append(OMovimento.dtIntegracaoSaldo.exibirData(true)).Append(";")
                .Append(OMovimento.observacao).Append(";");

            return linha.ToString();
        }
    }

}