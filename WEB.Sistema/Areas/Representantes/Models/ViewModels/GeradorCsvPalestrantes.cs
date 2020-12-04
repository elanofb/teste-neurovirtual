using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using DAL.Representantes;

namespace WEB.Areas.Representantes.ViewModels {

	public class GeradorCsvRepresentantes {
		
		//Exportacao do cadastro para formato EXCEL
        //Download do documento gerado
        public void baixarExcel(List<Representante> listaRepresentantes) {

            var OResponse = HttpContext.Current.Response;
          
            StringWriter sw = new StringWriter();

            sw.WriteLine(this.gerarCabecalhoExcel());

            foreach (var OItem in listaRepresentantes) {
                sw.WriteLine(this.gerarLinhaExcel(OItem));
            }

            OResponse.ClearContent();

            var nomeArquivo = String.Concat("lista-representante-", DateTime.Now.ToShortDateString().Replace("/", "-"), ".csv");
            
            OResponse.AddHeader("content-disposition", "attachment;filename="+nomeArquivo);
            
            OResponse.ContentType = "text/csv; charset=ISO-8859-1";
            
            OResponse.Charset = "ISO-8859-1";
            
            OResponse.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");

            OResponse.Write(sw.ToString());

            OResponse.End();
        }

        private string gerarCabecalhoExcel() {

            var cabecalho = new StringBuilder();

            cabecalho.Append("Código Sistema;");

            cabecalho.Append("Nome;")
                .Append("Atuação;")
                .Append("Telefones;")
                .Append("E-mail;")
                .Append("Site;")
                .Append("Status;");

            return cabecalho.ToString();
        }

        private string gerarLinhaExcel(Representante OItem) {

            StringBuilder linha = new StringBuilder();

            linha.Append(OItem.id).Append(";");
            linha.Append(OItem.Pessoa.nome).Append(";");
            linha.Append(OItem.Pessoa.profissao).Append(";");
            linha.Append(OItem.Pessoa.nroTelPrincipal + " | " + OItem.Pessoa.nroTelSecundario + " | " + OItem.Pessoa.nroTelTerciario).Append(";");
            linha.Append(OItem.Pessoa.emailPrincipal + " | " + OItem.Pessoa.emailSecundario).Append(";");
            linha.Append(OItem.Pessoa.enderecoWeb).Append(";");
            linha.Append(OItem.ativo == true ? "Sim" : "Não").Append(";");

            return linha.ToString();
        }
		
	}

}