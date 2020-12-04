using DAL.Compras;

namespace BLL.Compras {

    public interface ICarrinhoItemInclusaoBL {

        /// <summary>
        /// Adicionar um produto ao carrinho de compras
        /// </summary>
        void adicionar(CarrinhoItem OItemCarrinho);
    }
}