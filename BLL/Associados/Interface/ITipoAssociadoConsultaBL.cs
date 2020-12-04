using System;
using System.Linq;
using DAL.Associados;

namespace BLL.Associados {
	public interface ITipoAssociadoConsultaBL {
		IQueryable<TipoAssociado> query(int? idOrganizacaoParam = null);
        TipoAssociado carregar(int id, int? idOrganizacaoInf = null);
        TipoAssociado carregarPorDescricao(string descricao, int idCategoria, int? idOrganizacaoInf = null);
        IQueryable<TipoAssociado> listar(string valorBusca, bool? flagIsento, string ativo, int? idOrganizacaoInf = null);
	}
}