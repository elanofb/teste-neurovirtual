using System;
using System.Linq;
using DAL.Associados;

namespace BLL.Associados {

	public interface ITipoAssociadoCadastroBL {
        bool salvar(TipoAssociado OTipoAssociado);
		bool existe(string descricao, int idCategoria, int idDesconsiderado, int? idOrganizacaoInf = null);
	    bool ehEstudante(int id);
	}
}