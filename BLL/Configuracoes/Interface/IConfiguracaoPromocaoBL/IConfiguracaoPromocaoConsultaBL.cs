using System;
using System.Linq;

using DAL.Configuracoes;

namespace BLL.Configuracoes.Interface.IConfiguracaoPromocaoBL {

    public interface IConfiguracaoPromocaoConsultaBL {
        IQueryable<ConfiguracaoPromocao> query(bool history = false);

        IQueryable<ConfiguracaoPromocao> listar(string valorBusca = "", bool history = false, DateTime? dtInicioPremio = null, DateTime? dtFimPremio = null);

        ConfiguracaoPromocao carregar(int id = 0);
    }

}