using System;
using System.Collections.Generic;
using BLL.Services;
using DAL.Transacoes;
using DAL.Transacoes.Extensions;

namespace BLL.Transacoes.Debitos {

    public class GeradorMovimentoDebito: DefaultBL, IGeradorMovimentoDebito {
        
        /// <summary>
        /// 
        /// </summary>
        public UtilRetorno debitar(MovimentoResumoVW MovimentoResumo) {
            
            var MovimentoDestino = new Movimento();
            MovimentoDestino.captarDadosDestinoDebito(MovimentoResumo);
            MovimentoDestino.idTipoTransacao = MovimentoResumo.idTipoTransacao;
            
            MovimentoDestino.setDefaultInsertValues();
            
            var listaItens = new List<Movimento>();
            listaItens.Add(MovimentoDestino);
            
            db.Movimento.AddRange(listaItens);
            
            var Retorno = db.validateAndSave();

            if (Retorno.flagError) {
                
                return Retorno;
            }

            return UtilRetorno.newInstance(false, "", listaItens);
        }
        
    }

}
