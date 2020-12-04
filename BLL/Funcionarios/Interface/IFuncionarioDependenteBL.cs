using System;
using System.Linq;
using DAL.Funcionarios;

namespace BLL.Funcionarios {

	public interface IFuncionarioDependenteBL{
		FuncionarioDependente carregar(int id);
		IQueryable<FuncionarioDependente> listar(int idFuncionario, int idTipoDependente, string ativo);
		bool salvar(FuncionarioDependente ODependente);
		bool existe(string nroDocumento, int idDesconsiderar);
		UtilRetorno excluir(int id);
	}
}