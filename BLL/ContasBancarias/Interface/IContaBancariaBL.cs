using System.Linq;
using DAL.ContasBancarias;

namespace BLL.ContasBancarias {

    public interface IContaBancariaBL {

        IQueryable<ContaBancaria> query(int? idOrganizacaoParam = null);
        
        ContaBancaria carregar(int id);

        IQueryable<ContaBancaria> listar(string valorBusca, bool? ativo);

        bool existe(ContaBancaria OContaBancaria, bool descricao);

        bool salvar(ContaBancaria OContaBancaria);

        bool excluir(int id);
    }
}
