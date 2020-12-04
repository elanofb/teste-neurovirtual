using System;
using System.Linq;
using DAL.Notificacoes;

namespace BLL.Notificacoes {

	public interface INotificacaoSistemaEnvioBL {

		NotificacaoSistemaEnvio carregar(int id);

        IQueryable<NotificacaoSistemaEnvio> listar(int idReferencia, int idNotificacao);

        IQueryable<NotificacaoSistemaEnvio> listar(int idTarefa, bool? flagEnviado);

        bool salvar(NotificacaoSistemaEnvio oNotificacaoSistemaEnvio);

        UtilRetorno excluir(int id);

	}
}
