using System;
using BLL.Transacoes.Movimentos;
using DAL.Associados;
using DAL.Transacoes;
using DAL.Transacoes.Extensions;

namespace BLL.Transacoes.Compras {

    public class ValidadorCompra : ValidadorBase{
        
        

        /// <summary>
        /// 
        /// </summary>
        public override UtilRetorno validar(MovimentoOperacaoDTO Transacao) {

            var ValidacaoContas = this.validarContas(Transacao);

            if (ValidacaoContas.flagError) {
                
                return ValidacaoContas;
            }
            

            if (Transacao.MembroDestino.nroAssociado != 1) {

                return UtilRetorno.newInstance(true, "As compras devem ser feitas junto ao usuário SINCTEC.");
                
            }

            var MovimentoResumo = new MovimentoResumoVW();

            MovimentoResumo.idTipoTransacao = (byte)TipoTransacaoEnum.PRODUTOS_LINKEY;
            
            MovimentoResumo.descricaoTipoTransacao = "Pagamento Produtos SINCTEC";

            MovimentoResumo.captarDados(Transacao);
            
            return UtilRetorno.newInstance(false, "", MovimentoResumo);
        }
    }

}
