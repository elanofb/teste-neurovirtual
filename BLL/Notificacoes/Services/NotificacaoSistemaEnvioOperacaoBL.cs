using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.Services;
using DAL.Notificacoes;
using EntityFramework.Extensions;

namespace BLL.Notificacoes {

    public class NotificacaoSistemaEnvioOperacaoBL : DefaultBL, INotificacaoSistemaEnvioOperacaoBL {

        // Atributos
        private INotificacaoSistemaEnvioBL _INotificacaoSistemaEnvioBL;

        // Propriedades
        private INotificacaoSistemaEnvioBL ONotificacaoSistemaEnvioBL => _INotificacaoSistemaEnvioBL = _INotificacaoSistemaEnvioBL ?? new NotificacaoSistemaEnvioBL();

        // 
		public UtilRetorno vincularTarefaPorNotificacao(int idNotificacao, int idTarefa) {
			
			var listaNotificacaoPessoa = this.ONotificacaoSistemaEnvioBL.listar(0, idNotificacao).Where(x => x.flagExcluido == false).AsNoTracking().ToList();

			return this.vincularTarefaLote(listaNotificacaoPessoa, idTarefa);

		}

		// 
		public UtilRetorno vincularTarefaLote(List<NotificacaoSistemaEnvio> listaNotificacaoPessoa, int idTarefa) {
			
            var idsNotificacoesEnvio = listaNotificacaoPessoa.Select(x => x.id);
            
			db.NotificacaoSistemaEnvio.Where(x => idsNotificacoesEnvio.Contains(x.id))
              .Update(x => new NotificacaoSistemaEnvio
              {

                idTarefa = idTarefa, flagEnvioEmail = false, dtEnvioEmail = null

            });

			return UtilRetorno.newInstance(false, "Os e-mails de notificação foram gerados com sucesso!");
		}

        //
        public bool registrarLeitura(int id) {

            db.NotificacaoSistemaEnvio.Where(x => x.id == id)
              .Update(x => new NotificacaoSistemaEnvio { dtLeitura = DateTime.Now });
            
            var listaCheck = db.NotificacaoSistemaEnvio.Where(x => x.id == id && x.dtLeitura == null).ToList();
            return (listaCheck.Count == 0);

        }

    }

}
