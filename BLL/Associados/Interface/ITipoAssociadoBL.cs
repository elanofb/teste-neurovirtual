using System;
using System.Linq;
using DAL.Associados;

namespace BLL.Associados {

	public interface ITipoAssociadoBL {

		/*Rotinas de Consulta*/
		IQueryable<TipoAssociado> query(int? idOrganizacaoParam = null);
		TipoAssociado carregar(int id, int? idOrganizacaoInf = null);
		TipoAssociado carregarPorDescricao(string descricao, int idCategoria, int? idOrganizacaoInf = null);
		IQueryable<TipoAssociado> listar(string valorBusca, bool? flagIsento, string ativo, int? idOrganizacaoInf = null);

		/*Rotinas de Cadastro*/
		bool salvar(TipoAssociado OTipoAssociado);
		bool existe(string descricao, int idCategoria, int idDesconsiderado, int? idOrganizacaoInf = null);
		bool ehEstudante(int id);

		/*Rotinas de exclusao*/
		UtilRetorno excluir(int id);
	}
}