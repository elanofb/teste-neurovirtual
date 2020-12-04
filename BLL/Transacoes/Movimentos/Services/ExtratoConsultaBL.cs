using System;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Transacoes;

namespace BLL.Transacoes.Movimentos {

    public class ExtratoConsultaBL : DefaultBL, IExtratoConsultaBL {

        /// <summary>
        /// 
        /// </summary>
        public IQueryable<MovimentoResumoVW> query(int idMembroDestino, DateTime? dtInicio, DateTime? dtFim) {
            
            var query = from Mov in db.MovimentoResumoVW select Mov;
            
            if (idMembroDestino > 0) {

                query = query.Where(x => x.idMembroDestino == idMembroDestino);
            }
            
            if (dtInicio.HasValue) {
                
                query = query.Where(x => DbFunctions.TruncateTime(x.dtCadastro) >=  dtInicio);
                
            }
            
            if (dtFim.HasValue) {
                
                query = query.Where(x => DbFunctions.TruncateTime(x.dtCadastro) <=  dtFim);
                
            }
            
            return query;
        }
    }

}
