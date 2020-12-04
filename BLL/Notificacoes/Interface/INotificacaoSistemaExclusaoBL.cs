using System;

namespace BLL.Notificacoes
{
    public interface INotificacaoSistemaExclusaoBL
    {
        /// <summary>
        /// Excluir um registro pelo ID
        /// </summary>
        UtilRetorno excluir(int id);
    }
}