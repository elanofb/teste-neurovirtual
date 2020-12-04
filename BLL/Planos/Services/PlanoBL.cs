using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using EntityFramework.Extensions;
using DAL.Planos;
using DAL.Repository.Base;
using System.Data.Entity;

namespace BLL.Planos{

    public class PlanoBL: TableRepository<Plano>, IPlanoBL {

        //
        public PlanoBL(): base(null){         
            this.defaultPredicate = x => x.flagExcluido == "N";
        }

        //
        public Plano carregar(int id) {

            var db = this.getDataContext();
            var query = (from plano in db.Plano
                         where plano.id == id
                         && plano.flagExcluido == "N"
                         select plano).FirstOrDefault();

            return query;
        }

        //        
        public IQueryable<Plano> listar(string valorBusca, string ativo) {

            var db = this.getDataContext();
            var query = from plano in db.Plano
                        where plano.flagExcluido == "N"
                        select plano;

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.nome.Contains(valorBusca));
            }

            if (!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

        //
        public bool salvar(Plano OPlano) {

            this.save(OPlano, true);
            return (OPlano.id > 0);
        }

        //
        public bool existe(string descricao, int id) { 

			var db = this.getDataContext();
            var query = (from P in db.Plano
                        where P.descricao == descricao && P.id != id && P.flagExcluido == "N"
                        select P).Take(1).FirstOrDefault();

            return (query == null ? false : true);
		}

        //
        public bool excluir(int[] ids) {

            this.getDataContext().Plano.Where(x => ids.Contains(x.id))
                .Update(x => new Plano { flagExcluido = "S", dtAlteracao = DateTime.Now });

            var listaCheck = this.getDataContext().Plano
                .Where(x => ids.Contains(x.id) && x.flagExcluido == "N").ToList();
            return (listaCheck.Count == 0);
        }
    }
}
