using System;
using System.Linq;
using BLL.Services;
using DAL.ConfiguracoesTextos;

namespace BLL.ConfiguracoesTextos {

	public class ConfiguracaoTextoCategoriaBL : DefaultBL, IConfiguracaoTextoCategoriaBL {

		//
		public ConfiguracaoTextoCategoriaBL(){
		}

		public IQueryable<ConfiguracaoTextoCategoria> listar(string valorBusca) {
			
			var query = from C in db.ConfiguracaoTextoCategoria
                        where C.dtExclusao == null
						select C;

			if (!String.IsNullOrEmpty(valorBusca)) {
				query = query.Where(x => x.descricao.Contains(valorBusca));
			}

			return query;
		}
	}
}