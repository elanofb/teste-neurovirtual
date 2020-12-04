using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Associados;
using BLL.ConfiguracoesAssociados;
using BLL.ConfiguracoesTextos.Extensions;
using DAL.ConfiguracoesAssociados;
using DAL.DocumentosDigitais;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.NaoAssociados.ViewModels {

    public abstract class NaoAssociadoImpressao {

        //Atributos
        private IConfiguracaoAssociadoCampoBL _ConfiguracaoAssociadoCampoBL;

        //Propriedades
        public Associado NaoAssociado { get; set; }

        public DocumentoDigital OFichaCadastral { get; set; }

        protected List<ConfiguracaoAssociadoCampo> listaCampos { get; set; }

        public string htmlFicha { get; set; }

        //Servicos
        protected IConfiguracaoAssociadoCampoBL OConfiguracaoCampoBL => _ConfiguracaoAssociadoCampoBL = _ConfiguracaoAssociadoCampoBL ?? new ConfiguracaoAssociadoCampoBL();

        //Construtor
        public NaoAssociadoImpressao() {

        }

        public abstract void carregarCampos();

        public abstract void montarHtmlFicha();

        protected void incluirDadosEndereco() {

            var htmlDadosContatos = "";

            var listaEnderecos = NaoAssociado.Pessoa.listaEnderecos.Where(x => !string.IsNullOrEmpty(x.logradouro)).ToList();

            if (listaEnderecos.Any()) {

                var cont = 0;

                foreach (var OEndereco in listaEnderecos) {

                    var htmlNovaLinha = "<tr>" +
                                            $"<td>{ this.exibirCampo("Associado.Pessoa.listaEnderecos[" + cont + "].logradouro", OEndereco.logradouro) }, " +
                                            $"{ this.exibirCampo("Associado.Pessoa.listaEnderecos[" + cont + "].numero", OEndereco.numero) } " +
                                            $"{ this.exibirCampo("Associado.Pessoa.listaEnderecos[" + cont + "].complemento", OEndereco.complemento) }</td>" +

                                            $"<td>{ this.exibirCampo("Associado.Pessoa.listaEnderecos[" + cont + "].zona", OEndereco.zona) }</td>" +
                                            $"<td>{ this.exibirCampo("Associado.Pessoa.listaEnderecos[" + cont + "].bairro", OEndereco.bairro) }</td>" +
                                            $"<td>{ this.exibirCampo("Associado.Pessoa.listaEnderecos[" + cont + "].cep", UtilString.formatCEP(OEndereco.cep))}</td>" +
                                            $"<td>{ this.exibirCampo("Associado.Pessoa.listaEnderecos[" + cont + "].idCidade", OEndereco.Cidade?.nome) }</td>" +
                                            $"<td>{ this.exibirCampo("Associado.Pessoa.listaEnderecos[" + cont + "].Cidade.idEstado", OEndereco.Cidade?.Estado?.sigla) }</td>" +
                                        "</tr>";

                    htmlDadosContatos = String.Concat(htmlDadosContatos, htmlNovaLinha);

                    cont++;
                }

            } else {
                htmlDadosContatos = "<tr><td colspan=\"6\" align=\"center\">Não existe nenhum endereço para exibir no momento.</td></tr>";
            }

            this.htmlFicha = this.htmlFicha.Replace("#LISTA_ENDERECOS#", htmlDadosContatos);
        }

        protected void incluirDadosContato() {

            this.incluirDadosTelefone();

            this.incluirDadosEmail();

            this.Replace("#ENDERECO_WEB#", "Associado.Pessoa.enderecoWeb", NaoAssociado.Pessoa.enderecoWeb);
        }

        private void incluirDadosTelefone() {

            var htmlDadosContatos = "";

            var listaTelefones = NaoAssociado.Pessoa.listaTelefones.Where(x => !string.IsNullOrEmpty(x.nroTelefone)).ToList();

            if (listaTelefones.Any()) {

                var cont = 0;

                foreach (var OTelefone in listaTelefones) {

                    var htmlNovaLinha = "<tr>" +
                                            $"<td>{ this.exibirCampo("Associado.Pessoa.listaTelefones[" + cont + "].idTipoTelefone", OTelefone.TipoTelefone?.descricao) }</td>" +
                                            $"<td>{ this.exibirCampo("Associado.Pessoa.listaTelefones[" + cont + "].idOperadoraTelefonia", OTelefone.OperadoraTelefonia?.nome) }</td>" +
                                            $"<td>{ this.exibirCampo("Associado.Pessoa.listaTelefones[" + cont + "].nroTelefone", UtilString.formatPhone(OTelefone.nroTelefone)) }</td>" +
                                        "</tr>";

                    htmlDadosContatos = String.Concat(htmlDadosContatos, htmlNovaLinha);

                    cont++;

                }

            } else {

                htmlDadosContatos = "<tr><td colspan=\"6\" align=\"center\">Não existe nenhum telefone para exibir no momento.</td></tr>";
            }

            this.htmlFicha = this.htmlFicha.Replace("#LISTA_TELEFONES#", htmlDadosContatos);
        }

        private void incluirDadosEmail() {

            var htmlDadosContatos = "";

            var listaEmails = NaoAssociado.Pessoa.listaEmails.Where(x => !string.IsNullOrEmpty(x.email)).ToList();

            if (listaEmails.Any()) {

                var cont = 0;

                foreach (var OEmail in listaEmails) {

                    var htmlNovaLinha = "<tr>"
                        + $"<td>{ this.exibirCampo("Associado.Pessoa.listaEmails[" + cont + "].idTipoEmail", OEmail.TipoEmail?.descricao) }</td>"
                        + $"<td>{ this.exibirCampo("Associado.Pessoa.listaEmails[" + cont + "].email", OEmail.email) }</td>"
                        + "</tr>";

                    htmlDadosContatos = String.Concat(htmlDadosContatos, htmlNovaLinha);

                    cont++;
                }
            } else {
                htmlDadosContatos = "<tr><td colspan=\"6\" align=\"center\">Não existe nenhum email para exibir no momento.</td></tr>";
            }

            this.htmlFicha = this.htmlFicha.Replace("#LISTA_EMAILS#", htmlDadosContatos);
        }

        protected void incluirListaContatos() {

            var htmlDadosContatos = "";

/*            if (NaoAssociado.Pessoa.listaPessoaContato.Count == 0) {
                htmlDadosContatos = "<tr><td colspan=\"6\" align=\"center\">Não existe nenhum contato para exibir no momento.</td></tr>";
                this.htmlFicha = this.htmlFicha.Replace("#LISTA_CONTATOS#", htmlDadosContatos);
                return;
            }

            foreach (var OContato in NaoAssociado.Pessoa.listaPessoaContato) {

                var htmlNovaLinha = "<tr>" +
                                        $"<td>{ OContato.nome }</td>" +
                                        $"<td>{ OContato.email }</td>" +
                                        $"<td>{ OContato.TipoContato?.descricao }</td>" +
                                        $"<td>{ UtilString.formatPhone(OContato.telComercial) }</td>" +
                                        $"<td>{ UtilString.formatPhone(OContato.telCelular) }</td>" +
                                        $"<td>{ OContato.observacao }</td>" +
                                    "</tr>";

                htmlDadosContatos = String.Concat(htmlDadosContatos, htmlNovaLinha);
            }*/

            this.htmlFicha = this.htmlFicha.Replace("#LISTA_CONTATOS#", htmlDadosContatos);

        }

        protected void incluirListaRepresentantes() {

            var htmlDadosRepresentantes = "";

            //if (NaoAssociado.listaRepresentante.Count == 0) {
            //    htmlDadosRepresentantes = "<tr><td colspan=\"7\" align=\"center\">Não existe nenhum representante para exibir no momento.</td></tr>";
            //    this.htmlFicha = this.htmlFicha.Replace("#LISTA_REPRESENTANTES#", htmlDadosRepresentantes);
            //    return;
            //}

            //foreach (var ORepresentante in NaoAssociado.listaRepresentante) {

            //    var htmlNovaLinha = "<tr>" +
            //                            $"<td>{ (ORepresentante.TipoAssociadoRepresentante?.descricao) }</td>" +
            //                            $"<td>{ (ORepresentante.flagRepresentantaAssociacao == "S" ? "Sim" : "Não") }</td>" +
            //                            $"<td>{ ORepresentante.nome }</td>" +
            //                            $"<td class='docRepresentante'>{ UtilString.formatCPFCNPJ(ORepresentante.cpf) }</td>" +
            //                            $"<td class='nomeRepresentante'>{ ORepresentante.rg }</td>" +
            //                            $"<td>{ String.Concat(ORepresentante.ddiTelPrincipal, " ", ORepresentante.dddTelPrincipal, " ", ORepresentante.nroTelPrincipal) }</td>" +
            //                            $"<td>{ ORepresentante.emailPrincipal }</td>" +
            //                        "</tr>";

            //    htmlDadosRepresentantes = String.Concat(htmlDadosRepresentantes, htmlNovaLinha);
            //}

            this.htmlFicha = this.htmlFicha.Replace("#LISTA_REPRESENTANTES#", htmlDadosRepresentantes);

        }

        protected void incluirListaAreasAtuacao() {

            int idOrganizacao = HttpContextFactory.Current.User.idOrganizacao();

            this.htmlFicha = this.htmlFicha.labelTextoReplace(idOrganizacao, "area_atuacao_plural", "#TITULO_AREAS_ATUACOES_PLURAL#");
            this.htmlFicha = this.htmlFicha.labelTextoReplace(idOrganizacao, "area_atuacao_singular", "#TITULO_AREAS_ATUACOES_SINGULAR#");

            var htmlColunasCustomizadas = "";
            if (NaoAssociado.idTipoAssociado == TipoAssociadoConst.PROVEDOR || NaoAssociado.idTipoAssociado == TipoAssociadoConst.PROVEDOR_FORNECEDOR) {

                htmlColunasCustomizadas = "<td><b>Número da licença</b></td>" +
                                          "<td><b>Número do Ato</b></td>";
            }
            this.htmlFicha = this.htmlFicha.Replace("#AREAS_ATUACOES_COLUNAS_CUSTOMZADAS#", htmlColunasCustomizadas);

            var htmlDadosAreasAtuacoes = "";

            //if (NaoAssociado.listaAreaAtuacao.Count == 0) {
            //    htmlDadosAreasAtuacoes = $"<tr><td colspan=\"7\" align=\"center\">Nenhuma { LabelExtensions.labelTexto(idOrganizacao, "area_atuacao_singular") } cadastrado.</td></tr>";
            //    this.htmlFicha = this.htmlFicha.Replace("#LISTA_AREAS_ATUACOES#", htmlDadosAreasAtuacoes);
            //    return;
            //}

            //foreach (var OAreaAtuacao in NaoAssociado.listaAreaAtuacao) {

            //    var htmlNovaLinha = "<tr>" +
            //                            $"<td>{ OAreaAtuacao.AreaAtuacao.descricao }</td>";

            //    if (NaoAssociado.idTipoAssociado == TipoAssociadoConst.PROVEDOR || NaoAssociado.idTipoAssociado == TipoAssociadoConst.PROVEDOR_FORNECEDOR) {
            //        var htmlColunasCustomizadasForeach = $"<td>{ OAreaAtuacao.observacao1 }</td>" +
            //                                             $"<td>{ OAreaAtuacao.observacao2 }</td>";

            //        htmlNovaLinha = String.Concat(htmlNovaLinha, htmlColunasCustomizadasForeach);
            //    }

            //    htmlNovaLinha = String.Concat(htmlNovaLinha, "</tr>");
            //    htmlDadosAreasAtuacoes = String.Concat(htmlDadosAreasAtuacoes, htmlNovaLinha);
            //}

            this.htmlFicha = this.htmlFicha.Replace("#LISTA_AREAS_ATUACOES#", htmlDadosAreasAtuacoes);

        }

        protected void incluirListaTitulos() {

            var htmlDadosTitulos = "";

            //if (NaoAssociado.listaTitulo.Count == 0) {
            //    htmlDadosTitulos = "<tr><td colspan=\"5\" align=\"center\">Não existe nenhum título para exibir no momento.</td></tr>";
            //    this.htmlFicha = this.htmlFicha.Replace("#LISTA_TITULOS#", htmlDadosTitulos);
            //    return;
            //}

            //foreach (var OTitulo in NaoAssociado.listaTitulo) {

            //    var htmlNovaLinha = "<tr>" +
            //                            $"<td>{ OTitulo.TipoTitulo?.Instituicao?.descricao }</td>" +
            //                            $"<td>{ OTitulo.TipoTitulo?.descricao }</td>" +
            //                            $"<td>{ OTitulo.dtAquisicao.exibirData() }</td>" +
            //                            $"<td>{ OTitulo.dtProximaRenovacao.exibirData() }</td>" +
            //                            $"<td>{ OTitulo.TipoTitulo?.descricao }</td>" +
            //                            $"<td></td>" +
            //                        "</tr>";

            //    htmlDadosTitulos = String.Concat(htmlDadosTitulos, htmlNovaLinha);
            //}

            this.htmlFicha = this.htmlFicha.Replace("#LISTA_TITULOS#", htmlDadosTitulos);

        }

        // Metodos auxiliares
        protected void Replace(string hash, string nomeCampo, string valor) {

            this.htmlFicha = this.htmlFicha.Replace(hash, this.exibirCampo(nomeCampo, valor));

        }

        private string exibirCampo(string nomeCampo, string valor) {

            var flagReplacePermitido = this.listaCampos.Any(x => x.name.Equals(nomeCampo) && x.ativo == true);

            valor = flagReplacePermitido ? valor : "";

            return valor;

        }

    }

}