using System.Collections.Generic;
using System.Linq;
using DAL.Organizacoes;

namespace BLL.Organizacoes {

    public interface IOrganizacaoConsultaBL {
        
        Organizacao carregar(int id);

        /// <summary>
        /// Listagem de registro a partir de parametros
        /// </summary>
        IQueryable<Organizacao> query(int idOrganizacaoParam);

        /// <summary>
        /// Listagem de registro a partir de parametros
        /// </summary>
        List<Organizacao> listarHabilitadas(int idOrganizacaoParam);
    }

}