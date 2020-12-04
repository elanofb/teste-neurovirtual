using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Pedidos;
using BLL.Services;
using DAL.Pedidos;

namespace WEB.Areas.Pedidos.ViewModels{

	public class WidgetPedidosPorPeriodoVM {

		//Atributos
		private IPedidoBL _PedidoBL;

		//Propriedades
		private IPedidoBL OPedidoBL => _PedidoBL = _PedidoBL ?? new PedidoBL();

		public List<Pedido> listaPedidos { get; set; }
		public List<int> listStatusProducao { get; set; }
		
		public List<DateTime> listaDatas { get; set; }
		public List<DateTime> listaSemanas { get; set; }
		
		//
		public bool flagSemanal { get; set; }
		
		public DateTime? dtIniPesquisa { get; set; }

		public DateTime? dtFimPesquisa { get; set; }

		public void capturarDatas() {
			this.flagSemanal = UtilRequest.getBool("flagSemanal") ?? false;
			this.dtIniPesquisa = UtilRequest.getDateTime("dataInicio");
			this.dtFimPesquisa = UtilRequest.getDateTime("dataFim");
		}

		public WidgetPedidosPorPeriodoVM() {
			this.listaDatas = new List<DateTime>();
			this.listaSemanas = new List<DateTime>();
		}
		
		public void carregarDias() {

			for (var i = 0; i < 7; i++) {

				//Adiciona semana
				var qtdDias = i * 7;
				var dateIni = DateTime.Now.Date.AddDays(-qtdDias);

				this.listaSemanas.Add(dateIni);
				
				//Adiciona data
				var dateAtual = this.dtIniPesquisa.Value.AddDays(i);
				if (dateAtual > this.dtFimPesquisa) {
					continue;
				}
				
				this.listaDatas.Add(dateAtual);
				
			}

			this.listaDatas = this.listaDatas.OrderByDescending(x => x).ToList();
			this.listaSemanas = this.listaSemanas.OrderByDescending(x => x).ToList();

		}

		//
		public void carregarDados() {

			this.listStatusProducao = new List<int> { StatusPedidoConst.AGUARDANDO_EXPEDICAO, StatusPedidoConst.EM_MONTAGEM, StatusPedidoConst.EM_PREPARACAO, StatusPedidoConst.EM_ANDAMENTO };
			
			var queryPedidos = this.OPedidoBL.listar("","S",0);

			var diferencaDias = (dtFimPesquisa - dtIniPesquisa).Value.Days;

			if (diferencaDias > 7 && this.flagSemanal == false) {
				this.dtFimPesquisa = this.dtIniPesquisa.Value.AddDays(6);
			}

			if (dtIniPesquisa != null) {
				queryPedidos = queryPedidos.Where(x => x.dtCadastro >= dtIniPesquisa);
			}

			if (dtFimPesquisa != null) {
				var dtFiltro = dtFimPesquisa.Value.AddDays(1);
				queryPedidos = queryPedidos.Where(x => x.dtCadastro < dtFiltro);
			}
				
			this.listaPedidos = queryPedidos.Select(x => new {x.id, x.dtCancelamento, x.valorFrete, x.valorProdutos, x.valorDesconto, x.idStatusPedido, x.dtQuitacao, x.dtCadastro, x.ativo}).ToListJsonObject<Pedido>();

		}

	}

}