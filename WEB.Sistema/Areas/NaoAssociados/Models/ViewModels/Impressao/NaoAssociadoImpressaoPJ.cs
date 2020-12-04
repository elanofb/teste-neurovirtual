using System;
using System.Linq;
using BLL.ConfiguracoesAssociados;
using DAL.ConfiguracoesAssociados;
using DAL.Permissao.Security.Extensions;
using WEB.Helpers;

namespace WEB.Areas.NaoAssociados.ViewModels{

	public class NaoAssociadoImpressaoPJ : NaoAssociadoImpressao{
        
        // Propriedades
        private ConfiguracaoAssociadoPJ OConfigAssociadoPJ => ConfiguracaoAssociadoPJBL.getInstance.carregar();
	    
	    //Construtor
		public NaoAssociadoImpressaoPJ() {

		}

        /// <summary>
        /// Montar query e carregar lista de campos
        /// </summary>
        public override void carregarCampos() {

            int idOrganizacao = HttpContextFactory.Current.User.idOrganizacao();

            var query = this.OConfiguracaoCampoBL.listarFromCacheOrDefault(idOrganizacao).Where(x => x.idTipoCampoCadastro == TipoCampoCadastroConst.NA_PJ);
            
            this.listaCampos = query.OrderBy(x => x.ordemExibicao).ToList();

        }

        public override void montarHtmlFicha() {

            this.htmlFicha = this.OFichaCadastral.htmlCorpo;

            // Esconder boxes desativados e por pessoa
            this.esconderBlocos();

            //
            var linkLogo = String.Concat(UtilConfig.linkAbsSistema, "/upload/logotipo/", HttpContextFactory.Current.User.idOrganizacao(), "/logotipo_print.png");

            this.htmlFicha = this.htmlFicha.Replace("#LINK_LOGO#", linkLogo);

            this.htmlFicha = this.htmlFicha.Replace("#CSS_CUSTOMIZADO#", ".hide { display: none; } @media print { .no-display-print { display: none; } }");

            this.htmlFicha = this.htmlFicha.Replace("#CODIGO#", NaoAssociado.id.ToString());

            this.htmlFicha = this.htmlFicha.Replace("#NOME_ASSOCIACAO#", HttpContextFactory.Current.User.nomeOrganizacao());

            this.htmlFicha = this.htmlFicha.Replace("#NRO_ASSOCIADO#", NaoAssociado.nroAssociado.ToString());

            this.Replace("#NOME_ASSOCIADO#", "Associado.Pessoa.nome", NaoAssociado.Pessoa.nome);

            this.htmlFicha = this.htmlFicha.Replace("#DT_CADASTRO#", NaoAssociado.dtCadastro.exibirData(true));

            this.htmlFicha = this.htmlFicha.Replace("#DT_ADMISSAO#", NaoAssociado.dtAdmissao.exibirData());

            this.Replace("#TIPO_ASSOCIADO#", "Associado.idTipoAssociado", NaoAssociado.TipoAssociado.nomeDisplay);

            //Dados Cadastrais
            this.incluirDadosEmpresariais();

            // Endereços
            this.incluirDadosEndereco();
            
            // Telefones e Emails
            this.incluirDadosContato();

            //Documentos
            this.incluirDadosDocumentos();

            //Dados Específicos
            this.incluirDadosCorporativos();

            //Dados do Responsável
            this.incluirDadosResponsavel();

            //Lista de Contatos
            this.incluirListaContatos();

            //Lista de Representantes
            this.incluirListaRepresentantes();

            //Lista de Áreas de Atuações
            this.incluirListaAreasAtuacao();

            // Lista de Títulos
            this.incluirListaTitulos();
            
        }

        private void esconderBlocos() {
            
            if (OConfigAssociadoPJ.flagAbaRepresentantes == false) {
                this.htmlFicha = this.htmlFicha.Replace("#INFO_REPRESENTANTES#", "hide");
            }

            if (OConfigAssociadoPJ.flagAbaContato == false) {
                this.htmlFicha = this.htmlFicha.Replace("#INFO_CONTATOS#", "hide");
            }

            if (OConfigAssociadoPJ.flagAbaAreasAtuacao == false) {
                this.htmlFicha = this.htmlFicha.Replace("#INFO_AREAS_ATUACOES#", "hide");
            }

            if (OConfigAssociadoPJ.flagAbaTitulos == false) {
                this.htmlFicha = this.htmlFicha.Replace("#INFO_TITULOS#", "hide");
            }

        }

