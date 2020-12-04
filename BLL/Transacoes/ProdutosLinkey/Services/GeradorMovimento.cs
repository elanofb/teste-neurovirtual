using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Services;
using DAL.Pedidos;
using DAL.Transacoes;
using EntityFramework.Extensions;

namespace BLL.Transacoes.ProdutosLinkey {

    public class GeradorMovimento: DefaultBL, IGeradorMovimento{
        
        /// <summary>
        /// 
        /// </summary>
        public UtilRetorno transferir(List<PedidoProdutoRendimento> listaProdutos) {

            var listaMovimentos = new List<Movimento>();

            foreach (var Item in listaProdutos) {

                var MovimentoOrigem = new Movimento();
                MovimentoOrigem.idMembroDestino = Item.idMembro;
                MovimentoOrigem.idPessoaDestino = Item.idPessoa;
                MovimentoOrigem.idMembroOrigem = 1;
                MovimentoOrigem.valor = Item.valorGanhoDiario.toDecimal();
                MovimentoOrigem.flagDebito = false;
                MovimentoOrigem.flagCredito = true;
                MovimentoOrigem.idTipoTransacao = (byte) TipoTransacaoEnum.GANHO_PLANOS;
                MovimentoOrigem.idPedidoProduto = Item.id;
                MovimentoOrigem.setDefaultInsertValues();
                
                listaMovimentos.Add(MovimentoOrigem);
            }

            db.validateAndSave();
            db.Movimento.AddRange(listaMovimentos);
            
            var Retorno = db.validateAndSave();

            if (Retorno.flagError) {
                
                return Retorno;
            }

            var ids = listaMovimentos.Select(x => x.idPedidoProduto.toInt()).ToList();

            var dtOperacao = DateTime.Now;

            db.PedidoProduto.Where(x => ids.Contains(x.id)).Update(x => new PedidoProduto {dtUltimoPagamento = dtOperacao});

            return UtilRetorno.newInstance(false, "", listaMovimentos);
        }
        
    }

}
