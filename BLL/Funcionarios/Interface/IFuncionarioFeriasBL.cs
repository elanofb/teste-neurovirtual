using System;
using System.Linq;
using DAL.Funcionarios;

namespace BLL.Funcionarios {

	public interface IFuncionarioFeriasBL{
		FuncionarioFerias carregar(int id);
		IQueryable<FuncionarioFerias> listar(int idFuncionario, string ativo);
		bool salvar(FuncionarioFerias OContaBancaria);
		bool existe(DateTime? dtInicioFerias, DateTime? dtFimFerias, int idDesconsiderado);
		UtilRetorno excluir(int id);
	}
}