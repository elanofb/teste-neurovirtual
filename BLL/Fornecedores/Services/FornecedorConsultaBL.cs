using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Fornecedores;

namespace BLL.Fornecedores {

	public class FornecedorConsultaBL : DefaultBL, IFornecedorConsultaBL{

		//Atributos

		//Propriedades

		//Construtor
		public FornecedorConsultaBL() {
		}
		
	    //
	    public IQueryable<Fornecedor> query(int? idOrganizacaoParam = null) {
			
	        var query = from Obj in db.Fornecedor
	                    where Obj.flagExcluido == false
	                    select Obj;
		    
		    if (idOrganizacaoParam == null) {
			    idOrganizacaoParam = idOrganizacao;
		    }
			
		    if (idOrganizacaoParam > 0) {
			    query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
		    }
            
	        return query;

	    }

		//Carregar a partir do ID
		public Fornecedor carregar(int id) {

		    var query = this.query().condicoesSeguranca().Include(x => x.Pessoa)
                                    .Include(x => x.Pessoa.listaEnderecos);
            
		    return query.FirstOrDefault(x => x.id == id);

		}

		//Listagem de registros a partir dos parametros
		public IQueryable<Fornecedor> listar(string valorBusca, bool? ativo) {

		    var query = this.query().condicoesSeguranca().Include(x => x.Pessoa);
            
			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.Pessoa.nome.Contains(valorBusca));
			}

			if (ativo != null) {
				query = query.Where(x => x.ativo == ativo);
			}
            
			return query.AsNoTracking();
		}

		//Completar busca de autocomplete
		public object autocompletar(string term, int idFornecedor) {

            var query = this.query();

		    query = query.condicoesSeguranca();

		    var listaFornecedores = query.Where(x => x.Pessoa.nome.Contains(term) &&
		                                         x.id == idFornecedor || idFornecedor == 0 && x.ativo == true)
                                         .Select(x => new {
		                                    value = x.Pessoa.nome,
		                                    x.id,
		                                    x.idOrganizacao,
		                                    label = x.Pessoa.nome,
		                                    telPrincipal = x.Pessoa.dddTelPrincipal + x.Pessoa.nroTelPrincipal,
		                                    telSecundario = x.Pessoa.dddTelSecundario + x.Pessoa.nroTelSecundario,
		                                    cnpf = x.Pessoa.nroDocumento,
		                                    x.Pessoa.emailPrincipal,
		                                    x.Pessoa.emailSecundario
		                                }).ToList();
            
			return listaFornecedores;
		}

		//Verifica a existencia para evitar a duplicidade
		public bool existe(Fornecedor OFornecedor) {

			string nroDocumento = UtilString.onlyNumber(OFornecedor.Pessoa.nroDocumento);

            var query = this.query().Where(x => x.id != OFornecedor.id);
            
            query = query.condicoesSeguranca();

			if (!String.IsNullOrEmpty(nroDocumento)) {
				query = query.Where(x => x.Pessoa.nroDocumento == nroDocumento);
			}

			if (!String.IsNullOrEmpty(OFornecedor.Pessoa.emailPrincipal)) {
				query = query.Where(x => x.Pessoa.emailPrincipal == OFornecedor.Pessoa.emailPrincipal);
			}

			if (!String.IsNullOrEmpty(OFornecedor.Pessoa.nome)) {
				query = query.Where(x => x.Pessoa.nome == OFornecedor.Pessoa.nome);
			}

			return query.Any();

		}

	}
}