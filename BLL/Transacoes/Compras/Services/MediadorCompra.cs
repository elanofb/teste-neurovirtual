using System;
using System.Linq;
using BLL.Associados;
using BLL.Pedidos;
using BLL.Services;
using DAL.Associados;
using DAL.Pedidos;
using DAL.Transacoes;

namespace BLL.Transacoes.Compras {

    public class MediadorCompra : MediadorBase{
        
        //Atributos        
        private IPedidoProdutoBL _PedidoProdutoBL; 
        
        //Propriedades        
        private IPedidoProdutoBL PedidoProdutoBL => _PedidoProdutoBL = _PedidoProdutoBL ?? new PedidoProdutoBL();

        public override MovimentoOperacaoDTO carregarDados(int idPessoaOrigem, int idPessoaDestino, decimal valorTransacao, int idReferencia){
            
            MovimentoOperacaoDTO OMovimentoOperacaoDTO = base.carregarDados(idPessoaOrigem, 1, valorTransacao, idReferencia);
                
            OMovimentoOperacaoDTO.idProduto = this.carregarItemPedido(idReferencia).idProduto;
            
            return OMovimentoOperacaoDTO;

        }
        
        public override Associado carregarMembroDestino(int idPessoaDestino) {
            
            //Conta Fixa da Linkey
            var MembroLinkey = Queryable.Where<Associado>(this.AssociadoConsultaBL.queryNoFilter(1), x => x.nroAssociado == 1)
                                   .Select(x => new {
                                       x.id, 
                                       x.nroAssociado,
                                       x.idPessoa,
                                       Pessoa = new {
                                           x.Pessoa.id,
                                           x.Pessoa.nome,
                                           x.Pessoa.nroDocumento
                                       }
                                   }).FirstOrDefault()
                                   .ToJsonObject<Associado>() ?? new Associado();

            return MembroLinkey;
            
        }
        
        /// <summary>
        /// 
        /// </summary>
        public PedidoProduto carregarItemPedido(int idPedido) {
            
            var ItemPedido = this.PedidoProdutoBL.query()
                                 .Where(x => x.idPedido == idPedido)
                                 .Select(x => new {x.id, x.idProduto, x.idPedido})
                                 .FirstOrDefault()
                                 .ToJsonObject<PedidoProduto>();

            return ItemPedido;
        }
        
    }

}
