﻿using System;
using System.Collections.Generic;
using System.Linq;
using DAL.Pedidos;
using MoreLinq;
using PagedList;

namespace WEB.Areas.Pedidos.ViewModels {

    public class PedidoProducaoVM {

        public List<Pedido> listaTodosPedidos { get; set; }
        public IPagedList<Pedido> listaPedidos { get; set; }
        public List<PedidoProduto> listaProdutos { get; set; }
        public List<ItemDemandaPedidoDTO> listaResumoProdutos { get; set; }
        public List<int> idsPedidos { get; set; }

        public int totalPedidos { get; set; }
        public int totalPedidosAtrasados { get; set; }
        public int totalPedidosHoje { get; set; }
        public int totalPedidosAmanha { get; set; }
        public int totalPedidosSemana { get; set; }

        public string idBoxLista { get; set; }

        //
        public PedidoProducaoVM() {
            this.listaPedidos = new List<Pedido>().ToPagedList(1, 20);
            this.listaProdutos = new List<PedidoProduto>();
            this.listaResumoProdutos = new List<ItemDemandaPedidoDTO>();
            this.idsPedidos = new List<int>();
        }
        
        public void carregarTotais() {

            this.totalPedidos = this.listaTodosPedidos.Count;
            this.totalPedidosAtrasados = this.listaTodosPedidos.Count(x => x.listaPedidoEntrega.Any(e => e.dtAgendamentoEntrega < DateTime.Now.Date) && x.dtExpedicao == null);
            this.totalPedidosHoje = this.listaTodosPedidos.Count(x => x.listaPedidoEntrega.Any(e => e.dtAgendamentoEntrega == DateTime.Now.Date));
            this.totalPedidosAmanha = this.listaTodosPedidos.Count(x => x.listaPedidoEntrega.Any(e => e.dtAgendamentoEntrega == DateTime.Now.AddDays(1).Date));
            this.totalPedidosSemana = this.listaTodosPedidos.Count(x => x.listaPedidoEntrega.Any(e => e.dtAgendamentoEntrega == DateTime.Now.AddDays(5).Date));

            this.listaResumoProdutos = this.listaProdutos.Select(x => new ItemDemandaPedidoDTO {
                                           id = x.idProduto,
                                           nome = x.nomeProduto,
                                           observacoes = x.observacoes,
                                           qtd = this.listaProdutos.Where(l => x.idProduto == l.idProduto && l.observacoes == x.observacoes).Sum(s => s.qtde.toInt())
                                       }).DistinctBy(x => new { x.nome, x.observacoes }).OrderByDescending(x => x.qtd).ToList();
            
        }
        
    }
    
}