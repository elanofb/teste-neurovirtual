using System;
using System.Linq;
using System.Collections.Generic;
using BLL.Email;
using BLL.Pessoas;
using DAL.Eventos;
using DAL.Planos;

namespace BLL.Planos {

	public class EnvioPlanoContratacao : EnvioEmailAdapter {

        //Atributos
        private IPlanoContratacaoBL _PlanoContratacaoBL;

        //Propriedades
        private IPlanoContratacaoBL OPlanoContratacaoBL { get { return (this._PlanoContratacaoBL = this._PlanoContratacaoBL ?? new PlanoContratacaoBL()); } }


		//Private Construtor
		private EnvioPlanoContratacao(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia, List<string> listaCopiaOculta) :
			base(idOrganizacaoParam, listaDestino, listaCopia, listaCopiaOculta) {
		}

		//Factory
		public static EnvioPlanoContratacao factory(int idOrganizacaoParam, List<string> listaDestino, List<string> listaCopia) { 
			return new EnvioPlanoContratacao(idOrganizacaoParam, listaDestino, listaCopia, null);
		}

		//Sobreposicao obrigatorio do metodo abstrato
		public override UtilRetorno enviar(IDictionary<string, object> info, string assunto = "") {

			PlanoContratacao OPlanoContratacao = (info["PlanoContratacao"] as PlanoContratacao);
			
			PlanoContratacao dbPlanoContratacao = this.OPlanoContratacaoBL.carregar(OPlanoContratacao.id);

			assunto = "Contratação da Vitrine de Serviço";
				
			this.Subject = assunto;

			this.prepararMensagem("contratacao-vitrine.html");

			this.Body = this.Body.Replace("#NOME#", dbPlanoContratacao.Pessoa.nome);
			
			this.Body = this.Body.Replace("#PLANO#", dbPlanoContratacao.Plano.nome);
			
			this.Body = this.Body.Replace("#PERIODO#", dbPlanoContratacao.Plano.qtdeMesVigencia + " Meses");

			this.Body = this.Body.Replace("#VALOR#", dbPlanoContratacao.valor.ToString("C"));

			return this.disparar();
		}
	}
}