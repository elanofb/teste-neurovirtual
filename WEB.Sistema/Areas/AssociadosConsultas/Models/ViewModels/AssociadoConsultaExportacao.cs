using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using BLL.Pessoas;
using DAL.Associados;
using DAL.Configuracoes.Const;
using DAL.ConfiguracoesAssociados;
using WEB.Areas.Associados.Extensions;
using WEB.Helpers;
using BLL.ConfiguracoesAssociados;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using DAL.Pessoas;

namespace WEB.Areas.AssociadosConsultas.ViewModels {

    public class AssociadoConsultaExportacao {

        //Atributos
        private IPessoaEnderecoConsultaBL _PessoaEnderecoConsultaBL;
        private IPessoaEmailConsultaBL _PessoaEmailConsultaBL;
        private IPessoaTelefoneConsultaBL _PessoaTelefoneConsultaBL;
        private IConfiguracaoAssociadoCampoBL _ConfiguracaoAssociadoCampoBL;

        // Propriedades
        private IPessoaEnderecoConsultaBL OPessoaEnderecoConsultaBL => _PessoaEnderecoConsultaBL = _PessoaEnderecoConsultaBL ?? new PessoaEnderecoConsultaBL();
        private IPessoaEmailConsultaBL OPessoaEmailConsultaBL => _PessoaEmailConsultaBL = _PessoaEmailConsultaBL ?? new PessoaEmailConsultaBL(); 
        private IPessoaTelefoneConsultaBL OPessoaTelefoneConsultaBL => _PessoaTelefoneConsultaBL = _PessoaTelefoneConsultaBL ?? new PessoaTelefoneConsultaBL(); 
        private IConfiguracaoAssociadoCampoBL OConfiguracaoAssociadoCampoBL => _ConfiguracaoAssociadoCampoBL = _ConfiguracaoAssociadoCampoBL ?? new ConfiguracaoAssociadoCampoBL();


        //
        public void baixarExcel(List<AssociadoRelatorioVW> listaAssociado) {

            int idOrganizacao = HttpContext.Current.User.idOrganizacao();
            var OResponse = HttpContext.Current.Response;

            var listaCamposConfigurados = OConfiguracaoAssociadoCampoBL.listarFromCacheOrDefault(idOrganizacao).Where(x => x.idTipoCampo != ConfiguracaoTipoCampoConst.HIDDEN).OrderBy(x => x.ordemExibicao ?? 100000).ToList();

            var flagEstudante = listaAssociado.Exists(x => x.flagEstudante);
            var idsAssociadoPessoa = listaAssociado.Select(x => x.idPessoa).ToList();
            
            var listaEnderecos = OPessoaEnderecoConsultaBL.listar(0).Where(x => idsAssociadoPessoa.Contains(x.idPessoa)).ToList();

            var listaFones = this.OPessoaTelefoneConsultaBL.listar(0)
                                                            .Where(x => idsAssociadoPessoa.Contains(x.idPessoa))
                                                            .Select(x => new {x.id, x.idPessoa, x.nroTelefone, x.ddi})
                                                            .ToListJsonObject<PessoaTelefone>();
            
            var listaEmail = this.OPessoaEmailConsultaBL.listar(0)
                                                        .Where(x => idsAssociadoPessoa.Contains(x.idPessoa))
                                                        .Select(x => new {x.id, x.idPessoa, x.email})
                                                        .ToListJsonObject<PessoaEmail>();            
            
            StringWriter sw = new StringWriter();
            
            sw.WriteLine(this.gerarCabecalhoExcel(listaAssociado, listaCamposConfigurados));

            foreach (var OItem in listaAssociado) {
                
                OItem.listaEnderecos = listaEnderecos.Where(x => x.idPessoa == OItem.idPessoa).ToList();
                
                OItem.listaEmails = listaEmail.Where(x => x.idPessoa == OItem.idPessoa).ToList();
                
                OItem.listaTelefones = listaFones.Where(x => x.idPessoa == OItem.idPessoa).ToList();

                sw.WriteLine(this.gerarLinhaExcel(OItem, flagEstudante, listaCamposConfigurados));
            }

            OResponse.ClearContent();

            var nomeArquivo = String.Concat("relatorios-associados-", DateTime.Now.ToShortDateString().Replace("/", "-"), ".csv");
            OResponse.AddHeader("content-disposition", "attachment;filename="+nomeArquivo);
            OResponse.ContentType = "text/csv; charset=ISO-8859-1";
            OResponse.Charset = "ISO-8859-1";
            OResponse.ContentEncoding = Encoding.GetEncoding("ISO-8859-1");

            OResponse.Write(sw.ToString());

            OResponse.End();
        }

