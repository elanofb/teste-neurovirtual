using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using BLL.Associados;
using BLL.NaoAssociados;
using BLL.OrganizacaoConfiguracoes;
using BLL.Organizacoes;
using BLL.Services;
using DAL.Associados;
using DAL.OrganizacaoConfiguracoes;
using DAL.Organizacoes;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.AssociadosOrganizacoes.ViewModels{

	public class AssociadoOrganizacaoVM {
	
		//Atributos Serviços
		private IAssociadoBL _AssociadoBL;
		private INaoAssociadoBL _INaoAssociadoBL;
		private IOrganizacaoBL _IOrganizacaoBL;
		private IOrganizacaoDadosAssociadoBL _IOrganizacaoDadosAssociadoBL;
        
		//Propriedades Serviços
		private IAssociadoBL OAssociadoBL => _AssociadoBL = _AssociadoBL ?? new AssociadoBL();
		private INaoAssociadoBL ONaoAssociadoBL => _INaoAssociadoBL = _INaoAssociadoBL ?? new NaoAssociadoBL();
		private IOrganizacaoBL OOrganizacaoBL => _IOrganizacaoBL = _IOrganizacaoBL ?? new OrganizacaoBL();
		private IOrganizacaoDadosAssociadoBL OrganizacaoDadosAssociadoBL => _IOrganizacaoDadosAssociadoBL = _IOrganizacaoDadosAssociadoBL ?? new OrganizacaoDadosAssociadoBL();
		
		// Constantes
		private IPrincipal User => HttpContextFactory.Current.User;
		
		// Propriedades
		public int idAssociado { get; set; }

		public Associado OAssociado { get; set; }

		public List<Organizacao> listaOutrasOrganizacoes { get; set; }
		
		public List<Associado> listaAssociadosOutrasOrganizacoes { get; set; }
		
		public List<OrganizacaoDadosAssociado> listaConfiguracoesOrganizacoes { get; set; }

		//
		public AssociadoOrganizacaoVM() {
			
			this.listaOutrasOrganizacoes = new List<Organizacao>();
			
			this.listaAssociadosOutrasOrganizacoes = new List<Associado>();
			
			this.listaConfiguracoesOrganizacoes = new List<OrganizacaoDadosAssociado>();
			
		}
		
		//
		public void carregarInformacoes() {
			
			this.carregarDadosAssociado();

			if (this.OAssociado == null || this.OAssociado.Pessoa.nroDocumento.isEmpty()) {
				return;
			}

			this.carregarAssociadosOutrasOrganizacoes();

			if (!this.listaAssociadosOutrasOrganizacoes.Any()) {
				return;
			}
			
			this.carregarConfiguracoesOrganizacoes();

		}
		
		//
		private void carregarDadosAssociado() {
			
			this.OAssociado = this.OAssociadoBL.listar(0, "", "", "").Where(x => x.id == this.idAssociado)
								  .Select(x => new {
								  	  x.id, x.idTipoCadastro, Pessoa = new { x.Pessoa.nroDocumento }		
								  }).FirstOrDefault().ToJsonObject<Associado>();
			
			if (this.OAssociado == null) {
			
				this.OAssociado = this.ONaoAssociadoBL.listar("", "").Where(x => x.id == this.idAssociado)
									  .Select(x => new {
										  x.id, x.idTipoCadastro, Pessoa = new { x.Pessoa.nroDocumento }		
									  }).FirstOrDefault().ToJsonObject<Associado>();
			}
			
		}

		//
		public void carregarAssociadosOutrasOrganizacoes() {

			var idOrganizacao = User.idOrganizacao();
			
			this.listaOutrasOrganizacoes = this.OOrganizacaoBL.listar("", null, true).Where(x => x.idOrganizacaoGestora == idOrganizacao)
											   .Select(x => new { x.id, Pessoa = new { x.Pessoa.nome } }).ToListJsonObject<Organizacao>();

			var idsOrganizacoes = this.listaOutrasOrganizacoes.Select(x => x.id).ToList();

			IQueryable<Associado> query = null;
			
			if (this.OAssociado.idTipoCadastro == AssociadoTipoCadastroConst.CONSUMIDOR) {

				query = this.OAssociadoBL.query(0);
			}

			if (this.OAssociado.idTipoCadastro == AssociadoTipoCadastroConst.COMERCIANTE) {
			
				query = this.ONaoAssociadoBL.query(0);
			}

			if (query == null) {
				return;
			}

			this.listaAssociadosOutrasOrganizacoes = query.Where(x => idsOrganizacoes.Contains(x.idOrganizacao) && x.Pessoa.nroDocumento.Equals(this.OAssociado.Pessoa.nroDocumento))
														.Select(x => new {
															x.id, x.idOrganizacao, x.nroAssociado, x.dtCadastro, x.ativo,  
															Pessoa = new {
																
																// Dados PF
																x.Pessoa.nome, x.Pessoa.nroDocumento, x.Pessoa.flagTipoPessoa,
																x.Pessoa.rg, x.Pessoa.dtNascimento, x.Pessoa.flagSexo,
																
																// Dados PJ
																x.Pessoa.razaoSocial, x.Pessoa.inscricaoEstadual, x.Pessoa.inscricaoMunicipal,
																x.Pessoa.flagOptanteSimplesNacional,
																SetorAtuacao = new { x.Pessoa.SetorAtuacao.descricao },
																EmpresaPorte = new { x.Pessoa.EmpresaPorte.descricao },
																
																// Dados Profissionais
																x.Pessoa.profissao, x.Pessoa.instituicaoFormacao, x.Pessoa.nroMatriculaEstudante,
																x.Pessoa.anoFormacao,
																
																// Dados Respoonsavel
																x.Pessoa.nomeResponsavelCadastro, x.Pessoa.documentoResponsavelCadastro,
																x.Pessoa.obsResponsavelCadastro,
																
																// Dados de Funcionários
																x.Pessoa.qtdeEmpregadosCLT, x.Pessoa.qtdeEmpregadosTerceiros, 
																x.Pessoa.qtdeEstagiarios, x.Pessoa.qtdeMenorAprendiz,
																
																// Enderecos
																listaEnderecos = x.Pessoa.listaEnderecos.Where(c => !c.dtExclusao.HasValue).Select(c => new {
																	c.cep, c.logradouro, c.numero, c.complemento, c.bairro, c.zona,
																	c.nomeCidade, c.uf,
																	Cidade = new { c.Cidade.nome }, 
																	Estado = new { c.Estado.sigla }
																}),
																
																// Dados de Contato
																listaEmails = x.Pessoa.listaEmails.Where(c => !c.dtExclusao.HasValue).Select(c => new { c.email }),
																
																listaTelefones = x.Pessoa.listaTelefones.Where(c => !c.dtExclusao.HasValue).Select(c => new { c.ddi, c.nroTelefone }),
															},

															TipoAssociado = new { x.TipoAssociado.descricao }
														}).ToListJsonObject<Associado>();
			
			if (!this.listaAssociadosOutrasOrganizacoes.Any()) {
				return;
			}

			var idsOrganizacoesAssociados = this.listaAssociadosOutrasOrganizacoes.Select(x => x.idOrganizacao).ToList();

			this.listaOutrasOrganizacoes.RemoveAll(x => !idsOrganizacoesAssociados.Contains(x.id));

		}

		private void carregarConfiguracoesOrganizacoes() {
			
			var idsOrganizacoes = this.listaOutrasOrganizacoes.Select(x => (int?) x.id).ToList();
			
			this.listaConfiguracoesOrganizacoes = this.OrganizacaoDadosAssociadoBL.listar(0)
													  .Where(x => idsOrganizacoes.Contains(x.idOrganizacao)).ToList();
			
		}

	}

}
