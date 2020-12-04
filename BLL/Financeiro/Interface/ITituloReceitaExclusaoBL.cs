using System;

namespace BLL.Financeiro
{
    public interface ITituloReceitaExclusaoBL
    {
        /// <summary>
        /// Realizar a exclusao pelo ID do registro
        /// </summary>
        UtilRetorno excluir(int idTituloReceita, string motivo = "");

        /// <summary>
        /// Realizar a exclusao pelo tipo de receita e pela referencia
        /// </summary>
        UtilRetorno excluir(int idTipoReceita, int idReferencia, string motivo);
    }
}