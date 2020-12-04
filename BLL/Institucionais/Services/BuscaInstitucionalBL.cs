using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Services;
using DAL.Repository.Base;
using DAL.Institucionais;

namespace BLL.Institucionais {

	public class BuscaInstitucionalBL : DefaultBL {

		//Construtor
		public BuscaInstitucionalBL() {
		}

		//
		public IQueryable<ResultadoBuscaVW> buscar(string valorBusca, List<int> idPortal) {

			var query = from Ev in db.ResultadoBuscaVW
						select Ev;

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x =>
					x.titulo.Contains(valorBusca) ||
					x.descricao.Contains(valorBusca) ||
					x.informacaoData.Contains(valorBusca)
                );
			}

		    if (idPortal.Any()) {
		        query = query.Where(x => idPortal.Contains(x.idPortal ?? 0) || x.idPortal == 0 || x.idPortal == null);
		    }

		    return query;
		}
	}
}