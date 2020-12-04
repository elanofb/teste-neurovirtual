using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Web;
using DAL.Contatos;
using DAL.Associados;

namespace WEB.Areas.Contatos.ViewModels {

    public class ContatoConsultaExportacao {
        
        //Atributos

        // Propriedades

        //
        public ContatoConsultaExportacao() {
            
        }
        
        //
        public void baixarExcel(List<PessoaContatoVW> listaContatos) {

            var OResponse = HttpContext.Current.Response;

            StringWriter sw = new StringWriter();

            sw.WriteLine(this.gerarCabecalhoExcel());

            foreach (var OItem in listaContatos) {
                sw.WriteLine(this.gerarLinhaExcel(OItem));
            }

            OResponse.ClearContent();

            var nomeArquivo = String.Concat("relatorios-contatos-", DateTime.Now.ToShortDateString().Replace("/", "-"), ".csv");
            OResponse.AddHeader("content-disposition", "attachment;filename="+nomeArquivo);
            OResponse.ContentType = "text/csv; charset=ISO-8859-1";
            OResponse.Charset = "ISO-8859-1";
            OResponse.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");

            OResponse.Write(sw.ToString());

            OResponse.End();

        }

        private string gerarCabecalhoExcel() {

            StringBuilder cabecalho = new StringBuilder();

            cabecalho.Append("Código do Contato;")
                .Append("Código do Associado;")
                .Append("Nome do Contato;")
                .Append("Nome do Associado;")
                .Append("Área do Contato;")
                .Append("Tipo do Associado;")
                .Append("Situação Financeira;")
                .Append("Status;")
                .Append("Email;")
                .Append("Celular;")
                .Append("Telefone Comercial;")
                .Append("Data de Cadastro do Contato;")
                .Append("Data de Cadastro do Associado;")
                .Append("Observação;");
            
            return cabecalho.ToString();
        }

        private string gerarLinhaExcel(PessoaContatoVW OPessoaContatoVW) {
            
            StringBuilder linha = new StringBuilder();

            linha.Append(OPessoaContatoVW.idContato).Append(";")
                .Append(OPessoaContatoVW.idAssociado).Append(";")
                .Append(OPessoaContatoVW.nomeContato).Append(";")
                .Append(OPessoaContatoVW.nomeAssociado).Append(";")
                .Append(OPessoaContatoVW.descricaoTipoContato).Append(";")
                .Append(OPessoaContatoVW.descricaoTipoAssociado).Append(";")
                .Append(OPessoaContatoVW.flagSituacaoContribuicao == SituacaoContribuicaoConst.ADIMPLENTE ? "Adimplente" : (OPessoaContatoVW.flagSituacaoContribuicao == SituacaoContribuicaoConst.INADIMPLENTE ? "Inadimplente" : "Isento")).Append(";")
                .Append(OPessoaContatoVW.exibirStatus()).Append(";")
                .Append(OPessoaContatoVW.emailContato).Append(";")
                .Append(UtilString.formatPhone(OPessoaContatoVW.telCelular)).Append(";")
                .Append(UtilString.formatPhone(OPessoaContatoVW.telComercial)).Append(";")
                .Append(OPessoaContatoVW.dtCadastroContato).Append(";")
                .Append(OPessoaContatoVW.dtCadastroAssociado).Append(";")
                .Append(OPessoaContatoVW.observacao).Append(";");

            return linha.ToString();
        }

    }
}