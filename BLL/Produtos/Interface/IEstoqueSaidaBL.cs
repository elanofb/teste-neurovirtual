using System;
using System.Linq;
using DAL.Produtos;
using DAL.Repository.Base;
using EntityFramework.Extensions;
using System.Web;
using System.Collections.Generic;

namespace BLL.Produtos {

	public interface IEstoqueSaidaBL{


		EstoqueSaida carregar(int id);
        IQueryable<EstoqueSaida> listar(int idTipoReferenciaSaida, int idReferencia, int idProduto, string valorBusca, string ativo);
		bool salvar(EstoqueSaida OEstoqueSaida);
		bool existe(DateTime? dtMovimentacao, int idReferecia, int idTipoReferencia, int idProduto, int qtdMovimentada, int id);
		bool excluir(int id);
        IQueryable<EstoqueSaida> listarPorId(List<int> ids);
	}
}