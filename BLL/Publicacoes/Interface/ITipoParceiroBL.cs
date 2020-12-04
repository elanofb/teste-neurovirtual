using System;
using System.Json;
using System.Linq;
using DAL.Publicacoes;

namespace BLL.Publicacoes {
    public interface ITipoParceiroBL {

        IQueryable<TipoParceiro> query(int? idOrganizacaoParam = null);

        TipoParceiro carregar(int id);

        IQueryable<TipoParceiro> listar(int idOrganizacaoParam, string valorBusca, bool? ativo);
        
        bool existe(string descricao, int id, int idOrganizacao);

        bool salvar(TipoParceiro OTipoParceiro);

        JsonMessageStatus alterarStatus(int id);

        UtilRetorno excluir(int id);

    }
}
