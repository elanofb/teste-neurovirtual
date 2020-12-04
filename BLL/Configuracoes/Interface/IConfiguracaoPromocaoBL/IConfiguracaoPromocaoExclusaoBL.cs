using System;
using System.Json;
using System.Linq;

using DAL.Configuracoes;

namespace BLL.Configuracoes.Interface.IConfiguracaoPromocaoBL {

    public interface IConfiguracaoPromocaoExclusaoBL {
        UtilRetorno       excluir(int       id);
        JsonMessageStatus alterarStatus(int id);
    }

}