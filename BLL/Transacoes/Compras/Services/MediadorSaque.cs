using System;
using System.Linq;
using BLL.Associados;
using BLL.Pedidos;
using BLL.Services;
using DAL.Associados;
using DAL.Pedidos;
using DAL.Transacoes;

namespace BLL.Transacoes.Compras {

    public class MediadorSaque : MediadorBase {
        
        public override MovimentoOperacaoDTO carregarDados(int idPessoaOrigem, int idPessoaDestino, decimal valorTransacao, int idReferencia){
                
            MovimentoOperacaoDTO OMovimentoOperacaoDTO = base.carregarDados(idPessoaOrigem, idPessoaDestino, valorTransacao, idReferencia);
            
            OMovimentoOperacaoDTO.idTipoTransacao = (byte) TipoTransacaoEnum.SAQUE;
            
            OMovimentoOperacaoDTO.flagIgnorarSaldo = true;            
            
            return OMovimentoOperacaoDTO;

        }                      
        
        
        
    }

}
