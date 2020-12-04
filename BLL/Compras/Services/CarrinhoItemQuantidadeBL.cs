using System.Linq;
using BLL.Services;
using DAL.Compras;
using EntityFramework.Extensions;

namespace BLL.Compras {

    public class CarrinhoItemQuantidadeBL : DefaultBL, ICarrinhoItemQuantidadeBL {

        //Atributos

        //Propriedades

        /// <summary>
        /// Vincular um carrinho existente ao ID da pessoa logada
        /// </summary>
        public void atualizarQuantidade(int idOrganizacaoParam, int id, string idSessao, byte novaQtde) {

            var queryItens = db.CarrinhoItem.Where(x => 
                                                    x.id == id && 
                                                    x.idOrganizacao == idOrganizacaoParam  && 
                                                    x.idSessao == idSessao && 
                                                    x.dtExclusao == null && 
                                                    x.flagComprado == false
                                                );

            queryItens.Update(x => new CarrinhoItem { qtde = novaQtde });

        }
    }
}
