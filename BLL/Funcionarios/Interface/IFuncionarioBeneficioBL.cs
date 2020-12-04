using System;
using System.Linq;
using DAL.Funcionarios;

namespace BLL.Funcionarios {

	public interface IFuncionarioBeneficioBL{
		FuncionarioBeneficio carregar(int id);
		IQueryable<FuncionarioBeneficio> listar(int idFuncionario, string ativo);
		bool salvar(FuncionarioBeneficio OFuncionarioBeneficio);
		bool existe(int idBeneficio, int idDesconsiderado);
		UtilRetorno excluir(int id);
	}
}