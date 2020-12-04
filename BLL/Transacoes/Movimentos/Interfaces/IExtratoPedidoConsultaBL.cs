using System;
using System.Linq;
using DAL.Transacoes;

namespace BLL.Transacoes.Movimentos {

    public interface IExtratoPedidoConsultaBL {

        IQueryable<MovimentoResumoPedido> query(int idPedido, int idPedidoProduto, DateTime? dtInicio, DateTime? dtFim);
        
    }

}
