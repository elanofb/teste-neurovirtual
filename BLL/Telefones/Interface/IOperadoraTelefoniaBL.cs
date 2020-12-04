using System.Linq;
using DAL.Telefones;

namespace BLL.Telefones {

    public interface IOperadoraTelefoniaBL {
        /// <summary>
        /// Carregar um registro pelo ID
        /// </summary>
        OperadoraTelefonia carregar(int id);

        /// <summary>
        /// Listagem de registros de acordo com parametros informados
        /// </summary>
        IQueryable<OperadoraTelefonia> listar(string descricao, bool? ativo = true);

        /// <summary>
        /// Persistir e salvar os dados
        /// </summary>
        bool salvar(OperadoraTelefonia OOperadoraTelefonia);
    }
}