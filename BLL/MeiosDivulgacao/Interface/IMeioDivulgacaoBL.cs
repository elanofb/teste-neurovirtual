using System;
using System.Json;
using System.Linq;
using DAL.MeiosDivulgacao;

namespace BLL.MeiosDivulgacao {

    public interface IMeioDivulgacaoBL {

        MeioDivulgacao carregar(int id);

        IQueryable<MeioDivulgacao> listar(int idOrganizacaoParam, string valorBusca, bool? ativo);

        bool existe(MeioDivulgacao OMeioDivulgacao, int id);

        bool salvar(MeioDivulgacao OMeioDivulgacao);

        JsonMessageStatus alterarStatus(int id);

        UtilRetorno excluir(int id, int idUsuarioExclusao);

    }
}
