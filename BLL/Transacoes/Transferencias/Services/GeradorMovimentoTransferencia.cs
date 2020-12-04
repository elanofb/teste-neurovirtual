using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Services;
using DAL.Transacoes;
using DAL.Transacoes.Extensions;
using EntityFramework.Extensions;

namespace BLL.Transacoes.Transferencias {

    public class GeradorMovimentoTransferencia: DefaultBL, IGeradorMovimentoTransferencia {
        
        

        
        /// <summary>
        /// 
        /// </summary>
        public UtilRetorno transferir(MovimentoResumoVW MovimentoResumo) {

            var MovimentoOrigem = new Movimento();
            MovimentoOrigem.captarDadosOrigem(MovimentoResumo);
            MovimentoOrigem.idTipoTransacao = (byte) TipoTransacaoEnum.TRANSFERÊNCIA;
            

            var MovimentoDestino = new Movimento();
            MovimentoDestino.captarDadosDestino(MovimentoResumo);
            MovimentoDestino.idTipoTransacao = (byte) TipoTransacaoEnum.TRANSFERÊNCIA;

            MovimentoOrigem.setDefaultInsertValues();
            MovimentoDestino.setDefaultInsertValues();

            var listaItens = new List<Movimento>();
            listaItens.Add(MovimentoOrigem);
            listaItens.Add(MovimentoDestino);

            db.Movimento.AddRange(listaItens);
            
            var Retorno = db.validateAndSave();

            if (Retorno.flagError) {
                
                return Retorno;
            }

            db.Movimento.Where(x => x.id == MovimentoDestino.id).Update(x => new Movimento {idMovimentoPrincipal = MovimentoOrigem.id});

            return UtilRetorno.newInstance(false, "", listaItens);
        }
        
    }

}