        private string gerarCabecalhoExcel(List<AssociadoRelatorioVW> lista, List<ConfiguracaoAssociadoCampo> listaCamposConfigurados) {

            StringBuilder cabecalho = new StringBuilder();

            cabecalho.Append("Código Sistema;").Append("N° Associado;").Append("Tipo Pessoa;");

            if (listaCamposConfigurados.Any(x => x.name == "Associado.idTipoAssociado")) { cabecalho.Append("Tipo de Associado;"); }

            if (listaCamposConfigurados.Any(x => x.name == "Associado.Pessoa.nome")) { cabecalho.Append("Nome;"); }

            if (listaCamposConfigurados.Any(x => x.name == "Associado.Pessoa.nroDocumento")) { cabecalho.Append("N. Documento;"); }

            if (listaCamposConfigurados.Any(x => x.name == "Associado.Pessoa.rg")) { cabecalho.Append("RG/I.E;"); }

            if (listaCamposConfigurados.Any(x => x.name == "Associado.Pessoa.dtNascimento")) { cabecalho.Append("Nascto.;"); }

            if (listaCamposConfigurados.Any(x => x.name == "Associado.Pessoa.listaEmails[0].email")) { cabecalho.Append("Emails;"); }

            if (listaCamposConfigurados.Any(x => x.name == "Associado.flagInformativosOnline")) { cabecalho.Append("Receber Comunicados ?;"); }

            if (listaCamposConfigurados.Any(x => x.name == "Associado.Pessoa.listaTelefones[0].nroTelefone")) { cabecalho.Append("Telefones;"); }

            var listaCamposEnderecos = listaCamposConfigurados.Where(x => x.name.StartsWith("Associado.Pessoa.listaEnderecos")).Where(x => x.name.EndsWith("logradouro")).ToList();

            for (var i = 0; i < listaCamposEnderecos.Count(); i++) {
                if (listaCamposEnderecos.Any(x => x.name == $"Associado.Pessoa.listaEnderecos[{i}].logradouro")) {
                    cabecalho
                        .Append((i + 1) + "º Endereço Tipo;")
                        .Append((i + 1) + "º Endereço CEP;")
                        .Append((i + 1) + "º Endereço Logradouro;")
                        .Append((i + 1) + "º Endereço Numero;")
                        .Append((i + 1) + "º Endereço Complemento;")
                        .Append((i + 1) + "º Endereço Bairro;")
                        .Append((i + 1) + "º Endereço Cidade;")
                        .Append((i + 1) + "º Endereço UF;");
                }
            }

            if (listaCamposConfigurados.Any(x => x.name == "Associado.Pessoa.login")) { cabecalho.Append("Login;"); }

            cabecalho.Append("Status;")
                .Append("Situação Financeira;")
                .Append("Data Admissão;")
                .Append("Data Cadastro;")
                .Append("Observações;"); ;

            if (lista.Exists(x => x.flagEstudante)) {
                if (listaCamposConfigurados.Any(x => x.name == "Associado.Pessoa.nroMatriculaEstudante")) { cabecalho.Append("Matricula;"); }

                if (listaCamposConfigurados.Any(x => x.name == "Associado.Pessoa.instituicaoFormacao")) { cabecalho.Append("Universidade/Instituição;"); }
            }

            //if (!String.IsNullOrEmpty(ConfiguracaoGeral.siglaConselhoProfissional)) {
            //    cabecalho.Append(ConfiguracaoGeral.siglaConselhoProfissional).Append(";");
            //    cabecalho.Append("UF ").Append(ConfiguracaoGeral.siglaConselhoProfissional).Append(";");                
            //}

            return cabecalho.ToString();
        }

