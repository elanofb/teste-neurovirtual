using DAL.MateriaisApoio;
using System;
using System.Linq;

namespace BLL.MateriaisApoio {

	public interface ITipoMaterialApoioBL {

	    IQueryable<TipoMaterialApoio> query(int? idOrganizacaoParam = null);
        TipoMaterialApoio carregar(int id);
		IQueryable<TipoMaterialApoio> listar(string valorBusca, string ativo);
        bool existe(string descricao, int id);
		bool salvar(TipoMaterialApoio OTipoMaterialApoio);
		UtilRetorno excluir(int id);

	}
}