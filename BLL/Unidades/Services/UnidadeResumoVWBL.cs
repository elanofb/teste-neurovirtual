using System;
using System.Linq;
using DAL.Unidades;
using DAL.Permissao.Security.Extensions;
using BLL.Services;
using DAL.Permissao;

namespace BLL.Unidades {

	public class UnidadeResumoVWBL : DefaultBL, IUnidadeResumoVWBL {

		// Atributos

        // Propriedades

		//
		public UnidadeResumoVWBL(){
		}

		//Listagem de registro a partir de parametros
		public IQueryable<UnidadeResumoVW> listar(string ativo, bool flagTodasUnidades = true) {
			
			var query = from Unid in db.UnidadeResumoVW
						select Unid;


			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

		    int idOrganizacao = User.idOrganizacao();
		    if (idOrganizacao > 0) {		        
                query = query.Where(x => x.idOrganizacao == idOrganizacao);
		    }    
            		    
			if (!flagTodasUnidades){

                var idsUnidades = User.idsUnidades();

                query = query.Where(x => idsUnidades.Contains(x.id));
			}

			return query;
		}

	}
}