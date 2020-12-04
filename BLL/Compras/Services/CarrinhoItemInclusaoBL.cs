using System;
using System.Linq;
using BLL.Services;
using DAL.Compras;
using EntityFramework.Extensions;

namespace BLL.Compras {

	public class CarrinhoItemInclusaoBL : DefaultBL, ICarrinhoItemInclusaoBL{

        //Atributos
	    private ICarrinhoItemConsultaBL _CarrinhoItemConsultaBL;

        //Servicos
        private ICarrinhoItemConsultaBL OCarrinhoItemConsultaBL => _CarrinhoItemConsultaBL = _CarrinhoItemConsultaBL ?? new CarrinhoItemConsultaBL();

		/// <summary>
        /// Adicionar um produto ao carrinho de compras
        /// </summary>
		public void adicionar(CarrinhoItem OItemCarrinho) {

			OItemCarrinho.idTipoItem = 0;
			
			this.salvar(OItemCarrinho);
		}

		/// <summary>
        /// Salvar o item adicionado no carrinho, no banco de dados
        /// </summary>
		private bool salvar(CarrinhoItem OCarrinhoItem) {

			var ItemExistente = this.OCarrinhoItemConsultaBL.listar(OCarrinhoItem.idOrganizacao, OCarrinhoItem.idPessoa.toInt(), OCarrinhoItem.idSessao, false)
                                                            .Where(x => x.idProduto == OCarrinhoItem.idProduto)
                                                            .OrderByDescending(x => x.id).FirstOrDefault();

			if (ItemExistente == null) {

				if (OCarrinhoItem.idPessoa == 0) {

					OCarrinhoItem.idPessoa = null;

				}

                OCarrinhoItem.dtInclusao = DateTime.Now;
				
				this.db.CarrinhoItem.Add(OCarrinhoItem);
				
				this.db.SaveChanges();

				return (OCarrinhoItem.id > 0);

			}

			ItemExistente.qtde = (byte)(ItemExistente.qtde + OCarrinhoItem.qtde);

			this.db.CarrinhoItem.Where(x => x.id == ItemExistente.id)
                                .Update(x => new CarrinhoItem { qtde = ItemExistente.qtde});

			return (ItemExistente.id > 0);
		}


	}
}
