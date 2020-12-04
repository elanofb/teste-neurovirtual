using System;
using System.Linq;
using DAL.PedidosTemp;

namespace WEB.Areas.Pedidos.ViewModels{
    
    public class PedidoCadastroValidacaoVM {
        
        //Atributos

        //Propriedades
        
        //
        public UtilRetorno validar(PedidoTemp OPedidoTemp) {

            var ORetorno = UtilRetorno.newInstance(false);

            if (!OPedidoTemp.listaProdutos.Any()) {
                
                ORetorno.flagError = true;
                
                ORetorno.listaErros.Add("O pedido não pode ser gerado pois não possui nenhum produto adicionado.");
                
                return ORetorno;
            }

            if (OPedidoTemp.listaProdutos.Count > 1) {
                
                ORetorno.flagError = true;
                
                ORetorno.listaErros.Add("Não deve ser adicionado mais de um produto por pedido.");
                
                return ORetorno;
            }

            if (OPedidoTemp.flagFaturamentoCadastro &&
                OPedidoTemp.flagCartaoCreditoPermitido != true &&
                OPedidoTemp.flagBoletoBancarioPermitido != true &&
                OPedidoTemp.flagDepositoPermitido != true) {

                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Você deve habilitar pelo menos uma forma de pagamento.");
                return ORetorno;

            }


            return ORetorno;

        }
        
	}

}