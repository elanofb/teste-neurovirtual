using DAL.Associados;
using System.Collections.Generic;
using System.Linq;

namespace BLL.Associados {

    public interface IPendenciaCadastralBL {

        PendenciaCadastralVW carregar(int id);

        IQueryable<PendenciaCadastralVW> listar(List<int> idsTipoAssociado, int? qtdEmails, int? qtdTel, int? qtdEnderecos, string flagSituacaoContribuicao, string ativo, string valorBusca);
    }

}