using System;
using System.Linq;
using System.Web.Mvc;
using BLL.Financeiro;
using BLL.Pedidos;
using BLL.Services;
using DAL.Financeiro;
using DAL.Pedidos;
using MvcFlashMessages;
using WEB.Areas.Pedidos.ViewModels;

namespace WEB.Areas.Pedidos.Controllers {

    public class PedidoDetalhesController : Controller {
        
        //Atributos
        private IPedidoBL _IPedidoBL;
        private IPedidoEntregaBL _PedidoEntregaBL;
        private ITituloReceitaBL _TituloReceitaBL;

        //Propriedades
        private IPedidoBL OPedidoBL => _IPedidoBL = _IPedidoBL ?? new PedidoBL();
        private IPedidoEntregaBL OPedidoEntregaBL => _PedidoEntregaBL = _PedidoEntregaBL ?? new PedidoEntregaBL();
        private ITituloReceitaBL OTituloReceitaBL => _TituloReceitaBL = _TituloReceitaBL ?? new TituloReceitaPedidoBL();

        [HttpGet]
        public ActionResult index(int? id) {

            int idPedido = id.toInt();
            
            var ViewModel = new PedidoDetalhesVM();
            
            ViewModel.Pedido = this.OPedidoBL.query(1)
                                   .Where(x => x.id == idPedido)
                                   .Select(
                                       x => new {
                                           x.id,
                                           x.idAssociado,
                                           x.idNaoAssociado,
                                           x.idPessoa,
                                           x.ativo,
                                           x.dtCadastro,
                                           x.dtCancelamento,
                                           x.dtQuitacao,
                                           x.dtFinalizado,
                                           x.dtFaturamento,
                                           x.dtExpedicao,
                                           x.dtAtendimento,
                                           x.dtVencimento,
                                           x.nomePessoa,
                                           x.cpf,
                                           x.email,
                                           x.telPrincipal,
                                           x.telSecundario,
                                           x.valorDesconto,
                                           x.valorDescontoCupom,
                                           x.valorFrete,
                                           x.valorProdutos,
                                           x.idStatusPedido,
                                           MeioPagamento = new {
                                                id = x.MeioPagamento == null? 0: x.MeioPagamento.id,
                                                x.MeioPagamento.descricao
                                           },
                                           StatusPedido = new {
                                                x.StatusPedido.id,
                                                x.StatusPedido.descricao
                                           },
                                           Pessoa = new {
                                                x.Pessoa.id,
                                                x.Pessoa.nome,
                                                x.Pessoa.nroDocumento
                                           },
                                           Associado = new {
                                                id = x.Associado == null? 0: x.Associado.id,
                                                x.Associado.nroAssociado
                                           },
                                           NaoAssociado = new {
                                                id = x.NaoAssociado == null? 0: x.NaoAssociado.id,
                                                x.NaoAssociado.nroAssociado
                                           }
                                       }
                                   )
                                   .FirstOrDefault()
                                   .ToJsonObject<Pedido>();
            
            if (ViewModel.Pedido == null) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "O pedido informado não foi encontrado."));

                return RedirectToAction("index", "PedidoCadastro");
            }

            ViewModel.PedidoEntrega = this.OPedidoEntregaBL.listar()
                                          .Where(x => x.flagExcluido == "N" && x.idPedido == ViewModel.Pedido.id)
                                          .Select(
                                              x => new {
                                                  x.id,
                                                  x.idPedido,
                                                  x.logradouro,
                                                  x.numero,
                                                  x.complemento,
                                                  x.bairro,
                                                  x.idCidade,
                                                  x.nomeCidade,
                                                  x.idEstado,
                                                  x.idTipoEndereco,
                                                  x.dtAgendamentoEntrega
                                              }
                                          )
                                          .FirstOrDefault()
                                          .ToJsonObject<PedidoEntrega>();

            ViewModel.Pedido.TituloReceita = this.OTituloReceitaBL.query().Where(x => x.idReceita == ViewModel.Pedido.id && x.dtExclusao == null)
                                                                  .Select(
                                                                        x => new {
                                                                             x.id, 
                                                                             x.idTipoReceita, 
                                                                             x.descricao,
                                                                             x.idReceita,
                                                                             x.dtExclusao,
                                                                             listaTituloReceitaPagamento = x.listaTituloReceitaPagamento
                                                                                                            .Where(y => y.dtExclusao == null)
                                                                                                            .Select(
                                                                                                                y => new {
                                                                                                                   y.id
                                                                                                                }
                                                                                                            )
                                                                        }
                                                                  )
                                                                  .FirstOrDefault()
                                                                  .ToJsonObject<TituloReceita>(true) ?? new TituloReceita();

            return View(ViewModel);
        }
    }
}