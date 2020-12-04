using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.Net;
using UTIL.CorreiosWS;
using DAL.Frete;

namespace BLL.Frete {


	public class CorreiosBL {

		//Atributos
		private CalcPrecoPrazoWSSoapClient _CorreioServico;

		//Propriedades
		private CalcPrecoPrazoWSSoapClient CorreioWS { get { return (this._CorreioServico = this._CorreioServico ?? new CalcPrecoPrazoWSSoapClient() ); } }

		public string codigoEmpresa { get; set;}
		
		public string senhaEmpresa { get; set;}

		public readonly string baseUrlCalcPrecoPrazo = "http://ws.correios.com.br/calculador/CalcPrecoPrazo.aspx?";

		//
		public CorreiosBL() { 
			//this.codigoEmpresa = "12152498";
			//this.senhaEmpresa = "08319244";
		}

		//
		public CorreiosBL(string codigoEmpresaCorreios, string senha) { 
			this.codigoEmpresa = codigoEmpresaCorreios;
			this.senhaEmpresa = senha;
		}

		//Montar a url para fazer a requisicao de consulta de CEP
		private string getUrlPrecoPrazo(string cepOrigem, string cepDestino, decimal peso, int idTipoFrete, decimal comprimento = 30, decimal altura = 20, decimal largura = 20) { 
			StringBuilder url = new StringBuilder();
			url.Append(this.baseUrlCalcPrecoPrazo);

            if (!String.IsNullOrEmpty(codigoEmpresa) && !String.IsNullOrEmpty(senhaEmpresa)) { 
			    url.Append(String.Format("nCdEmpresa={0}", codigoEmpresa));
			    url.Append(String.Format("&sDsSenha={0}", senhaEmpresa));
            }

			url.Append(String.Format("&sCepOrigem={0}", cepOrigem));
			url.Append(String.Format("&sCepDestino={0}", cepDestino));
			url.Append(String.Format("&nVlPeso={0}", peso));
			url.Append(String.Format("&nCdFormato={0}", 1));

			if (comprimento > 0) {
				url.Append(String.Format("&nVlComprimento={0}", comprimento));
			}

			if (altura > 0) {
				url.Append(String.Format("&nVlAltura={0}", altura));
			}

			if (largura > 0) {
				url.Append(String.Format("&nVlLargura={0}", largura));
			}

			url.Append(String.Format("&sCdMaoPropria={0}", "N"));
			url.Append(String.Format("&nVlValorDeclarado={0}", 0));
			url.Append(String.Format("&sCdAvisoRecebimento={0}", "N"));
			url.Append(String.Format("&nCdServico={0}", idTipoFrete));
			url.Append(String.Format("&nVlDiametro={0}", 0));
			url.Append(String.Format("&StrRetorno={0}", "xml"));	
			return url.ToString();
		}

		//Calcular o prazo do Frete PAC/SEDEX
		public List<cResultado> calcularPrecoPrazo(string cepOrigem, string cepDestino, decimal peso, decimal comprimento = 30, decimal altura = 20, decimal largura = 20 ) { 
			
			List<cResultado> listaResultados = new List<cResultado>();

			int[] idsTiposFrete = new int[]{
				Convert.ToInt32(CorreiosTipoFreteEnum.PAC),
				Convert.ToInt32(CorreiosTipoFreteEnum.SEDEX)			
			};
			
			if (peso <= 0) { 
				return listaResultados;
			}

			//tratamento para medidas mínimas aceitas pelo Correio
			decimal valorMinimo = new decimal(20);
			comprimento = (comprimento < valorMinimo)? valorMinimo: comprimento;
			altura = (altura < valorMinimo)? valorMinimo: altura;
			largura = (largura < valorMinimo)? valorMinimo: largura;
			
			foreach(int idTipoFrete in idsTiposFrete){

				var Resultado = this.CorreioWS.CalcPrecoPrazo(this.codigoEmpresa ?? "", this.senhaEmpresa ?? "", idTipoFrete.ToString(), cepOrigem, cepDestino, peso.ToString(), 1, comprimento, altura, largura, 0, "N", 0, "N");

				listaResultados.Add(Resultado);
			}

			return listaResultados;
		}
	}
}
