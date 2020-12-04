using System.Collections.Generic;
using System.Linq;
using DAL.Produtos;

namespace BLL.Produtos
{
    public interface IProdutoTipoAssociadoBL
    {
        /// <summary>
        /// Montagem de query para consulta
        /// </summary>
        IQueryable<ProdutoTipoAssociado> query(int idProduto);

        /// <summary>
        /// Carregamento a partir do ID 
        /// </summary>
        ProdutoTipoAssociado carregar(int id);

        bool salvar(int idProduto, List<int> idsTipoAssociado);
    }
}