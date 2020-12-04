using System;
using System.Linq;
using DAL.Mailings;

namespace BLL.Mailings {

    public interface ITipoMailingBL {
        TipoMailing carregar(int id);

        IQueryable<TipoMailing> listar(string ativo, string descricao);

        bool salvar(TipoMailing OTipoMailing);

        bool existe(string descricao, int idDesconsiderar);

        bool excluir(int id);
    }
}