using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using DAL.Contratos;

namespace WEB.Areas.Contratos.ViewModels {

    public class ContratoExportacao {
        
        //Atributos

        // Propriedades

        //
        public ContratoExportacao() {
            
        }
        
        //
        public void baixarExcel(List<Contrato> listaContratos) {

            var OResponse = HttpContext.Current.Response;

            StringWriter sw = new StringWriter();

            sw.WriteLine(this.gerarCabecalhoExcel());

            foreach (var OItem in listaContratos) {
                sw.WriteLine(this.gerarLinhaExcel(OItem));
            }

            OResponse.ClearContent();

            var nomeArquivo = String.Concat("relatorios-contratos-", DateTime.Now.ToShortDateString().Replace("/", "-"), ".csv");
            OResponse.AddHeader("content-disposition", "attachment;filename="+nomeArquivo);
            OResponse.ContentType = "text/csv; charset=ISO-8859-1";
            OResponse.Charset = "ISO-8859-1";
            OResponse.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");

            OResponse.Write(sw.ToString());

            OResponse.End();

        }

        private string gerarCabecalhoExcel() {

            StringBuilder cabecalho = new StringBuilder();

            cabecalho.Append("Código Sistema;")
                .Append("Tipo de Contrado;")
                .Append("Contratado;")
                .Append("Número Contrato;")
                .Append("Título;")
                .Append("Objeto do Contrato;")

                .Append("Início Vigência;")
                .Append("Fim Vigência;")

                .Append("Operação Financeira;")
                .Append("Centro de Custo;")
                .Append("Valor Total;");


            return cabecalho.ToString();
        }

        private string gerarLinhaExcel(Contrato OContrato) {
            
            StringBuilder linha = new StringBuilder();

            linha.Append(OContrato.id).Append(";")
                .Append(OContrato.TipoContrato?.descricao).Append(";")
                .Append(OContrato.Fornecedor?.Pessoa.nome).Append(";")
                .AppendFormat("=\"{0}\"",OContrato.nroContrato).Append(";")
                .Append(OContrato.titulo).Append(";")
                .Append(OContrato.objetoContrato).Append(";")

                .Append(OContrato.dtInicioVigencia.exibirData()).Append(";")
                .Append(OContrato.dtFimVigencia.exibirData()).Append(";")

                .Append(OContrato.flagOperacaoFinanceira == "D" ? "Débito" : (OContrato.flagOperacaoFinanceira == "C" ? "Crédito" : "")).Append(";")
                .Append(OContrato.flagOperacaoFinanceira == "D" ? OContrato.CentroCusto.descricao : "").Append(";")
                .Append(OContrato.valorTotal).Append(";");

            return linha.ToString();
        }

    }
}