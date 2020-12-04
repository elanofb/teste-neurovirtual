using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using DAL.Associados;

namespace WEB.Areas.AssociadosConsultas.ViewModels {

    public class AssociadoAreaAtuacaoConsultaExportacao {
        
        //
        public AssociadoAreaAtuacaoConsultaExportacao() {

        }
            
            //
        public void baixarExcel(List<AssociadoAreaAtuacaoVW> listaEmails) {
            
            var OResponse = HttpContext.Current.Response;
            
            StringWriter sw = new StringWriter();
            sw.WriteLine(this.gerarCabecalhoExcel());

            foreach (var OItem in listaEmails) {
                sw.WriteLine(this.gerarLinhaExcel(OItem));
            }
            
            OResponse.ClearContent();

            var nomeArquivo = String.Concat("consulta-area-atuacao", DateTime.Now.ToShortDateString().Replace("/", "-"), ".csv");
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
                .Append("Documento;")
                .Append("E-mail;")
                .Append("Tipo de E-mail;")
                .Append("Tipo de Cadastro;")
                .Append("Telefone;")
                .Append("Area de Atuação;");
            
            return cabecalho.ToString();

        }
        
        private string gerarLinhaExcel(AssociadoAreaAtuacaoVW OAssociado) {
            
            StringBuilder linha = new StringBuilder();
            
            linha.Append(OAssociado.nome).Append(";");
            linha.Append(OAssociado.nroDocumento).Append(";");
            linha.Append(OAssociado.email).Append(";");
            linha.Append(OAssociado.descricaoTipoEmail).Append(";");
            linha.Append(OAssociado.descricaoTipoAssociado).Append(";");
            linha.Append(OAssociado.nroTelefone).Append(";");
            linha.Append(OAssociado.descricaoAreaAtuacao).Append(";");
            
            return linha.ToString();

        }

    }
}