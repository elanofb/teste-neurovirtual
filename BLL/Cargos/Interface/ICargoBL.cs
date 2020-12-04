using System;
using System.Json;
using System.Linq;
using DAL.Cargos;

namespace BLL.Cargos {

    public interface ICargoBL {

        Cargo carregar(int id);

        IQueryable<Cargo> listar(string valorBusca, string ativo);

        bool existe(Cargo OCargo, int id);

        bool salvar(Cargo OCargo);

        JsonMessageStatus alterarStatus(int id);

        UtilRetorno excluir(int id, int idUsuarioExclusao);

    }
}
