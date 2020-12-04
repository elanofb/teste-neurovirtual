using System;
using System.Collections.Generic;
using System.Linq;
using BLL.AssociadosContribuicoes;
using BLL.AssociadosContribuicoes.Emails;
using BLL.Configuracoes;
using BLL.Notificacoes;
using DAL.Notificacoes;
using BLL.Services;
using DAL.AssociadosContribuicoes;
using EntityFramework.Extensions;

namespace BLL.AssociadosNotificacoes {

    public class NotificacaoSistemaEnvioCobrancaContribuicaoBL : DefaultBL, INotificacaoSistemaEnvioCobrancaBL {

        // Atributos
        private IAssociadoContribuicaoResumoBL _IAssociadoContribuicaoResumoBL;

        // Propriedades
        private IAssociadoContribuicaoResumoBL OAssociadoContribuicaoResumoBL => _IAssociadoContribuicaoResumoBL = _IAssociadoContribuicaoResumoBL ?? new AssociadoContribuicaoResumoBL();


        public NotificacaoSistemaEnvioCobrancaContribuicaoBL() {

        }
        
		// 1 - Carregar uma cobranca registrada
		// 2 - Carregar os dados da anuidade
		// 3 - carregar e-mails que devem ser copiados
		// 4 - Chamar servico de disparo de e-mail
		// 5 - Registrar o envio
		public void enviarEmail(List<NotificacaoSistemaEnvio> listaEnvios) {
            
            var idsAssociadosContribuicao = listaEnvios.Select(x => x.idReferencia).ToList();

            var listaContribuicoes = this.OAssociadoContribuicaoResumoBL.listar(0, 0).Where(x => idsAssociadosContribuicao.Contains(x.id)).ToList();

            var listaIdsEnviados = new List<int>();
			var listaExcluidos = new List<UtilRetorno>();
            var listaIdsRemovidos = new List<int>();

		    foreach (var ONotificacaoEnvio in listaEnvios) {
		    
                var OAssociadoContribuicao = listaContribuicoes.FirstOrDefault(x => x.id == ONotificacaoEnvio.idReferencia);

		        if (OAssociadoContribuicao == null){
		            listaIdsRemovidos.Add(ONotificacaoEnvio.id);
				    continue;
		        }

                var ORetorno = this.enviarEmail(ONotificacaoEnvio.NotificacaoSistema, OAssociadoContribuicao, ONotificacaoEnvio.ToEmailList());
                
			    if (!ORetorno.flagError) { 
                    listaIdsEnviados.Add(ONotificacaoEnvio.id);
			    }
			    
			    if (ORetorno.flagError) { 
				    
				    ORetorno.info = ONotificacaoEnvio.id;
				    
				    listaExcluidos.Add(ORetorno);   
			    }

		    }

            if (listaIdsEnviados.Any()) {

                db.NotificacaoSistemaEnvio
                    .Where(x => listaIdsEnviados.Contains(x.id))
                    .Update(x => new NotificacaoSistemaEnvio() { dtEnvioEmail = DateTime.Now, flagEnvioEmail = true });
            } 

			if (listaExcluidos.Any()) {

				var idsEnviosExcluidos = listaExcluidos.Select(x => x.info.toInt()).Distinct().ToList();

				var listaEnviosExcluidos = db.NotificacaoSistemaEnvio.Where(x => idsEnviosExcluidos.Contains(x.id)).ToList(); 
                
				listaEnviosExcluidos.ForEach(x => {

					x.flagExcluido = true;

					x.motivoExclusao = listaExcluidos.FirstOrDefault(c => c.info.toInt() == x.id)?.listaErros?.FirstOrDefault() ?? "Os e-mails configurados não são válidos.";

				});

				db.SaveChanges();
	            
			}

            if (listaIdsRemovidos.Any()) {

                db.NotificacaoSistemaEnvio
                    .Where(x => listaIdsRemovidos.Contains(x.id))
                    .Update(x => new NotificacaoSistemaEnvio() { flagExcluido = true, motivoExclusao = "A cobrança informada não existe ou não foi localizada." });
            }
            
		}

		// 1 - Chamada do servico para disparo do e-mail
		private UtilRetorno enviarEmail(NotificacaoSistema ONotificacaoSistemaEnvio, AssociadoContribuicaoResumoVW OAssociadoContribuicao, List<string> listaEmails) {

			IEnvioCobrancaContribuicao EnvioEmail = EnvioCobrancaContribuicao.factory(OAssociadoContribuicao.idOrganizacao, listaEmails, new List<string>());

			return EnvioEmail.enviar(ONotificacaoSistemaEnvio, OAssociadoContribuicao);
		}

    }

}
