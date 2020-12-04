using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using DAL.Pessoas;
using DAL.Fornecedores;

namespace WEB.Areas.Fornecedores.ViewModels {

    public class FornecedorExportacao {
        
        //Atributos

        // Propriedades

        //
        public FornecedorExportacao() {
            
        }
        
        //
        public void baixarExcel(List<Fornecedor> listaFornecedores) {

            var OResponse = HttpContext.Current.Response;

            StringWriter sw = new StringWriter();

            sw.WriteLine(this.gerarCabecalhoExcel());

            foreach (var OItem in listaFornecedores) {
                sw.WriteLine(this.gerarLinhaExcel(OItem));
            }

            OResponse.ClearContent();

            var nomeArquivo = String.Concat("relatorios-fornecedores-", DateTime.Now.ToShortDateString().Replace("/", "-"), ".csv");
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
                .Append("Pessoa Física ou Jurídica?;")

                .Append("Nome;")
                .Append("CPF;")
                .Append("RG;")

                .Append("CNPJ;")
                .Append("Razão Social;")
                .Append("Inscrição Estadual;")
                .Append("Inscrição Municipal;")

                .Append("Associação;")

                .Append("E-mail Principal;")
                .Append("E-mail Secundário;")
                .Append("1º Telefone;")
                .Append("2º Telefone;")
                .Append("3º Telefone;")

                .Append("Logradouro;")
                .Append("CEP;")
                .Append("Número;")
                .Append("Complemento;")
                .Append("Bairro;")
                .Append("Cidade;")
                .Append("UF;");
            
            return cabecalho.ToString();
        }

        private string gerarLinhaExcel(Fornecedor OFornecedor) {
            
            StringBuilder linha = new StringBuilder();

            linha.Append(OFornecedor.id).Append(";")
                .Append(OFornecedor.Pessoa.flagTipoPessoa == "F" ? "Física" : "Jurídica").Append(";")

                .Append(OFornecedor.Pessoa.nome).Append(";")
                .Append(OFornecedor.Pessoa.flagTipoPessoa == "F" ? UtilString.formatCPFCNPJ(OFornecedor.Pessoa.nroDocumento) : "").Append(";")
                .AppendFormat("=\"{0}\"", OFornecedor.Pessoa.rg).Append(";")

                .Append(OFornecedor.Pessoa.flagTipoPessoa == "J" ? UtilString.formatCPFCNPJ(OFornecedor.Pessoa.nroDocumento) : "").Append(";")
                .Append(OFornecedor.Pessoa.razaoSocial).Append(";")
                .AppendFormat("=\"{0}\"", OFornecedor.Pessoa.inscricaoEstadual).Append(";")
                .AppendFormat("=\"{0}\"", OFornecedor.Pessoa.inscricaoMunicipal).Append(";")

                .Append(OFornecedor.Organizacao?.Pessoa.nome).Append(";")

                .Append(OFornecedor.Pessoa.emailPrincipal).Append(";")
                .Append(OFornecedor.Pessoa.emailSecundario).Append(";")
                .Append(OFornecedor.Pessoa.nroTelPrincipal).Append(";")
                .Append(OFornecedor.Pessoa.nroTelSecundario).Append(";")
                .Append(OFornecedor.Pessoa.nroTelTerciario).Append(";");

            var Endereco = OFornecedor.Pessoa.listaEnderecos.FirstOrDefault() ?? new PessoaEndereco();
            
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