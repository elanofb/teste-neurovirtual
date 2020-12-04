using System;
using System.Collections.Generic;
using System.Linq;
using DAL.ConfiguracoesEtiquetas;

namespace BLL.ConfiguracoesEtiquetas {

	public interface IConfiguracaoEtiquetaBL {

	    ConfiguracaoEtiqueta carregar(int id, int? idOrganizacaoParam = null);

	    IQueryable<ConfiguracaoEtiqueta> listar(int? idOrganizacaoParam = null);

	    List<ConfiguracaoEtiqueta> listarFromCache(int idOrganizacaoParam, bool flagCache = true);

	    bool salvar(ConfiguracaoEtiqueta OConfiguracoes);

	    UtilRetorno excluir(int id);

	}
}