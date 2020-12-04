using System;

namespace BLL.ConfiguracoesAssociados {

    public interface IConfiguracaoAssociadoCampoExclusaoBL {
        
        /// <summary>
        /// Excluir um registro
        /// </summary>
        UtilRetorno excluir(int id);
        
    }
}