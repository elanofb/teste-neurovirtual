using System;
using System.Json;
using System.Linq;
using DAL.AreasAtuacao;

namespace BLL.AreasAtuacao {

    public interface IAreaAtuacaoBL {

        AreaAtuacao carregar(int id);

        IQueryable<AreaAtuacao> listar(string valorBusca, string ativo);

        bool existe(AreaAtuacao OAreaAtuacao, int id);

        bool salvar(AreaAtuacao OAreaAtuacao);

        JsonMessageStatus alterarStatus(int id);

        UtilRetorno excluir(int id, int idUsuarioExclusao);

    }
}
