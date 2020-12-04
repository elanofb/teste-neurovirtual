using System;
using System.Linq;
using DAL.Cargos;
using DAL.ConfiguracoesTextos;

namespace BLL.ConfiguracoesTextos {

    public interface IConfiguracaoTextoCategoriaBL {
        IQueryable<ConfiguracaoTextoCategoria> listar(string valorBusca);
    }
}
