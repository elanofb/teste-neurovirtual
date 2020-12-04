using System.Linq;
using DAL.Funcionarios;
using System.Json;

namespace BLL.Funcionarios {

	public interface IFuncionarioConsultaBL {

	    IQueryable<Funcionario> query(int? idOrganizacaoParam = null);
        Funcionario carregar(int id);
        Funcionario carregar(string cnpj);
        IQueryable<Funcionario> listar(string valorBusca, string ativo);
        bool existe(string cnpj, string email, int idDesconsiderado);
    }
}
