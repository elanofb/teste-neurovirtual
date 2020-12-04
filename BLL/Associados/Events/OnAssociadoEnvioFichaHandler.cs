using BLL.Core.Events;
using DAL.Associados;
using System.Linq;
using System.Collections.Generic;
using System;
using BLL.Email;
using BLL.Pessoas;

namespace BLL.Associados.Events {

	public class OnAssociadoEnvioFichaHandler : IHandler<object> {
        
		//Atributos
        private IAssociadoBL _AssociadoBL;

		//Propridades
		private Associado OAssociado { get; set;}
		private IAssociadoBL OAssociadoBL => (this._AssociadoBL = this._AssociadoBL ?? new AssociadoBL() );

	    //Chamador das ações do evento
        public void execute(object source) {
			try {

                var listaParametros = source as List<object>;

                var idAssociado = Convert.ToInt32(listaParametros[0]);
                var emails = (listaParametros[1] as string);

				this.OAssociado = this.OAssociadoBL.carregar(idAssociado);
                
				this.dispararEmail(emails);

			} catch (Exception ex) {
				UtilLog.saveError(ex, String.Format("Erro no manipulador do evento de envio de email da ficha cadastro do associado {0}", this.OAssociado.Pessoa.nome));
			}

		}
        
        //
        private void dispararEmail(string emails) {

            try {

                List<string> listaEmail = emails.Split(';').ToList();

			    var OEmail = EnvioAssociadoFichaCadastral.factory(OAssociado.idOrganizacao,listaEmail, null, null);
            
			    OEmail.enviar(this.OAssociado);

            } catch (Exception ex) {
	            
                UtilLog.saveError(ex, "Erro ao enviar e-mail com a ficha cadastral do associado.");
	            
            }
	        
        }
	}
}