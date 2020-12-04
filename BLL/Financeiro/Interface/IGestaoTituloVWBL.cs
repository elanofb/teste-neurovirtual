using System;
using System.Linq;
using DAL.Financeiro;

namespace BLL.Financeiro {

	public interface IGestaoTituloVWBL {

        IQueryable<GestaoTituloVW> listar(string valorBusca, string destinatario, string nf, string flagPago, string pesquisarPor, DateTime? dtInicio, DateTime? dtFim);
	}
}
