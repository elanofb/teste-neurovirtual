using System;
using System.Collections.Generic;
using System.Json;
using System.Linq;
using System.Web;
using DAL.Ajudas;

namespace BLL.Ajudas {

    public interface IAjudaModuloBL {

        IQueryable<AjudaModulo> query();

        AjudaModulo carregar(int id);

        IQueryable<AjudaModulo> listar(string valorBusca, bool? ativo);

        bool existe(string descricao, int id);

        bool salvar(AjudaModulo OAjudaModulo);

        JsonMessageStatus alterarStatus(int id);

        UtilRetorno excluir(int id);

    }
}
