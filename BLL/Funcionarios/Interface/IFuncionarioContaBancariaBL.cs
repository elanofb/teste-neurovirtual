using System;
using System.Linq;
using DAL.Funcionarios;

namespace BLL.Funcionarios {

	public interface IFuncionarioContaBancariaBL{
		FuncionarioContaBancaria carregar(int id);
		IQueryable<FuncionarioContaBancaria> listar(int idFuncionario, string ativo);
		bool salvar(FuncionarioContaBancaria OContaBancaria);
		bool existe(string codigoBanco, string nroAgencia, string nroConta, int idDesconsiderar);
		UtilRetorno excluir(int id);
	}
}