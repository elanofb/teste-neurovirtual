using BLL.Core.Events;
using BLL.Financeiro;
using BLL.CuponsDesconto;
using System;
using DAL.Planos;
using BLL.Email;
using System.Collections.Generic;
using DAL.Permissao.Security.Extensions;

namespace BLL.Planos {

	public class OnPlanoContratadoHandler : IHandler<object> {

		//Atributos
        private IPlanoContratacaoBL _PlanoContratacaoBL;
        private ITituloReceitaBL _TituloReceitaBL;
        private ICupomDescontoBL _CupomDescontoBL;

        //Propriedades
        private IPlanoContratacaoBL OPlanoContratacaoBL { get { return (this._PlanoContratacaoBL = this._PlanoContratacaoBL ?? new PlanoContratacaoBL()); } }
        private ITituloReceitaBL OTituloReceitaBL { get { return (this._TituloReceitaBL = this._TituloReceitaBL ?? new TituloReceitaPlanoContratacaoBL()); } }
        private ICupomDescontoBL OCupomDescontoBL { get { return (this._CupomDescontoBL = this._CupomDescontoBL ?? new CupomDescontoBL()); } }

		//
		public void execute(object source) {
            
            PlanoContratacao OPlanoContratacao = (source as PlanoContratacao);

			PlanoContratacao dbPlanoContratacao = this.OPlanoContratacaoBL.carregar(OPlanoContratacao.id);

            this.gerarContratacaoEmail(dbPlanoContratacao);

            this.gerarTituloFinanceiro(dbPlanoContratacao);

            if (OPlanoContratacao.idCupomDesconto != null) {
                OCupomDescontoBL.registrarUso(Convert.ToInt32(dbPlanoContratacao.idCupomDesconto));
            }
		}

		//Gerar Registro de contratação
		public void gerarContratacaoEmail(PlanoContratacao OPlanoContratacao) { 

			List<string> listaEmails = new List<string>();
			List<string> listaCopias = new List<string>();

			listaEmails.Add(OPlanoContratacao.Pessoa.emailPrincipal);

			Dictionary<string, object> infosEmail = new Dictionary<string,object>();
			
			infosEmail["PlanoContratacao"] = (OPlanoContratacao as object);
						
			EnvioEmailAdapter EnvioEmail = EnvioPlanoContratacao.factory(HttpContextFactory.Current.User.idOrganizacao(), listaEmails, listaCopias);
			EnvioEmail.enviar(infosEmail, "");
		}

		//Gerar o e-mail de cobrança para envio posterior
		public void gerarTituloFinanceiro(PlanoContratacao OPlanoContratacao) { 
			//this.OTituloReceitaBL.gerar(OPlanoContratacao);
		}
	}
}