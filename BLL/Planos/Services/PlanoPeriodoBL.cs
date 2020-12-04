using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityFramework.Extensions;
using DAL.Planos;
using DAL.Repository.Base;

namespace BLL.Planos{

    public class PlanoPeriodoBL: TableRepository<PlanoPeriodo>{

        //
        public PlanoPeriodoBL(): base(null){         
            this.defaultPredicate = x => x.flagExcluido == "N";
        }

        /**
         * 
         */
        public PlanoPeriodo carregar(int id) {

            var db = this.getDataContext();
            var query = (from PlanoPeriodo in db.PlanoPeriodo
                         where PlanoPeriodo.id == id
                         && PlanoPeriodo.flagExcluido == "N"
                         select PlanoPeriodo).FirstOrDefault();

            return query;
        }

        /**
         * 
         */        
        public IQueryable<PlanoPeriodo> listar(string valorBusca, string ativo) {
            var db = this.getDataContext();
            var query = from F in db.PlanoPeriodo
                        where F.flagExcluido == "N"
                        select F;

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.descricao.Contains(valorBusca));
            }

            if (!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

        /**
         * 
         */

        public bool salvar(PlanoPeriodo OPlanoPeriodo) {

            this.save(OPlanoPeriodo, true);
            return (OPlanoPeriodo.id > 0);
        }

        /**
         * 
         */
        public bool existe(string descricao, int id) { 

			var db = this.getDataContext();
            var query = (from P in db.PlanoPeriodo
                        where P.descricao == descricao && P.id != id && P.flagExcluido == "N"
                        select P).Take(1).FirstOrDefault();

            return (query == null ? false : true);
		}

        /**
         * 
         */
        public bool excluir(int[] ids) {

            this.getDataContext().PlanoPeriodo.Where(x => ids.Contains(x.id))
                .Update(x => new PlanoPeriodo { flagExcluido = "S", dtAlteracao = DateTime.Now });

            var listaCheck = this.getDataContext().PlanoPeriodo
                .Where(x => ids.Contains(x.id) && x.flagExcluido == "N").ToList();
            return (listaCheck.Count == 0);
        }
    }
}
