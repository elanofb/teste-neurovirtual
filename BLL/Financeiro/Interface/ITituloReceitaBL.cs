using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Financeiro;

namespace BLL.Financeiro {

	public interface ITituloReceitaBL {

	    /// <summary>
	    /// Base para montagem de busca por condicoes especificas
	    /// </summary>
	    IQueryable<TituloReceita> query(int? idOrganizacaoParam = null);

		TituloReceita carregar(int id, bool? flagExcluido = false);

	    /// <summary>
	    /// Carregar o titulo dando join nos dados da pessoa
	    /// </summary>
	    TituloReceita carregarComPessoa(int id);

		TituloReceita carregarPorReceita(int id);
        
        /// <summary>
        /// Montagem de consulta com possibilidade de pre-envio de parametros
        /// </summary>
        /// <returns></returns>
		IQueryable<TituloReceita> listar(int idTipoReceita, int idReceita, int idPessoa, string valorBusca, bool? flagExcluido = false);

	    UtilRetorno substituirCategoriaEMacroConta(List<int> ids, int idNovaCategoria, int idNovaMacroConta);

	    UtilRetorno substituirMacroConta(List<int> ids, int idNovaMacroConta);

	}

}
