using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using BLL.ConfiguracoesEcommerce;
using BLL.Pedidos;
using BLL.PedidosTemp;
using DAL.Enderecos;
using DAL.PedidosTemp;
using DAL.Permissao.Security.Extensions;
using MvcFlashMessages;
using WEB.Areas.Pedidos.ViewModels;

namespace WEB.Areas.Pedidos.Controllers {

    [OrganizacaoFilter]
    public class PedidoCadastroController : Controller {

        //Constantes

        //Atributos
        private IPedidoTempBL _IPedidoTempBL;
        private IPedidoCadastroBL _IPedidoCadastroBL;

        //Propriedades
        private IPedidoTempBL OPedidoTempBL => _IPedidoTempBL = _IPedidoTempBL ?? new PedidoTempBL();
        private IPedidoCadastroBL OPedidoCadastroBL => _IPedidoCadastroBL = _IPedidoCadastroBL ?? new PedidoCadastroBL();

        //Inicio de um novo pedido ou visualização de um pedido já realizado
        [HttpGet]
        public ActionResult index() {

            int idPessoa = UtilRequest.getInt32("idPessoa");

            bool? flagNovo = UtilRequest.getBool("flagNovo");

            var ViewModel = new PedidoCadastroForm();
            
            ViewModel.Pedido = this.OPedidoTempBL.carregar(Session.SessionID);

            if (ViewModel.Pedido == null || flagNovo == true) {
            
                ViewModel.Pedido = new PedidoTemp();

                ViewModel.Pedido.idPessoa = idPessoa;

                ViewModel.Pedido.idSessao = Session.SessionID;

                ViewModel.Pedido.idPais = "BRA";

                ViewModel.Pedido.idTipoEndereco = TipoEnderecoConst.PRINCIPAL;

                ViewModel.Pedido.cepOrigem = ConfiguracaoEcommerceBL.getInstance.carregar(User.idOrganizacao()).cepOrigemFrete;

                this.OPedidoTempBL.salvar(ViewModel.Pedido);

                ViewModel.Pedido.listaProdutos = new List<PedidoProdutoTemp>();

            }

            if (ViewModel.Pedido.idPessoa > 0) {

                ViewModel.carregarDadosComprador(ViewModel.Pedido.idPessoa.toInt());

            }
            
            return View(ViewModel);
        }
        
        //
        [HttpPost, ActionName("salvar-comprador")]
        public ActionResult salvarComprador(PedidoCadastroForm ViewModel) {

            if (!ModelState.IsValid) {
                return View("index", ViewModel);
            }

            var OPedidoTemp = this.OPedidoTempBL.carregar(Session.SessionID);
            
            OPedidoTemp.idPessoa = ViewModel.Pedido.idPessoa;
            
            OPedidoTemp.idAssociado = ViewModel.Pedido.idAssociado;
            
            OPedidoTemp.idNaoAssociado = ViewModel.Pedido.idNaoAssociado;

            this.OPedidoTempBL.salvar(OPedidoTemp);

            return RedirectToAction("index");

        }

        //Submit do pedido com dados para criação ou atualização
        [HttpPost, ActionName("salvar-pedido")]
        public ActionResult salvarPedido(PedidoCadastroForm ViewModel) {

            var OPedidoTemp = this.OPedidoTempBL.carregar(Session.SessionID);

            // Preencher os dados financeiros da tabela temporária
            OPedidoTemp = ViewModel.prencherDadosFinanceiros(OPedidoTemp);

            // Preencher os dados de agendamento/limite de entrega da tabela temporária
            OPedidoTemp = ViewModel.prencherDadosDataEntrega(OPedidoTemp);


            this.OPedidoTempBL.salvar(OPedidoTemp);

            // Validados dados do pedido
            var OPedidoValidacaoVM = new PedidoCadastroValidacaoVM();
            var ORetorno = OPedidoValidacaoVM.validar(OPedidoTemp);

            if (ORetorno.flagError) {

                this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", ORetorno.listaErros.FirstOrDefault()));

                return RedirectToAction("index");
            }
            
            var OPedidoGerado = this.OPedidoCadastroBL.salvar(OPedidoTemp);

            if (OPedidoGerado.id > 0) {

                this.Flash(UtilMessage.TYPE_MESSAGE_SUCCESS, UtilMessage.success("Sucesso!", "O pedido foi criado com sucesso."));
                
                return RedirectToAction("index", "PedidoDetalhes", new { OPedidoGerado.id });

            }

            this.Flash(UtilMessage.TYPE_MESSAGE_ERROR, UtilMessage.error("Erro!", "Houve algum problema ao gerar o pedido. Tente novamente mais tarde."));

            return RedirectToAction("index");

        }
        
        [HttpPost, ActionName("partial-box-valores")]
        public ActionResult partialBoxValores() {

            var ViewModel = new PedidoCadastroForm();
            
            ViewModel.Pedido = this.OPedidoTempBL.carregar(Session.SessionID);

            return PartialView(ViewModel);

        }
        
    }

}