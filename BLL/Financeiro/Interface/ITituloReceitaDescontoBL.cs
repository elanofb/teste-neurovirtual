using System;
using DAL.Financeiro;

namespace BLL.Financeiro {

	public interface ITituloReceitaDescontoBL {
		
		//
        TituloReceita salvarCupomDesconto(int idTituloReceita, int idCupomDesconto);

    }
}
