using System;
using System.Linq;
using System.Web;
using EntityFramework.Extensions;
using DAL.Planos;
using DAL.Repository.Base;

namespace BLL.Planos{

    public class StatusPlanoContratacaoBL : TableRepository<StatusPlanoContratacao> {

        //
        public StatusPlanoContratacaoBL() : base(null) { 
        
        }

		//
        public IQueryable<StatusPlanoContratacao> listar(string valorBusca){ 

			var db = this.getDataContext();
            var query = from T in db.StatusPlanoContratacao 
						where T.flagExcluido == "N"
						select T;

			if (!String.IsNullOrEmpty(valorBusca)) { 
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			return query;
		}
    }
}
