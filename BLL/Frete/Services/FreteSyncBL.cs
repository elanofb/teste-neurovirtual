using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DAL.Frete;
using DAL.Configuracoes;
using BLL.Localizacao;
using BLL.Configuracoes;
using BLL.ConfiguracoesEcommerce;
using DAL.Localizacao;

namespace BLL.Frete {

   
    public class FreteSyncBL {

        //Constantes

        //Atributos
        private ICepBrasilSyncBL _ICepBrasilSyncBL;
        private CorreiosBL _CorreiosBL;

        //Propriedades
        private ICepBrasilSyncBL OCepBrasilSyncBL => _ICepBrasilSyncBL = _ICepBrasilSyncBL ?? new CepBrasilSyncBL();
        private CorreiosBL OCorreiosBL => _CorreiosBL = _CorreiosBL ?? new CorreiosBL();

        //
        public FreteSyncBL() {

        }

		//Buscar informacao de CEP para cálculo
        public List<CorreiosFreteRetorno> buscar(string cepOrigem, string cepDestino, decimal peso, decimal comprimento, decimal altura, decimal largura) {

            cepDestino = UtilString.onlyNumber(cepDestino);
            List<CorreiosFreteRetorno> listaRetorno = new List<CorreiosFreteRetorno>() ;

            if (peso > 0) {
                
                var listaFretes = this.OCorreiosBL.calcularPrecoPrazo(cepOrigem, cepDestino, peso, comprimento, altura, largura);

                listaRetorno = new List<CorreiosFreteRetorno>();

                foreach (var DadosFrete in listaFretes) {

                    var ItemRetorno = new CorreiosFreteRetorno();

                    foreach (var Item in DadosFrete.Servicos) {

                        ItemRetorno.valorEntrega = UtilNumber.toDecimal(Item.Valor);
                        ItemRetorno.prazoEntrega = Item.PrazoEntrega;
                        ItemRetorno.codigoServico = Item.Codigo;

                        listaRetorno.Add(ItemRetorno);

                    }
                }
            }

            CepBrasil OCep = this.OCepBrasilSyncBL.buscarEndereco(cepDestino);

            if (OCep.id > 0) {

                if (listaRetorno.Count == 0) {
                    listaRetorno = new List<CorreiosFreteRetorno>() { new CorreiosFreteRetorno() };
                }

                listaRetorno.ForEach(Item => {
				
					Item.cepOriginal = OCep.cepOriginal;
					
					Item.bairro = OCep.bairroIni;
					
					Item.idCidade = OCep.idCidade;
					
					Item.nomeCidade = OCep.nomeCidade;
					
					Item.idEstado = OCep.idEstado;
					
					Item.siglaEstado = OCep.siglaEstado;
					
					Item.tipoLogradouro = OCep.tipoLogradouro;
					
					Item.logradouro = OCep.logradouro;
				});
            }

            return listaRetorno;
        }


    }
}