        private string gerarLinhaExcel(AssociadoRelatorioVW OAssociado, bool existEstudante, List<ConfiguracaoAssociadoCampo> listaCamposConfigurados) {
            
            StringBuilder linha = new StringBuilder();

            linha.Append(OAssociado.id).Append(";");
            linha.Append(OAssociado.nroAssociado).Append(";");
            linha.Append(OAssociado.flagTipoPessoa == "J" ? "Jurídica" : "Física").Append(";");

            if (listaCamposConfigurados.Any(x => x.name == "Associado.idTipoAssociado")) {
                linha.Append(OAssociado.descricaoTipoAssociado).Append(";");
            }

            if (listaCamposConfigurados.Any(x => x.name == "Associado.Pessoa.nome")) {
                linha.Append(UtilString.limparParaCSV(OAssociado.nome)).Append(";");
            }

            if (listaCamposConfigurados.Any(x => x.name == "Associado.Pessoa.nroDocumento")) {
                linha.Append(UtilString.formatCPFCNPJ(OAssociado.nroDocumento)).Append(";");
            }

            if (listaCamposConfigurados.Any(x => x.name == "Associado.Pessoa.rg")) {
                linha.Append(OAssociado.rg).Append(";");
            }

            if (listaCamposConfigurados.Any(x => x.name == "Associado.Pessoa.dtNascimento")) {
                linha.Append(OAssociado.dtNascimento.exibirData()).Append(";");
            }

            if (listaCamposConfigurados.Any(x => x.name == "Associado.Pessoa.listaEmails[0].email")) {

                var listaEmailsFormat = "";
                
                if (OAssociado.listaEmails.Any()) {
                    listaEmailsFormat = string.Join(",", OAssociado.listaEmails.Select(x => x.email).Where(x => !x.isEmpty()).ToList());
                }

                linha.Append(UtilString.limparParaCSV(listaEmailsFormat)).Append(";");
            }

            if (listaCamposConfigurados.Any(x => x.name == "Associado.flagInformativosOnline")) {
                linha.Append(OAssociado.flagInformativosOnline == "S" ? "Sim" : "Não").Append(";");
            }

            if (listaCamposConfigurados.Any(x => x.name == "Associado.Pessoa.listaTelefones[0].nroTelefone")) {
                
                var listaTelefonesFormat = "";
                
                if (OAssociado.listaTelefones.Any()) {
                    
                    listaTelefonesFormat = string.Join(",", OAssociado.listaTelefones.Select(x => x.nroTelefone).Where(x => !x.isEmpty()).Select(UtilString.formatPhone).ToList());
                    
                }

                linha.Append(UtilString.limparParaCSV(listaTelefonesFormat)).Append(";");
            }

            var listaCamposEnderecos = listaCamposConfigurados.Where(x => x.name.StartsWith("Associado.Pessoa.listaEnderecos")).Where(x => x.name.EndsWith("logradouro")).ToList();
            for (var i = 0; i < listaCamposEnderecos.Count(); i++) {
                if (listaCamposConfigurados.Any(x => x.name == $"Associado.Pessoa.listaEnderecos[{i}].logradouro")) {

                    var Endereco = OAssociado.listaEnderecos.ElementAtOrDefault(i) ?? new PessoaEndereco();

                    if (Endereco.id == 0) {
                        linha.Append(";;;;;;;;");
                        continue;
                    }

                    linha.Append(Endereco.TipoEndereco?.descricao).Append(";");
                    linha.Append(UtilString.formatCEP(Endereco.cep)).Append(";");
                    linha.Append(UtilString.limparParaCSV(Endereco.logradouro)).Append(";");
                    linha.Append(Endereco.numero).Append(";");
                    linha.Append(UtilString.limparParaCSV(Endereco.complemento)).Append(";");
                    linha.Append(UtilString.limparParaCSV(Endereco.bairro)).Append(";");
                    linha.Append(UtilString.limparParaCSV(Endereco.Cidade?.nome)).Append(";");
                    linha.Append(UtilString.limparParaCSV(Endereco.Cidade?.Estado?.sigla)).Append(";");
                }
            }

            if (listaCamposConfigurados.Any(x => x.name == "Associado.Pessoa.login")) {
                linha.Append(OAssociado.login).Append(";");
            }

            linha.Append(OAssociado.exibirStatus()).Append(";");
            linha.Append(OAssociado.exibirSituacao()).Append(";");
            linha.Append(OAssociado.dtAdmissao.exibirData()).Append(";");
            linha.Append(OAssociado.dtCadastro.exibirData()).Append(";");
            linha.Append(UtilString.limparParaCSV(OAssociado.observacoes)).Append(";");

            if (existEstudante) {
                if (listaCamposConfigurados.Any(x => x.name == "Associado.Pessoa.nroMatriculaEstudante")) {
                    linha.Append(UtilString.limparParaCSV(OAssociado.nroMatriculaEstudante)).Append(";");
                }

                if (listaCamposConfigurados.Any(x => x.name == "Associado.Pessoa.instituicaoFormacao")) {
                    linha.Append(UtilString.limparParaCSV(OAssociado.nomeUniversidadeFormacao)).Append(";");
                }
            }

            //if (!String.IsNullOrEmpty(ConfiguracaoGeral.siglaConselhoProfissional)) {
            //    linha.Append(OAssociado.nroPrimeiraCOCEP).Append(";");
            //    linha.Append(OAssociado.idEstadoPrimeiraCOCEP > 0 ? "-" : OAssociado.siglaEstadoPrimeiroCOCEP).Append(";");
            //}

            return linha.ToString();
        }
    }
}