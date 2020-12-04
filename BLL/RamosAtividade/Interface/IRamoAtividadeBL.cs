using System;
using System.Json;
using System.Linq;
using DAL.RamosAtividade;

namespace BLL.RamosAtividade {

    public interface IRamoAtividadeBL {

        RamoAtividade carregar(int id);

        IQueryable<RamoAtividade> listar(string valorBusca, bool? ativo);

        bool existe(RamoAtividade ORamoAtividade, int id);

        bool salvar(RamoAtividade ORamoAtividade);

        JsonMessageStatus alterarStatus(int id);

        UtilRetorno excluir(int id, int idUsuarioExclusao);

    }
}
