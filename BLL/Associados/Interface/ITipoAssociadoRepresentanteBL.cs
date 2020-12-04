using System;
using System.Linq;
using DAL.Associados;

namespace BLL.Associados {

	public interface ITipoAssociadoRepresentanteBL {

		TipoAssociadoRepresentante carregar(int id);

        TipoAssociadoRepresentante carregarPorDescricao(string descricao, int idCategoria);

		IQueryable<TipoAssociadoRepresentante> listar(string valorBusca, bool? flagIsento, string ativo);
		
        bool salvar(TipoAssociadoRepresentante OTipoAssociadoRepresentante);
		
        bool existe(string descricao, int idCategoria, int idDesconsiderado);

        UtilRetorno excluir(int id);

	}
}