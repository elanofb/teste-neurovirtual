using BLL.Core.Events;
using DAL.Pedidos;
using BLL.Financeiro;
using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Pedidos.Emails;
using BLL.Services;
using DAL.Financeiro;

namespace BLL.Pedidos {

	public class OnPedidoFaturadoHandler : IHandler<object> {

        //Atritos
        private ITituloReceitaBL _TituloReceitaBL;

        //Propriedades
        private ITituloReceitaGeradorBL OTituloReceitaGeradorBL => new TituloReceitaGeradorPedidoBL();
        private ITituloReceitaBL OTituloReceitaBL => _TituloReceitaBL = _TituloReceitaBL ?? new TituloReceitaPedidoBL();
        private IPedidoHistoricoBL OPedidoOcorrenciaBL => new PedidoHistoricoBL();

		//
		public void execute(object source) {

            try { 

                Pedido OPedido = (source as Pedido);

                this.gerarTitulo(OPedido);

                this.registrarOcorrencia(OPedido);

                OPedido.TituloReceita = this.OTituloReceitaBL.query()
                                                         .Where(x => x.idReceita == OPedido.id && x.dtExclusao == null)
                                                         .Select(x => new { x.id, x.descricao })
                                                         .FirstOrDefault()
                                                         .ToJsonObject<TituloReceita>() ?? new TituloReceita();

                if (OPedido.dtVencimento.HasValue && OPedido.dtCadastro.Date == OPedido.dtVencimento.Value.Date){
                    this.enviarEmailNovoPedido(OPedido);
                }

                if (OPedido.dtVencimento.HasValue && OPedido.dtVencimento.Value.Date > OPedido.dtCadastro.Date) {
                    this.enviarEmailFaturamentoPedido(OPedido);
                }


            } catch (Exception ex) {

                UtilLog.saveError(ex, "Erro ao executar o evento OnPedidoFaturadoHandler.");

            }

		}


        // Gerar título financeiro
	    private void gerarTitulo(Pedido OPedido) {

	        try {

    			this.OTituloReceitaGeradorBL.gerar(OPedido);

	        } catch (Exception ex) {

	            UtilLog.saveError(ex, $"Erro ao gerar o título financeiro do pedido {OPedido.id}");

	        }
	        
	    }

        //
        private void registrarOcorrencia(Pedido OPedido) {

            this.OPedidoOcorrenciaBL.criarOcorrenciaFaturamentoPedido(OPedido.id);

        }

        //enviar e-mail para o associados após a criaçãod do pedido
        private void enviarEmailNovoPedido(Pedido OPedido) {

            try {

                IEnvioNovoPedido EnvioEmail = EnvioNovoPedido.factory(OPedido.idOrganizacao.toInt(), new List<string> { OPedido.email }, null);

                EnvioEmail.enviar(OPedido);

            } catch (Exception ex) {
                UtilLog.saveError(ex, $"Erro ao enviar e-mail do pedido {OPedido.id}");
            }

        }

        //enviar e-mail para o associados após pedido faturado
        private void enviarEmailFaturamentoPedido(Pedido OPedido) {

            try {

                IEnvioFaturamentoPedido EnvioEmail = EnvioFaturamentoPedido.factory(OPedido.idOrganizacao.toInt(), new List<string> { OPedido.email }, null);

                EnvioEmail.enviar(OPedido);

            } catch (Exception ex) {
                UtilLog.saveError(ex, $"Erro ao enviar e-mail do pedido de faturamento {OPedido.id}");
            }

        }

    }

}