        private void incluirDadosEmpresariais() {
            
            this.Replace("#NOME#", "Associado.Pessoa.nome", NaoAssociado.Pessoa.nome);

            this.Replace("#RAZAO_SOCIAL#", "Associado.Pessoa.razaoSocial", NaoAssociado.Pessoa.razaoSocial);

            this.Replace("#DT_FUNDACAO#", "Associado.Pessoa.dtNascimento", NaoAssociado.Pessoa.dtNascimento.exibirData());

            this.Replace("#SETOR_ATUACAO#", "Associado.Pessoa.idSetorAtuacao", NaoAssociado.Pessoa.SetorAtuacao?.descricao);

            this.Replace("#PORTE_EMPRESA#", "Associado.Pessoa.idEmpresaPorte", NaoAssociado.Pessoa.EmpresaPorte?.sigla);

            this.Replace("#FLAG_SIMPLES_NACIONAL#", "Associado.Pessoa.flagOptanteSimplesNacional", NaoAssociado.Pessoa.flagOptanteSimplesNacional == true ? "Sim" : "Não");
            
        }

	    private void incluirDadosDocumentos() {

            this.Replace("#NRO_DOCUMENTO#", "Associado.Pessoa.nroDocumento", UtilString.formatCPFCNPJ(NaoAssociado.Pessoa.nroDocumento));
            
            this.Replace("#IE#", "Associado.Pessoa.inscricaoEstadual", NaoAssociado.Pessoa.inscricaoEstadual);
            
            this.Replace("#INSCRICAO_MUNICIPAL#", "Associado.Pessoa.inscricaoMunicipal", NaoAssociado.Pessoa.inscricaoMunicipal);

        }

        private void incluirDadosResponsavel() {

            this.Replace("#NOME_RESPONSAVEL#", "Associado.Pessoa.nomeResponsavelCadastro", NaoAssociado.Pessoa.nomeResponsavelCadastro);

            this.Replace("#CPF_RESPONSAVEL#", "Associado.Pessoa.documentoResponsavelCadastro", UtilString.formatCPFCNPJ(NaoAssociado.Pessoa.documentoResponsavelCadastro));

            this.Replace("#OBSERVACOES_RESPONSAVEL#", "Associado.Pessoa.obsResponsavelCadastro", NaoAssociado.Pessoa.obsResponsavelCadastro);

        }

        private void incluirDadosCorporativos() {
            
            this.Replace("#ORGAO_CLASSE#", "Associado.Pessoa.idOrgaoClasse", NaoAssociado.Pessoa.OrgaoClasse?.sigla);
            this.Replace("#OBSERVACOES#", "Associado.observacoes", NaoAssociado.observacoes);

            // PJ
            this.Replace("#FLAG_FINS_LUCRATIVOS#", "Associado.Pessoa.flagFinsLucrativos", NaoAssociado.Pessoa.flagFinsLucrativos == true ? "Sim" : "Não");
            this.Replace("#FLAG_SIMPLES_NACIONAL#", "Associado.Pessoa.flagOptanteSimplesNacional", NaoAssociado.Pessoa.flagOptanteSimplesNacional == true ? "Sim" : "Não");
            this.Replace("#NRO_FUNCIONARIOS_CLT#", "Associado.Pessoa.qtdeEmpregadosCLT", NaoAssociado.Pessoa.qtdeEmpregadosCLT.ToString());
            this.Replace("#NRO_FUNCIONARIOS_TERCEIRIZADOS#", "Associado.Pessoa.qtdeEmpregadosTerceiros", NaoAssociado.Pessoa.qtdeEmpregadosTerceiros.ToString());
            this.Replace("#NRO_ESTAGIARIOS#", "Associado.Pessoa.qtdeEstagiarios", NaoAssociado.Pessoa.qtdeEstagiarios.ToString());
            this.Replace("#NRO_MENORES_APRENDIZES#", "Associado.Pessoa.qtdeEmpregadosTerceiros", NaoAssociado.Pessoa.qtdeEmpregadosTerceiros.ToString());

        }
        
        private new void incluirListaContatos() {

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
                                        $"<td>{ (OContato.TipoContato?.descricao) }</td>" +
                                        $"<td>{ UtilString.formatPhone(OContato.telComercial) }</td>" +
                                        $"<td>{ UtilString.formatPhone(OContato.telCelular) }</td>" +
                                        $"<td>{ OContato.observacao }</td>" +
                                    "</tr>";

                htmlDadosContatos = String.Concat(htmlDadosContatos, htmlNovaLinha);
            }*/

            this.htmlFicha = this.htmlFicha.Replace("#LISTA_CONTATOS#", htmlDadosContatos);

        }

    }

}