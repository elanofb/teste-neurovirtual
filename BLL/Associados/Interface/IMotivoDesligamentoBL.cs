using System;
using System.Json;
using System.Linq;
using DAL.Associados;

namespace BLL.Associados {

    public interface IMotivoDesligamentoBL {

        MotivoDesligamento carregar(int id);

        IQueryable<MotivoDesligamento> listar(string valorBusca, bool? ativo);

        bool existe(MotivoDesligamento OMotivoDesligamento, int id);

        bool salvar(MotivoDesligamento OMotivoDesligamento);

        JsonMessageStatus alterarStatus(int id);

        UtilRetorno excluir(int id, int idUsuarioExclusao);

    }
}
