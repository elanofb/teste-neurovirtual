using System;
using System.Linq;
using BLL.Configuracoes;
using BLL.ConfiguracoesAssociados;
using DAL.ConfiguracoesAssociados;
using DAL.Permissao.Security.Extensions;
using WEB.Helpers;

namespace WEB.Areas.Associados.ViewModels{

	public class AssociadoImpressaoPJ : AssociadoImpressao{
        
        // Propriedades
        private ConfiguracaoAssociadoPJ OConfigAssociadoPJ => ConfiguracaoAssociadoPJBL.getInstance.carregar();
	    
	    //Construtor
		public AssociadoImpressaoPJ() {

		}

        /// <summary>
        /// Montar query e carregar lista de campos
        /// </summary>
        public override void carregarCampos() {

            int idOrganizacao = HttpContextFactory.Current.User.idOrganizacao();

            var query = this.OConfiguracaoCampoBL.listarFromCacheOrDefault(idOrganizacao).Where(x => x.idTipoCampoCadastro == TipoCampoCadastroConst.PJ);
            
            this.listaCampos = query.OrderBy(x => x.ordemExibicao).ToList();

        }

        public override void montarHtmlFicha() {

            this.htmlFicha = this.OFichaCadastral.htmlCorpo;

            // Esconder boxes desativados e por pessoa
            this.esconderBlocos();

            //
            var linkLogo = ConfiguracaoImagemBL.linkImagemOrganizacao(HttpContextFactory.Current.User.idOrganizacao(), ConfiguracaoImagemBL.IMAGEM_PRINT_SISTEMA);

            this.htmlFicha = this.htmlFicha.Replace("#LINK_LOGO#", linkLogo);

            this.htmlFicha = this.htmlFicha.Replace("#CSS_CUSTOMIZADO#", ".hide { display: none; } @media print { .no-display-print { display: none; } }");

            this.htmlFicha = this.htmlFicha.Replace("#CODIGO#", Associado.id.ToString());

            this.htmlFicha = this.htmlFicha.Replace("#NOME_ASSOCIACAO#", HttpContextFactory.Current.User.nomeOrganizacao());

            this.htmlFicha = this.htmlFicha.Replace("#NRO_ASSOCIADO#", Associado.nroAssociado.ToString());

            this.Replace("#NOME_ASSOCIADO#", "Associado.Pessoa.nome", Associado.Pessoa.nome);

            this.htmlFicha = this.htmlFicha.Replace("#DT_CADASTRO#", Associado.dtCadastro.exibirData(true));

            this.htmlFicha = this.htmlFicha.Replace("#DT_ADMISSAO#", Associado.dtAdmissao.exibirData());

            this.Replace("#TIPO_ASSOCIADO#", "Associado.idTipoAssociado", Associado.TipoAssociado.nomeDisplay);

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
            
            this.Replace("#NOME#", "Associado.Pessoa.nome", Associado.Pessoa.nome);

            this.Replace("#RAZAO_SOCIAL#", "Associado.Pessoa.razaoSocial", Associado.Pessoa.razaoSocial);

            this.Replace("#DT_FUNDACAO#", "Associado.Pessoa.dtNascimento", Associado.Pessoa.dtNascimento.exibirData());

            this.Replace("#SETOR_ATUACAO#", "Associado.Pessoa.idSetorAtuacao", Associado.Pessoa.SetorAtuacao?.descricao);

            this.Replace("#PORTE_EMPRESA#", "Associado.Pessoa.idEmpresaPorte", Associado.Pessoa.EmpresaPorte?.sigla);

            this.Replace("#FLAG_SIMPLES_NACIONAL#", "Associado.Pessoa.flagOptanteSimplesNacional", Associado.Pessoa.flagOptanteSimplesNacional == true ? "Sim" : "Não");
            
        }

	    private void incluirDadosDocumentos() {

            this.Replace("#NRO_DOCUMENTO#", "Associado.Pessoa.nroDocumento", UtilString.formatCPFCNPJ(Associado.Pessoa.nroDocumento));
            
            this.Replace("#IE#", "Associado.Pessoa.inscricaoEstadual", Associado.Pessoa.inscricaoEstadual);
            
            this.Replace("#INSCRICAO_MUNICIPAL#", "Associado.Pessoa.inscricaoMunicipal", Associado.Pessoa.inscricaoMunicipal);

        }

        private void incluirDadosResponsavel() {

            this.Replace("#NOME_RESPONSAVEL#", "Associado.Pessoa.nomeResponsavelCadastro", Associado.Pessoa.nomeResponsavelCadastro);

            this.Replace("#CPF_RESPONSAVEL#", "Associado.Pessoa.documentoResponsavelCadastro", UtilString.formatCPFCNPJ(Associado.Pessoa.documentoResponsavelCadastro));

            this.Replace("#OBSERVACOES_RESPONSAVEL#", "Associado.Pessoa.obsResponsavelCadastro", Associado.Pessoa.obsResponsavelCadastro);

        }

        private void incluirDadosCorporativos() {
            
            this.Replace("#ORGAO_CLASSE#", "Associado.Pessoa.idOrgaoClasse", Associado.Pessoa.OrgaoClasse?.sigla);
            this.Replace("#OBSERVACOES#", "Associado.observacoes", Associado.observacoes);

            // PJ
            this.Replace("#FLAG_FINS_LUCRATIVOS#", "Associado.Pessoa.flagFinsLucrativos", Associado.Pessoa.flagFinsLucrativos == true ? "Sim" : "Não");
            this.Replace("#FLAG_SIMPLES_NACIONAL#", "Associado.Pessoa.flagOptanteSimplesNacional", Associado.Pessoa.flagOptanteSimplesNacional == true ? "Sim" : "Não");
            this.Replace("#NRO_FUNCIONARIOS_CLT#", "Associado.Pessoa.qtdeEmpregadosCLT", Associado.Pessoa.qtdeEmpregadosCLT.ToString());
            this.Replace("#NRO_FUNCIONARIOS_TERCEIRIZADOS#", "Associado.Pessoa.qtdeEmpregadosTerceiros", Associado.Pessoa.qtdeEmpregadosTerceiros.ToString());
            this.Replace("#NRO_ESTAGIARIOS#", "Associado.Pessoa.qtdeEstagiarios", Associado.Pessoa.qtdeEstagiarios.ToString());
            this.Replace("#NRO_MENORES_APRENDIZES#", "Associado.Pessoa.qtdeEmpregadosTerceiros", Associado.Pessoa.qtdeEmpregadosTerceiros.ToString());

        }
        
        private new void incluirListaContatos() {

            var htmlDadosContatos = "";




            this.htmlFicha = this.htmlFicha.Replace("#LISTA_CONTATOS#", htmlDadosContatos);

        }

    }

}