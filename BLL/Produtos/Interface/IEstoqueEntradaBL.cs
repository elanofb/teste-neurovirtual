using System;
using System.Linq;
using DAL.Produtos;
using DAL.Repository.Base;
using EntityFramework.Extensions;
using System.Web;
using System.Collections.Generic;

namespace BLL.Produtos {

	public interface IEstoqueEntradaBL{

		EstoqueEntrada carregar(int id);
		IQueryable<EstoqueEntrada> listar(int idFornecedor, int idProduto, string valorBusca, string ativo);
		bool salvar(EstoqueEntrada OEstoqueEntrada);
		bool existe(DateTime? dtMovimentacao, int idFornecedor, int idProduto, int qtdMovimentada, int id);
		bool excluir(int id);
        IQueryable<EstoqueEntrada> listarPorId(List<int> ids);
	}
}