using System;
using System.Linq;
using System.Collections.Generic;
using System.Security.Principal;
using BLL.Fornecedores;
using BLL.Services;
using DAL.Fornecedores;
using DAL.Permissao.Security.Extensions;
using PagedList;
using WEB.Extensions;
using WEB.Helpers;

namespace WEB.Areas.Fornecedores.ViewModels {

    public class FornecedorVM {

	    //Atributos
	    private IFornecedorConsultaBL _FornecedorConsultaBL;

	    //Propriedades
	    private IFornecedorConsultaBL OFornecedorConsultaBL => _FornecedorConsultaBL = _FornecedorConsultaBL ?? new FornecedorConsultaBL();
	    
		public IPagedList<Fornecedor> listaFornecedoresPaged { get; set;}
		public List<Fornecedor> listaFornecedores { get; set;}
	    
	    public string flagTipoSaida { get; set; }
	    
	    // Constants
	    private IPrincipal User => HttpContextFactory.Current.User;

		//Construtor
        public FornecedorVM() {

	        this.listaFornecedoresPaged = new List<Fornecedor>().ToPagedList(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());
	        this.listaFornecedores = new List<Fornecedor>();

        }

		//Carregar lista de fornecedores
		public void carregar() {

			bool? ativo = UtilRequest.getBool("flagAtivo");
			string valorBusca = UtilRequest.getString("valorBusca");
			this.flagTipoSaida = UtilRequest.getString("tipoSaida");
			
			var query = OFornecedorConsultaBL.query(User.idOrganizacao());
			
			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.Pessoa.nome.Contains(valorBusca));
			}
			
			if (ativo != null) {
				query = query.Where(x => x.ativo == ativo);
			}
			
			if (this.flagTipoSaida == TipoSaidaHelper.EXCEL) {
				
				this.listaFornecedores = query.Select(x => new {
					x.id,
					x.idPessoa,
					x.ativo,
					x.dtCadastro,
					Organizacao = new {
						Pessoa = new { x.Organizacao.Pessoa.nome }
					},
					Pessoa = new {
						x.Pessoa.flagTipoPessoa,
						x.Pessoa.razaoSocial,
						x.Pessoa.nome,
						x.Pessoa.nroDocumento,
						x.Pessoa.inscricaoEstadual,
						x.Pessoa.inscricaoMunicipal,
						x.Pessoa.emailPrincipal,
						x.Pessoa.emailSecundario,
						x.Pessoa.nroTelPrincipal,
						x.Pessoa.nroTelSecundario,
						x.Pessoa.nroTelTerciario,
						listaEnderecos = x.Pessoa.listaEnderecos.Where(y => y.dtExclusao == null)
					}
				}).OrderBy(x => x.Pessoa.nome).ToListJsonObject<Fornecedor>();
				
				return;
			}
			
			this.listaFornecedoresPaged = query.Select(x => new {
				x.id,
				x.idPessoa,
				x.ativo,
				x.dtCadastro,
				Pessoa = new {
					x.Pessoa.flagTipoPessoa,
					x.Pessoa.razaoSocial,
					x.Pessoa.nome,
					x.Pessoa.nroDocumento,
					x.Pessoa.emailPrincipal,
					x.Pessoa.nroTelPrincipal,
				}
			}).OrderBy(x => x.Pessoa.nome).ToPagedListJsonObject<Fornecedor>(UtilRequest.getNroPagina(), UtilRequest.getNroRegistros());

		}
    }


}