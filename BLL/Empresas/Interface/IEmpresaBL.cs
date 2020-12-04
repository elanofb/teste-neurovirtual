using System.Linq;
using DAL.Empresas;

namespace BLL.Empresas {

	public interface IEmpresaBL {

		Empresa carregar(int id);
		Empresa carregar(string cnpj);
		IQueryable<Empresa> listar(string valorBusca, string ativo);

		bool salvar(Empresa OEmpresa);
		bool existe(string cnpj, string email, int idDesconsiderado);
		bool alterarStatus(int idEmpresa);
		bool excluir(int idEmpresa);
	}
}
