using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using DAL.Pessoas;
using DAL.Empresas;

namespace WEB.Areas.Empresas.ViewModels {

    public class EmpresaExportacao {
        
        //Atributos

        // Propriedades

        //
        public EmpresaExportacao() {
            
        }
        
        //
        public void baixarExcel(List<Empresa> listaUnidades) {

            var OResponse = HttpContext.Current.Response;

            StringWriter sw = new StringWriter();

            sw.WriteLine(this.gerarCabecalhoExcel());

            foreach (var OItem in listaUnidades) {
                sw.WriteLine(this.gerarLinhaExcel(OItem));
            }

            OResponse.ClearContent();

            var nomeArquivo = String.Concat("relatorios-empresas-", DateTime.Now.ToShortDateString().Replace("/", "-"), ".csv");
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
                .Append("E-mail;")
                .Append("Logradouro;")
                .Append("CEP;")
                .Append("Número;")
                .Append("Complemento;")
                .Append("Bairro;")
                .Append("Cidade;")
                .Append("UF;");
            
            return cabecalho.ToString();
        }

        private string gerarLinhaExcel(Empresa OEmpresa) {
            
            StringBuilder linha = new StringBuilder();

            linha.Append(OEmpresa.id).Append(";")
                .Append(UtilString.formatCPFCNPJ(OEmpresa.Pessoa.nroDocumento)).Append(";")
                .Append(OEmpresa.Pessoa.nome).Append(";")
                .Append(OEmpresa.Pessoa.razaoSocial).Append(";")
                .AppendFormat("=\"{0}\"", OEmpresa.Pessoa.inscricaoEstadual).Append(";")
                .AppendFormat("=\"{0}\"", OEmpresa.Pessoa.inscricaoMunicipal).Append(";")
                .Append(OEmpresa.Pessoa.emailPrincipal).Append(";");

            var Endereco = OEmpresa.Pessoa.listaEnderecos.FirstOrDefault() ?? new PessoaEndereco();
            
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