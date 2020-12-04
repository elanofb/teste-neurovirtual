using System;
using System.Linq;
using BLL.Services;
using DAL.Escolaridades;

namespace BLL.Escolaridades {

	public class NivelEscolarBL : DefaultBL, INivelEscolarBL {

		//
		public NivelEscolarBL(){
		}

		//
		public IQueryable<NivelEscolar> listar(string valorBusca, string ativo) {
			
			var query = from B in db.NivelEscolar
				where B.flagExcluido == "N"
				select B;

			query = query.condicoesSeguranca();

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			if (!String.IsNullOrEmpty(ativo)) {
				query = query.Where(x => x.ativo == ativo);
			}

			return query;
		}	
	}
}