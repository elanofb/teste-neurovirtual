using System;
using System.Linq;
using System.Threading.Tasks;
using DAL.Pedidos;

namespace BLL.Compras {

	public interface ICarrinhoBL {

		UtilRetorno alterarQuantidade(int idItem, byte qtde);

	}
}
