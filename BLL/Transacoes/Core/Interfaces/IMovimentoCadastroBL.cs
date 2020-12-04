using System;
using System.Linq.Expressions;
using DAL.Transacoes;

namespace BLL.Transacoes {

	public interface IMovimentoCadastroBL {
		
		void atualizarSincronizacao(int[] idMembros, DateTime dtSincronizacao);
	}
}