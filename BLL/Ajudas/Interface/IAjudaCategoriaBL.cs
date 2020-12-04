using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DAL.Ajudas;

namespace BLL.Ajudas {

    public interface IAjudaCategoriaBL {

        IQueryable<AjudaCategoria> query();

        AjudaCategoria carregar(int id);

        IQueryable<AjudaCategoria> listar(string valorBusca, bool? ativo);

        bool existe(string descricao, int id);

        bool salvar(AjudaCategoria OAjudaCategoria);

        UtilRetorno excluir(int id);

    }
}
