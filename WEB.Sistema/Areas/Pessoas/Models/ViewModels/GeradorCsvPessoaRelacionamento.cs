using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using DAL.Pessoas;
using WEB.Helpers;

namespace WEB.Areas.Pessoas.ViewModels {

	public class GeradorCsvPessoaRelacionamento {
        
        //
        public void baixarExcel(List<PessoaRelacionamento> listaOcorrencias) {

            var OResponse = HttpContext.Current.Response;

            StringWriter sw = new StringWriter();

            sw.WriteLine(this.gerarCabecalhoExcel());

            foreach (var OItem in listaOcorrencias) {
                sw.WriteLine(this.gerarLinhaExcel(OItem));
            }

            OResponse.ClearContent();

            var nomeArquivo = String.Concat("historico-lancados-associado-", DateTime.Now.ToShortDateString().Replace("/", "-"), ".csv");
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
                .Append("Ocorrência;")
                .Append("Data da Ocorrência;")
                .Append("Observações;")
                .Append("Data Cadastro;")
                .Append("Usuário Cadastro;");
            
            return cabecalho.ToString();
        }

        private string gerarLinhaExcel(PessoaRelacionamento OOcorrencia) {
            
            StringBuilder linha = new StringBuilder();

            linha.Append(OOcorrencia.id).Append(";")
                .Append(OOcorrencia.OcorrenciaRelacionamento?.descricao ?? "-").Append(";")
                .Append(OOcorrencia.dtOcorrencia.exibirData()).Append(";")
                .Append(OOcorrencia.observacao).Append(";")
                .Append(OOcorrencia.dtCadastro.exibirData(true)).Append(";")
                .Append(OOcorrencia.UsuarioCadastro?.nome ?? "Sistema").Append(";");

            return linha.ToString();
        }

	}
	
}