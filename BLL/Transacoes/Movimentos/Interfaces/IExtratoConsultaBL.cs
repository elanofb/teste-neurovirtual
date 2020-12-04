using System;
using System.Linq;
using DAL.Transacoes;

namespace BLL.Transacoes.Movimentos {

    public interface IExtratoConsultaBL {

        IQueryable<MovimentoResumoVW> query(int idMembroDestino, DateTime? dtInicio, DateTime? dtFim);
        
    }

}
