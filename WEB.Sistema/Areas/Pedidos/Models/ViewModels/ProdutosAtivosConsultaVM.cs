using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.Pedidos;
using BLL.Services;
using DAL.Pedidos;

namespace WEB.Areas.Pedidos.ViewModels {

    public class ProdutosAtivosConsultaVM {

        //Atributos
        private IPedidoProdutoBL _IPedidoProdutoBL;        
        
        //Propriedades
        private IPedidoProdutoBL OPedidoProdutoBL => _IPedidoProdutoBL = _IPedidoProdutoBL ?? new PedidoProdutoBL();        
        
        public DateTime? dtCompraInicio { get; set; }
        public DateTime? dtCompraFim { get; set; }
        public DateTime? dtFinalizacaoInicio { get; set; }
        public DateTime? dtFinalizacaoFim { get; set; }
        public int? idProduto { get; set; }        
        public int? idPessoa { get; set; }
        public string valorBusca { get; set; }
        
        public List<PedidoProduto> listaItens { get; set; }
        
        //
        public ProdutosAtivosConsultaVM(){
            
            this.listaItens = new List<PedidoProduto>();
            
        }
        
        public void carregar(){
            
            this.capturarParametros();
            this.montarLista();
        }
        
        public void capturarParametros(){
            
            this.dtCompraInicio = UtilRequest.getDateTime("dtCompraInicio");
            this.dtCompraFim = UtilRequest.getDateTime("dtCompraFim");
            this.dtFinalizacaoInicio = UtilRequest.getDateTime("dtFinalizacaoInicio");
            this.dtFinalizacaoFim = UtilRequest.getDateTime("dtFinalizacaoFim");
            this.idProduto = UtilRequest.getInt32("idProduto");
            this.idPessoa = UtilRequest.getInt32("idPessoa");
            this.valorBusca = UtilRequest.getString("valorBusca");                        
            
        }
        
        public void montarLista(){
            
            var query = this.OPedidoProdutoBL.query();
            
            query = query.Where(x => x.Pedido.idStatusPedido == StatusPedidoConst.PAGO && x.dtFimGanhoDiario != null && x.dtFimGanhoDiario >= DateTime.Now);
            
            if (this.dtCompraInicio.HasValue) {
                
                query = query.Where(x => DbFunctions.TruncateTime(x.Pedido.dtCadastro) >= dtCompraInicio);
                
            }
            
            if (this.dtCompraFim.HasValue) {
                
                query = query.Where(x => DbFunctions.TruncateTime(x.Pedido.dtCadastro) <= dtCompraFim);
                
            }
            
            if (this.dtFinalizacaoInicio.HasValue) {
                
                query = query.Where(x => DbFunctions.TruncateTime(x.dtFimGanhoDiario) >= dtFinalizacaoInicio);
                
            }
            
            if (this.dtFinalizacaoFim.HasValue) {
                
                query = query.Where(x => DbFunctions.TruncateTime(x.dtFimGanhoDiario) <= dtFinalizacaoFim);
                
            }
            
            if (this.idProduto.toInt() > 0) {
                query = query.Where(x => x.idProduto == this.idProduto);
            }
            
            if (this.idPessoa.toInt() > 0) {
                query = query.Where(x => x.Pedido.idPessoa == this.idPessoa);
            }
            
            if (!this.valorBusca.isEmpty()){
                
                int valorBuscaInt = this.valorBusca.toInt();
                
                query = query.Where(x => x.idPedido == valorBuscaInt || x.Pedido.Associado.nroAssociado == valorBuscaInt || x.Pedido.Pessoa.nome.Contains(this.valorBusca));
                
            }
            
            this.listaItens = query.Select(x => new {
                    x.id,
                    x.idPedido,
                    x.idProduto,
                    x.qtdeDiasDuracao,
                    x.dtFimGanhoDiario,
                    x.valorGanhoDiario,
                    x.qtdePontosPlanoCarreira,
                    x.qtde,
                    x.valorItem,
                    Produto = new{
                        x.Produto.nome    
                    },
                    Pedido = new{
                        x.Pedido.dtCadastro,
                        Pessoa = new{
                            x.Pedido.Pessoa.nome
                        },
                        Associado = new{
                            x.Pedido.Associado.nroAssociado
                        }
                    }                
                }).ToListJsonObject<PedidoProduto>();            

        }                     
        
    }
    
}