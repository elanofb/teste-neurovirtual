using System;

namespace BLL.Compras{

    public interface ICarrinhoItemExclusaoBL{

        /// <summary>
        /// Remover um item do carrinho a partir do ID
        /// </summary>
        UtilRetorno excluir(int idOrganizacaoParam, string idSessao, int id);

        /// <summary>
        /// Limpar o carrinho de compras do usuário
        /// </summary>
        void excluirTudo(int idOrganizacaoParam, int? idPessoa, string idSessao);
    }
}