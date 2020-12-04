using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using DAL.Associados;
using WEB.Helpers;

namespace WEB.Areas.AssociadosConsultas.ViewModels {

    public class AssociadoEmailConsultaExportacao {
        
        //
        public AssociadoEmailConsultaExportacao() {

        }
            
            //
        public void baixarExcel(List<AssociadoEmailVW> listaEmails) {

            var OResponse = HttpContext.Current.Response;
            
            StringWriter sw = new StringWriter();
            sw.WriteLine(this.gerarCabecalhoExcel());

            foreach (var OItem in listaEmails) {
                sw.WriteLine(this.gerarLinhaExcel(OItem));
            }

            OResponse.ClearContent();

            var nomeArquivo = String.Concat("base-emails-", DateTime.Now.ToShortDateString().Replace("/", "-"), ".csv");
            OResponse.AddHeader("content-disposition", "attachment;filename="+nomeArquivo);
            OResponse.ContentType = "text/csv; charset=ISO-8859-1";
            OResponse.Charset = "ISO-8859-1";
            OResponse.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");

            OResponse.Write(sw.ToString());

            OResponse.End();
        }
        
        private string gerarCabecalhoExcel() {

            StringBuilder cabecalho = new StringBuilder();

            cabecalho.Append("Nome;")
                     .Append("E-mail;")
                     .Append("Tipo de E-mail;")
                     .Append("Data de Cadastro;");

            return cabecalho.ToString();

        }

        private string gerarLinhaExcel(AssociadoEmailVW OEmail) {
            
            StringBuilder linha = new StringBuilder();

            linha.Append(OEmail.nome).Append(";");
            linha.Append(OEmail.email).Append(";");
            linha.Append(OEmail.descricaoTipoEmail).Append(";");
            linha.Append(OEmail.dtCadastro.exibirData()).Append(";");

            return linha.ToString();

        }

    }
}