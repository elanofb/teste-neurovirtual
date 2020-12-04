using System;
using BLL.BancoCentral.Cotacoes;
using BLL.Services;
using DAL.Financeiro;

namespace BLL.Financeiro {

    public class CotacaoBancoCentralConsultaBL : DefaultBL, ICotacaoBancoCentralConsultaBL {

        private FachadaWSSGSClient WsBancoCentral = new FachadaWSSGSClient();
        private int _serie;
        
        public CotacaoBancoCentralConsultaBL(int serie) {
            _serie = serie;
        }
        
        //Carregamento de registro pelo ID
        public CotacaoDolarDTO consultar() {

            WSSerieVO Retorno = WsBancoCentral.getUltimoValorVO(_serie);

            var OCotacaoDolar = new CotacaoDolarDTO();

            OCotacaoDolar.valor = Retorno.ultimoValor.svalor.Replace(".", ",").toDecimal();
            OCotacaoDolar.nomeAbreviado = Retorno.nomeAbreviado;
            OCotacaoDolar.nomeCompleto = Retorno.nomeCompleto;
            OCotacaoDolar.nomeCompleto = Retorno.unidadePadrao;

            return OCotacaoDolar;

        }
        
    }
}
