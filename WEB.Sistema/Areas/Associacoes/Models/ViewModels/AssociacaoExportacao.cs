using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using DAL.Organizacoes;
using DAL.Pessoas;

namespace WEB.Areas.Associacoes.ViewModels {

    public class AssociacaoExportacao {
        
        //Atributos

        // Propriedades

        //
        public AssociacaoExportacao() {
            
        }
        
        //
        public void baixarExcel(List<Organizacao> listaAssociacoes) {

            var OResponse = HttpContext.Current.Response;

            StringWriter sw = new StringWriter();

            sw.WriteLine(this.gerarCabecalhoExcel());

            foreach (var OItem in listaAssociacoes) {
                sw.WriteLine(this.gerarLinhaExcel(OItem));
            }

            OResponse.ClearContent();

            var nomeArquivo = String.Concat("relatorios-associacoes-", DateTime.Now.ToShortDateString().Replace("/", "-"), ".csv");
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
                .Append("CNPJ;")
                .Append("Nome Fantasia;")
                .Append("Razão Social;")
                .Append("Inscrição Estadual;")
                .Append("Inscrição Municipal;")
                .Append("Logradouro;")
                .Append("CEP;")
                .Append("Número;")
                .Append("Complemento;")
                .Append("Bairro;")
                .Append("Cidade;")
                .Append("UF;");
            
            return cabecalho.ToString();
        }

        private string gerarLinhaExcel(Organizacao OAssociacao) {
            
            StringBuilder linha = new StringBuilder();

            linha.Append(OAssociacao.id).Append(";")
                .Append(UtilString.formatCPFCNPJ(OAssociacao.Pessoa.nroDocumento)).Append(";")
                .Append(OAssociacao.Pessoa.nome).Append(";")
                .Append(OAssociacao.Pessoa.razaoSocial).Append(";")
                .AppendFormat("=\"{0}\"", OAssociacao.Pessoa.inscricaoEstadual).Append(";")
                .AppendFormat("=\"{0}\"", OAssociacao.Pessoa.inscricaoMunicipal).Append(";");

            var Endereco = OAssociacao.Pessoa.listaEnderecos.FirstOrDefault() ?? new PessoaEndereco();
            
            linha.Append(UtilString.limparParaCSV(Endereco.logradouro)).Append(";");
            linha.Append(UtilString.formatCEP(Endereco.cep)).Append(";");
            linha.Append(Endereco.numero).Append(";");
            linha.Append(UtilString.limparParaCSV(Endereco.complemento)).Append(";");
            linha.Append(UtilString.limparParaCSV(Endereco.bairro)).Append(";");
            linha.Append(UtilString.limparParaCSV(Endereco.Cidade?.nome)).Append(";");
            linha.Append(UtilString.limparParaCSV(Endereco.Cidade?.Estado?.sigla)).Append(";");

            return linha.ToString();
        }

    }
}