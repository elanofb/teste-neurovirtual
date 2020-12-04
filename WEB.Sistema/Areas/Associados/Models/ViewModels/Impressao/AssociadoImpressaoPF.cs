using System;
using System.Linq;
using BLL.Configuracoes;
using BLL.ConfiguracoesAssociados;
using DAL.ConfiguracoesAssociados;
using DAL.Localizacao;
using DAL.Permissao.Security.Extensions;
using WEB.Helpers;

namespace WEB.Areas.Associados.ViewModels{

	public class AssociadoImpressaoPF : AssociadoImpressao{
        
		//Propriedades
        private ConfiguracaoAssociadoPF OConfigAssociadoPF => ConfiguracaoAssociadoPFBL.getInstance.carregar();
	    
	    //Construtor
		public AssociadoImpressaoPF() {

		}

        /// <summary>
        /// Montar query e carregar lista de campos
        /// </summary>
        public override void carregarCampos() {

            int idOrganizacao = HttpContextFactory.Current.User.idOrganizacao();

            var query = this.OConfiguracaoCampoBL.listarFromCacheOrDefault(idOrganizacao).Where(x => x.idTipoCampoCadastro == TipoCampoCadastroConst.PF);
            
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

            //Dados Pessoais
            this.incluirDadosPessoais();

            // Endereços
            this.incluirDadosEndereco();
            
            // Telefones e Emails
            this.incluirDadosContato();

            //Documentos
            this.incluirDadosDocumentos();

            //Dados Específicos
            this.incluirDadosProfissionais();

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
            
            if (OConfigAssociadoPF.flagAbaContato == false) {
                this.htmlFicha = this.htmlFicha.Replace("#INFO_CONTATOS#", "hide");
            }

            if (OConfigAssociadoPF.flagAbaAreasAtuacao == false) {
                this.htmlFicha = this.htmlFicha.Replace("#INFO_AREAS_ATUACOES#", "hide");
            }

            if (OConfigAssociadoPF.flagAbaTitulos == false) {
                this.htmlFicha = this.htmlFicha.Replace("#INFO_TITULOS#", "hide");
            }
        }

        private void incluirDadosPessoais() {

            this.Replace("#NOME#", "Associado.Pessoa.nome", Associado.Pessoa.nome);

            this.Replace("#SEXO#", "Associado.Pessoa.flagSexo", (Associado.Pessoa.flagSexo == "M" ? "Masculino" : "Feminino"));
            
            var PaisNascimento = this.Associado.Pessoa.PaisOrigem ?? new Pais();
            var CidadeNascto = this.Associado.Pessoa.CidadeOrigem ?? new Cidade();

            this.Replace("#DT_NASCIMENTO#", "Associado.Pessoa.dtNascimento", Associado.Pessoa.dtNascimento.exibirData());

            this.Replace("#NACIONALIDADE#", "Associado.Pessoa.idPaisOrigem", PaisNascimento.nome);

            this.Replace("#NATURALIDADE#", "Associado.Pessoa.idCidadeOrigem", CidadeNascto.nome);
        }
        
	    private void incluirDadosDocumentos() {

            this.Replace("#NRO_DOCUMENTO#", "Associado.Pessoa.nroDocumento", UtilString.formatCPFCNPJ(Associado.Pessoa.nroDocumento));
            
            this.Replace("#RG_IE#", "Associado.Pessoa.rg", Associado.Pessoa.rg);
            this.Replace("#ESTADO_EMISSOR#", "Associado.Pessoa.idEstadoEmissaoRg", Associado.Pessoa.idEstadoEmissaoRg.ToString());
            this.Replace("#ORGAO_EMISSOR_RG#", "Associado.Pessoa.orgaoEmissorRg", Associado.Pessoa.orgaoEmissorRg);
            this.Replace("#NRO_CNH#", "Associado.Pessoa.nroCNH", Associado.Pessoa.nroCNH);
            this.Replace("#CATEGORIA_CNH#", "Associado.Pessoa.categoriaCNH", Associado.Pessoa.categoriaCNH);
            this.Replace("#PASSAPORTE#", "Associado.Pessoa.passaporte", Associado.Pessoa.passaporte);
            
        }

        private void incluirDadosProfissionais() {

            this.Replace("#UNIVERSIDADE#", "Associado.Pessoa.instituicaoFormacao", Associado.Pessoa.instituicaoFormacao);
            this.Replace("#NRO_MATRICULA#", "Associado.Pessoa.nroMatriculaEstudante", Associado.Pessoa.nroMatriculaEstudante);
            this.Replace("#ANO_FORMACAO#", "Associado.Pessoa.anoFormacao", Associado.Pessoa.anoFormacao.ToString());
            
            this.Replace("#PROFISSAO#", "Associado.Pessoa.profissao", Associado.Pessoa.profissao);
            this.Replace("#ORGAO_CLASSE#", "Associado.Pessoa.idOrgaoClasse", Associado.Pessoa.OrgaoClasse?.sigla);
            this.Replace("#NRO_REGISTRO_ORGAO_CLASSE#", "Associado.Pessoa.nroRegistroOrgaoClasse", Associado.Pessoa.nroRegistroOrgaoClasse);
            
            this.Replace("#OBSERVACOES#", "Associado.observacoes", Associado.observacoes);

        }
        

    }

}