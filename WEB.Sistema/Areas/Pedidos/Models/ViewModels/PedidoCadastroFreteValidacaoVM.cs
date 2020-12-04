using System;
using System.Linq;
using DAL.PedidosTemp;
using DAL.PedidosTemp.Extensions;

namespace WEB.Areas.Pedidos.ViewModels{
    
    public class PedidoCadastroFreteValidacaoVM {
        
        //
        public UtilRetorno validar(PedidoTemp OPedidoTemp) {

            var ORetorno = UtilRetorno.newInstance(false);

            if (OPedidoTemp.cepOrigem.LengthNullable() < 8) {
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Informe o CEP de origem corretamente para realizar o cálculo de frete.");
                return ORetorno;
            }

            if (OPedidoTemp.cep.LengthNullable() < 8) {
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Informe o CEP corretamente para realizar o cálculo de frete.");
                return ORetorno;
            }

            var listaProdutosAdicionados = OPedidoTemp.listaProdutos;

            if (!listaProdutosAdicionados.Any()) {
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("O pedido não possui produtos para realizar o cálculo de frete.");
                return ORetorno;
            }
            
            if (listaProdutosAdicionados.All(x => x.flagCalcularFrete != true)) {
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Nenhum dos produtos adicionados foi configurado para calcular frete.");
                return ORetorno;
            }

            var pesoTotal = listaProdutosAdicionados.Where(x => x.flagCalcularFrete == true).ToList().getPesoTotal();

            if (pesoTotal == 0) {
                ORetorno.flagError = true;
                ORetorno.listaErros.Add("Não é possível cálcular o frete do pedido, pois não há peso informado nos produtos.");
                return ORetorno;
            }

            return ORetorno;

        }
        
	}

}