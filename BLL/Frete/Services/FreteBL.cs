using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using DAL.Frete;
using BLL.Localizacao;
using DAL.Localizacao;
using BLL.ConfiguracoesEcommerce;

namespace BLL.Frete {


    public class FreteBL {

        //Constantes

        //Atributos
        private ICepBrasilBL _ICepBrasilBL;
        private CorreiosBL _CorreiosBL;

        //Propriedades
        private ICepBrasilBL OCepBrasilBL => _ICepBrasilBL = _ICepBrasilBL ?? new CepBrasilBL();
        private CorreiosBL OCorreiosBL => _CorreiosBL = _CorreiosBL ?? new CorreiosBL();

        //
        public FreteBL() {

        }

        /// <summary>
        /// Calcular o frete verificando configuracoes
        /// </summary>
        public async Task<List<CorreiosFreteRetorno>> calcularFrete(int idOrganizacaoParam, decimal valorProdutos, string cepDestino, decimal peso, decimal comprimento, decimal altura, decimal largura) {

            var ConfiguracaoEcommerce = ConfiguracaoEcommerceBL.getInstance.carregar(idOrganizacaoParam);

            peso = peso > 0 ? peso : new decimal(0.100);

            var listaFretes = await this.buscar(ConfiguracaoEcommerce.cepOrigemFrete, cepDestino, peso, comprimento, altura, largura);

            if (ConfiguracaoEcommerce.flagHabilitarFreteGratuito == true && valorProdutos >= ConfiguracaoEcommerce.valorParaFreteGratuito.toDecimal()) {

                foreach (var OFrete in listaFretes) {

                    OFrete.flagFreteGratis = true;

                    OFrete.valorEntrega = new decimal(0);

                }
            }

            return listaFretes;
        }

        //Buscar informacao de CEP para cálculo
        public async Task<List<CorreiosFreteRetorno>> buscar(string cepOrigem, string cepDestino, decimal peso, decimal comprimento, decimal altura, decimal largura) {

            cepDestino = UtilString.onlyNumber(cepDestino);

            List<CorreiosFreteRetorno> listaRetorno = new List<CorreiosFreteRetorno>();
    
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

            CepBrasil OCep = await this.OCepBrasilBL.buscarEndereco(cepDestino);

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
