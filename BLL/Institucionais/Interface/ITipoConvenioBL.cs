using System.Linq;
using DAL.Institucionais;
using System.Json;
using System;

namespace BLL.Institucionais {

    public interface ITipoConvenioBL {

        TipoConvenio carregar(int id);

        IQueryable<TipoConvenio> listar(string valorBusca, bool? ativo);

        bool existe(string chaveUrl, int id);

        bool salvar(TipoConvenio OTipoConvenio);

        JsonMessageStatus alterarStatus(int id);

        UtilRetorno excluir(int id, int idUsuarioExclusao);
    }
}
