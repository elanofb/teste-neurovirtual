using System;
using System.Linq;
using DAL.Produtos;
using DAL.Repository.Base;
using EntityFramework.Extensions;

namespace BLL.Produtos {

	public interface ITipoReferenciaSaidaBL{

		TipoReferenciaSaida carregar(int id);
		IQueryable<TipoReferenciaSaida> listar(string valorBusca, string ativo);
		bool existe(string descricao, int id);
		bool salvar(TipoReferenciaSaida OTipoReferenciaSaida);
		bool excluir(int id);
	}
}