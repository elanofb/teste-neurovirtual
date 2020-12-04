using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using BLL.ConfiguracoesEtiquetas;
using BLL.Pedidos;
using BLL.Services;
using DAL.ConfiguracoesEtiquetas;
using DAL.Pedidos;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.Etiquetas.ViewModels {
    
    public class EtiquetaProdutoVM {

        //Atributos Serviços
        private IPedidoProdutoBL _IPedidoProdutoBL;
        
        //Propriedades Serviços
        private IPedidoProdutoBL OPedidoProdutoBL => _IPedidoProdutoBL = _IPedidoProdutoBL ?? new PedidoProdutoBL();
        
        // Propriedades
        public DateTime? dtPrazoInicio { get; set; }
        
        public DateTime? dtPrazoFim { get; set; }

        public List<int> idsStatusPedido { get; set; }
        
        public string valorBusca { get; set; }

        public int? idProduto { get; set; }

        public List<PedidoProduto> listaProdutos { get; set; }
        
        public ConfiguracaoEtiqueta OConfiguracaoEtiqueta { get; set; }

        public List<string> listaEtiquetas { get; set; }

        // Constants
        private IPrincipal User => HttpContextFactory.Current.User;

        //
        public EtiquetaProdutoVM() {
            
            this.listaProdutos = new List<PedidoProduto>();
            
            this.listaEtiquetas = new List<string>();
            
            this.OConfiguracaoEtiqueta = new ConfiguracaoEtiqueta();
            
        }
        
        //
        public void carregarInformacoes() {

            var query = this.montarQuery();

            this.listaProdutos = query.Select(x => new {
                                           x.idProduto, x.nomeProduto, x.observacoes,
                                           Produto = new { TipoProduto = new { x.Produto.TipoProduto.descricao } },
                                           Pedido = new { x.Pedido.dtPreparacao }
                                       }).ToListJsonObject<PedidoProduto>();

            if (!this.listaProdutos.Any()) {
                return;
            }
            
            this.carregarEtiquetas();
            
        }

        //
        private IQueryable<PedidoProduto> montarQuery() {

            var idOrganizacao = User.idOrganizacao();
            
            var query = this.OPedidoProdutoBL.listar(0).Where(x => x.Pedido.idOrganizacao == idOrganizacao);

            query = query.Where(x => x.Pedido.idStatusPedido == StatusPedidoConst.PAGO ||
                                    (x.Pedido.idStatusPedido == StatusPedidoConst.EM_ABERTO && x.Pedido.flagPagamentoNaEntrega == true) ||
                                    (x.Pedido.idStatusPedido == StatusPedidoConst.AGUARDANDO_PAGAMENTO && x.Pedido.flagPagamentoNaEntrega == true));

            if (this.idProduto > 0) {
                query = query.Where(x => x.idProduto == this.idProduto);
            }

            if (!valorBusca.isEmpty()) {
                
                var idPedido = valorBusca.toInt();
                
                query = query.Where(x => x.Pedido.nomePessoa.Contains(valorBusca) || x.Pedido.id == idPedido);
            }

            if (this.dtPrazoInicio.HasValue) {
                query = query.Where(x => x.Pedido.listaPedidoEntrega.Any(c => c.dtAgendamentoEntrega >= this.dtPrazoInicio));
            }

            if (this.dtPrazoFim.HasValue) {
                var dtFiltro = this.dtPrazoFim.Value.AddDays(1);
                query = query.Where(x => x.Pedido.listaPedidoEntrega.Any(c => c.dtAgendamentoEntrega < dtFiltro));
            }

            if (this.idsStatusPedido?.Any() == true) {
                query = query.Where(x => this.idsStatusPedido.Contains(x.Pedido.idStatusPedido));
            }

            return query;
            
        }
        
        //    
        private void carregarEtiquetas() {
            
            var idConfiguracaoEtiqueta = UtilRequest.getInt32("idConfiguracaoEtiqueta");
            
            if (idConfiguracaoEtiqueta > 0){
                
                this.OConfiguracaoEtiqueta = ConfiguracaoEtiquetaBL.getInstance.listarFromCache(User.idOrganizacao()).FirstOrDefault(x => x.id == idConfiguracaoEtiqueta);
            } else {
                
                this.OConfiguracaoEtiqueta = ConfiguracaoEtiquetaBL.getInstance.listarFromCache(User.idOrganizacao()).FirstOrDefault();    
            }
            
            if (this.OConfiguracaoEtiqueta == null) {
                return;
            }
            
            foreach (var OItemPedido in this.listaProdutos) {
                
                var corpoEtiqueta = this.OConfiguracaoEtiqueta.html;

                corpoEtiqueta = corpoEtiqueta.Replace("#ID_PRODUTO#", OItemPedido.idProduto.ToString());
                
                corpoEtiqueta = corpoEtiqueta.Replace("#TIPO_PRODUTO#", OItemPedido.Produto?.TipoProduto?.descricao ?? "Não Definido");
                
                corpoEtiqueta = corpoEtiqueta.Replace("#NOME_PRODUTO#", OItemPedido.nomeProduto);
                
                corpoEtiqueta = corpoEtiqueta.Replace("#DT_PRODUCAO#", OItemPedido.Pedido.dtPreparacao.exibirData(false, "Não Produzido"));
                
                corpoEtiqueta = corpoEtiqueta.Replace("#OBSERVACOES#", OItemPedido.observacoes);
                
                this.listaEtiquetas.Add(corpoEtiqueta);

            }
            
        }

    }
    
}