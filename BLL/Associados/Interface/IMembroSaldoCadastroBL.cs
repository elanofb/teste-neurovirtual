using System;
using System.Linq.Expressions;
using DAL.Associados;

namespace BLL.Membros {

	public interface IMembroSaldoCadastroBL {
		
        bool salvar(MembroSaldo OMembroSaldo);
		
		void atualizar(int[] ids, Expression<Func<MembroSaldo, MembroSaldo>> updateExpression);

		void atualizarOuInserir(int[] ids, Expression<Func<MembroSaldo, MembroSaldo>> updateExpression);
	}
}