using DAL.AssociadosCarteirinha;
using System;
using System.Linq;


namespace BLL.AssociadosCarteireinha {

    public interface IAssociadoCarteirinhaBL {
    
        AssociadoCarteirinha carregar(int id);
        IQueryable<AssociadoCarteirinha> listar(int idAssociado);
        bool salvar(AssociadoCarteirinha OAssociadoCarteirinha);
        UtilRetorno excluir(int id);

    }
}
