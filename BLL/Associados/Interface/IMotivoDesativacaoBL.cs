using System;
using System.Json;
using System.Linq;
using DAL.Associados;

namespace BLL.Associados {

    public interface IMotivoDesativacaoBL {

        MotivoDesativacao carregar(int id);

        IQueryable<MotivoDesativacao> listar(string valorBusca, bool? ativo);

        bool existe(MotivoDesativacao OMotivoDesativacao, int id);

        bool salvar(MotivoDesativacao OMotivoDesativacao);

        JsonMessageStatus alterarStatus(int id);

        UtilRetorno excluir(int id, int idUsuarioExclusao);
    }
}
