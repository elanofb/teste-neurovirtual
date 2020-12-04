using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using BLL.Frete;
using BLL.PedidosTemp;
using DAL.Enderecos;
using DAL.Frete;
using DAL.PedidosTemp;
using DAL.PedidosTemp.Extensions;
using WEB.Areas.Pedidos.ViewModels;

namespace WEB.Areas.Pedidos.Controllers {

    public class PedidoCadastroFreteController : Controller {

        //Constantes

        //Atributos
        private FreteBL _FreteBL;
        private IPedidoTempBL _IPedidoTempBL;

        //Propriedades
        private FreteBL OFreteBL => _FreteBL = _FreteBL ?? new FreteBL();
        private IPedidoTempBL OPedidoTempBL => _IPedidoTempBL = _IPedidoTempBL ?? new PedidoTempBL();
        
        //
        [HttpPost, ActionName("salvar-endereco-entrega")]
        public ActionResult salvarEnderecoEntrega(PedidoTemp ODadosEntrega) {

            var OPedidoTemp = this.OPedidoTempBL.carregar(Session.SessionID);

            OPedidoTemp.idPais = "BRA";

            OPedidoTemp.idTipoEndereco = TipoEnderecoConst.PRINCIPAL;

            OPedidoTemp.cepOrigem = ODadosEntrega.cepOrigem;

            OPedidoTemp.cep = ODadosEntrega.cep;

            OPedidoTemp.logradouro = ODadosEntrega.logradouro;

            OPedidoTemp.numero = ODadosEntrega.numero;

            OPedidoTemp.complemento = ODadosEntrega.complemento;

            OPedidoTemp.bairro = ODadosEntrega.bairro;

            OPedidoTemp.idCidade = ODadosEntrega.idCidade;

            OPedidoTemp.nomeCidade = ODadosEntrega.nomeCidade;

            OPedidoTemp.idEstado = ODadosEntrega.idEstado;

            OPedidoTemp.dtAgendamentoEntrega = ODadosEntrega.dtAgendamentoEntrega;

            OPedidoTemp.flagPeriodo = ODadosEntrega.flagPeriodo;

            this.OPedidoTempBL.salvar(OPedidoTemp);
            
            return Json(true);

        }

        //Buscar frete para calcular o pedido
        [HttpPost]
        public async Task<JsonResult> buscarFrete() {
            
            var OPedidoTemp = this.OPedidoTempBL.carregar(Session.SessionID);
            
            if (OPedidoTemp.cepOrigem.LengthNullable() < 8 || OPedidoTemp.cep.LengthNullable() < 8) {
                return Json(new { errorSemCep = true });
            }

            var OPedidoFreteValidacaoVM = new PedidoCadastroFreteValidacaoVM();

            var ORetorno = OPedidoFreteValidacaoVM.validar(OPedidoTemp);

            if (ORetorno.flagError) {

                OPedidoTemp.idTipoFrete = null;
                OPedidoTemp.valorFrete = 0;

                this.OPedidoTempBL.salvar(OPedidoTemp);

                return Json(new { error = true, message = ORetorno.listaErros.FirstOrDefault() });
            }

            var pesoTotal = OPedidoTemp.listaProdutos.Where(x => x.flagCalcularFrete == true).ToList().getPesoTotal();

            var ORetornoFrete = await this.OFreteBL.buscar(OPedidoTemp.cepOrigem, OPedidoTemp.cep, pesoTotal, 0, 0, 0);

            var OFrete = ORetornoFrete.FirstOrDefault();

            if (pesoTotal > 0) { 
	            OFrete = ORetornoFrete.FirstOrDefault(x => x.codigoServico == CorreiosTipoFreteConst.SEDEX);
            }

            if (OFrete == null) {
		         return Json(new { error = true, message="Não localizamos valores de frete no momento. Tente novamente mais tarde." }, JsonRequestBehavior.AllowGet);
	        }
            
            OPedidoTemp.idTipoFrete = OFrete.codigoServico;
            OPedidoTemp.valorFrete = OFrete.valorEntrega;

            this.OPedidoTempBL.salvar(OPedidoTemp);

            var Retorno = new {
                OFrete.codigoServico,
                OFrete.prazoEntrega,
                prazoEntregaTratado = OFrete.prazoEntregaTratado(),
                OFrete.valorEntrega,
                valorEntregaFormatado = OFrete.valorEntrega.ToString("C")
            };

            return Json(Retorno, JsonRequestBehavior.AllowGet);

        }
    }
}