using System;
using System.Linq;
using EntityFramework.Extensions;
using BLL.Services;
using DAL.Permissao.Security.Extensions;
using DAL.Financeiro;

namespace BLL.Financeiro {

	public class ConciliacaoFinanceiraDetalheExclusaoBL : DefaultBL, IConciliacaoFinanceiraDetalheExclusaoBL {

        //Remover um registro (exclusao lógica - nao remove-se fisicamente)
        public bool excluir(int id) {            

            db.ConciliacaoFinanceiraDetalhe
                .Where(x => x.id == id)
                .Update(x => new ConciliacaoFinanceiraDetalhe { dtExclusao = DateTime.Now, idUsuarioExclusao = User.id()});
            return true;
        }

		public bool excluir(int[] ids) {
			
			db.ConciliacaoFinanceiraDetalhe.Where(x => ids.Contains(x.id) && !x.dtExclusao.HasValue)
				.Update(x => new ConciliacaoFinanceiraDetalhe { dtExclusao = DateTime.Now, idUsuarioExclusao = User.id()});
				//.Delete();
			
			var listaCheck = db.ConciliacaoFinanceiraDetalhe.Where(x => ids.Contains(x.id) && !x.dtExclusao.HasValue).ToList();
			return (listaCheck.Count == 0);

		}
	}
}