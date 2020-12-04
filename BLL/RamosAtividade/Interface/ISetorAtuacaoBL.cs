using System;
using System.Json;
using System.Linq;
using DAL.RamosAtividade;

namespace BLL.RamosAtividade {

    public interface ISetorAtuacaoBL {

        SetorAtuacao carregar(int id);

        SetorAtuacao carregar(string descricaoRamoAtividade, string descricaoSetorAtuacao);

        IQueryable<SetorAtuacao> listar(int idRamoAtividade, string valorBusca, bool? ativo, int? idOrganizacaoInf = null);

        bool existe(SetorAtuacao ORamoAtividade, int id);

        bool salvar(SetorAtuacao ORamoAtividade);

        JsonMessageStatus alterarStatus(int id);

        UtilRetorno excluir(int id, int idUsuarioExclusao);

    }
}
