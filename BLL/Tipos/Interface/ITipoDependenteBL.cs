using System;
using System.Linq;
using DAL.Tipos;

namespace BLL.Tipos {

	public interface ITipoDependenteBL{
		TipoDependente carregar(int id);
		IQueryable<TipoDependente> listar(string ativo);
		bool salvar(TipoDependente OTipoDependente);
		bool existe(string descricao, int idDesconsiderar);
		UtilRetorno excluir(int id);
	}
}