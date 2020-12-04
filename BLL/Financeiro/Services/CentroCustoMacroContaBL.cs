using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Services;
using DAL.Financeiro;
using EntityFramework.Extensions;

namespace BLL.Financeiro {

	public class CentroCustoMacroContaBL : DefaultBL, ICentroCustoMacroContaBL {

        //
        public IQueryable<CentroCustoMacroConta> listar(int idMacroConta, int idCentroCusto) {

            var query = from Ass in db.CentroCustoMacroConta.AsNoTracking()
                        where 
                            !Ass.dtExclusao.HasValue
                        select Ass;

            if (idMacroConta > 0) {
                query = query.Where(x => x.idMacroConta == idMacroConta);
            }

            if (idCentroCusto > 0) {
                query = query.Where(x => x.idCentroCusto == idCentroCusto);
            }

            return query;
        }

        //
        public void salvar(int idMacroConta, int idCentroCusto, List<CentroCustoMacroConta> listaCentroCusto) {

            if (idMacroConta > 0) {
                this.db.CentroCustoMacroConta.Where(x => x.idMacroConta == idMacroConta)
                    .Update(x => new CentroCustoMacroConta {
                        dtExclusao = DateTime.Now
                    });
            }

            if (idCentroCusto > 0) {
                this.db.CentroCustoMacroConta.Where(x => x.idCentroCusto == idCentroCusto)
                    .Update(x => new CentroCustoMacroConta {
                        dtExclusao = DateTime.Now
                    });
            }

	        foreach (var OCentroCustoMacroConta in listaCentroCusto) {
                OCentroCustoMacroConta.setDefaultInsertValues();
                db.CentroCustoMacroConta.Add(OCentroCustoMacroConta);
                db.SaveChanges();
	        }
	    }
	}
}