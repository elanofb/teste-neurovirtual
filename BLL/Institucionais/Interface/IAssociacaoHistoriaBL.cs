using System.Linq;
using DAL.Institucionais;
using System.Json;
using System;

namespace BLL.Institucionais {
    public interface IAssociacaoHistoriaBL {

        AssociacaoHistoria carregar(int id);

        IQueryable<AssociacaoHistoria> listar(int idOrganizacao, bool? ativo);

        bool salvar(AssociacaoHistoria OAssociacaoHistoria);
    }
}
