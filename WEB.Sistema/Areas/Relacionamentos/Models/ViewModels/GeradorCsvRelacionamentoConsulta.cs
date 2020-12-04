using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using DAL.Relacionamentos;

namespace WEB.Areas.Relacionamentos.ViewModels {

    public class GeradorCsvRelacionamentoConsulta {
        
        //
        public GeradorCsvRelacionamentoConsulta() {
            
        }
        
        //
        public void baixarExcel(List<PessoaRelacionamentoVW> listaOcorrencias) {

            var OResponse = HttpContext.Current.Response;

            StringWriter sw = new StringWriter();

            sw.WriteLine(this.gerarCabecalhoExcel());

            foreach (var OItem in listaOcorrencias) {
                sw.WriteLine(this.gerarLinhaExcel(OItem));
            }

            OResponse.ClearContent();

            var nomeArquivo = String.Concat("relatorio-historico-relacionamentos-", DateTime.Now.ToShortDateString().Replace("/", "-"), ".csv");
            OResponse.AddHeader("content-disposition", "attachment;filename="+nomeArquivo);
            OResponse.ContentType = "text/csv; charset=ISO-8859-1";
            OResponse.Charset = "ISO-8859-1";
            OResponse.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");

            OResponse.Write(sw.ToString());

            OResponse.End();

        }

        private string gerarCabecalhoExcel() {

            StringBuilder cabecalho = new StringBuilder();

            cabecalho.Append("Código da Ocorrência;")
                     .Append("Nome do Associado/Não Associado;")
                     .Append("Ocorrência;")
                     .Append("Data da Ocorrência;")
                     .Append("Observações;")
                     .Append("Data de Cadastro;")
                     .Append("Usuário Cadastro;");
            
            return cabecalho.ToString();
        }

        private string gerarLinhaExcel(PessoaRelacionamentoVW OOcorrencia) {
            
            StringBuilder linha = new StringBuilder();

            linha.Append(OOcorrencia.id).Append(";")
                 .Append(OOcorrencia.nomePessoa).Append(";")
                 .Append(OOcorrencia.descricaoTipoOcorrencia).Append(";")
                 .Append(OOcorrencia.dtOcorrencia.exibirData()).Append(";")
                 .Append(OOcorrencia.observacao).Append(";")
                 .Append(OOcorrencia.dtCadastroOcorrencia.exibirData()).Append(";")
                 .Append(OOcorrencia.nomeUsuarioCadastro).Append(";");
                
            return linha.ToString();
        }

    }
}