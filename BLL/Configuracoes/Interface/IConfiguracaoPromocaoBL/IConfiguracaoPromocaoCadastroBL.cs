using System;
using System.Linq;

using DAL.Configuracoes;

namespace BLL.Configuracoes.Interface.IConfiguracaoPromocaoBL {

    public interface IConfiguracaoPromocaoCadastroBL {
        UtilRetorno salvar(ConfiguracaoPromocao newObj);
    }

}