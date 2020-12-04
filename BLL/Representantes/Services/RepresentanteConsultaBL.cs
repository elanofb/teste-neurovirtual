using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Representantes;

namespace BLL.Representantes {

	public class RepresentanteConsultaBL : DefaultBL , IRepresentanteConsultaBL {

		//
		public RepresentanteConsultaBL() {
		}

        // 
        public IQueryable<Representante> query(int? idOrganizacaoParam = null) {

            var query = from C in db.Representante
                        where C.dtExclusao == null
                        select C;
            
            if (idOrganizacaoParam == null) {
                idOrganizacaoParam = idOrganizacao;
            }

            if (idOrganizacaoParam > 0) {
                query = query.Where(x => x.idOrganizacao == idOrganizacaoParam);
            }

            return query;

        }

        //Carregamento de registro pelo ID
        public Representante carregar(int id) {

            var query = this.query().Include(x => x.Pessoa).condicoesSeguranca();

            return query.FirstOrDefault(x => x.id == id);

        }

        //Listagem de registros de acordo com filtros
        public IQueryable<Representante> listar(string valorBusca, bool? ativo = true) {
                
            var query = this.query().Include(x => x.Pessoa).condicoesSeguranca();

            if (!String.IsNullOrEmpty(valorBusca)) {
                var documento = valorBusca.onlyNumber();
                
                query = query.Where(x => x.Pessoa.nome.Contains(valorBusca) || x.Pessoa.razaoSocial.Contains(valorBusca) || x.Pessoa.nroDocumento == documento);
            }
            
            if (ativo != null) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }
	    
	}
}