using BLL.Core.Events;
using DAL.Associados;
using System.Linq;
using System.Collections.Generic;
using System;
using BLL.Email;
using BLL.Pessoas;

namespace BLL.NaoAssociados.Events {

	public class OnNaoAssociadoEnvioFichaHandler : IHandler<object> {
        
		//Atributos
        private INaoAssociadoBL _NaoAssociadoBL;

		//Propridades
		private Associado ONaoAssociado { get; set;}
		private INaoAssociadoBL ONaoAssociadoBL => (this._NaoAssociadoBL = this._NaoAssociadoBL ?? new NaoAssociadoBL() );

	    //Chamador das ações do evento
        public void execute(object source) {
			try {

                var listaParametros = source as List<object>;

                var idNaoAssociado = Convert.ToInt32(listaParametros[0]);
                var emails = (listaParametros[1] as string);

				this.ONaoAssociado = this.ONaoAssociadoBL.carregar(idNaoAssociado).condicoesSeguranca().FirstOrDefault();
                
				this.dispararEmail(emails);

			} catch (Exception ex) {
				UtilLog.saveError(ex, String.Format("Erro no manipulador do evento de envio de email da ficha cadastro do não associado {0}", this.ONaoAssociado.Pessoa.nome));
			}
		}
        
        //
        private void dispararEmail(string emails) {

            try {

                List<string> listaEmail = emails.Split(';').ToList();

			    var OEmail = EnvioNaoAssociadoFichaCadastral.factory(ONaoAssociado.idOrganizacao, listaEmail, null, null);
            
			    OEmail.enviar(this.ONaoAssociado);

            } catch (Exception ex) {
                UtilLog.saveError(ex, "Erro ao enviar e-mail com a ficha cadastral do não associado.");
            }
        }
		
	}
}