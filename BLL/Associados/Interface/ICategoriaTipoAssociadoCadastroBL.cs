using System;
using System.Linq;
using DAL.Associados;

namespace BLL.Associados {
	public interface ICategoriaTipoAssociadoCadastroBL {
		//*Rotinas de Cadastro*//
		bool salvar(CategoriaTipoAssociado OCategoriaTipoAssociado);
		bool existe(string descricao, int idDesconsiderado, int? idOrganizacaoInf = null);
	}
}