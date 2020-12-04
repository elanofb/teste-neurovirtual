using System;
using System.Collections.Generic;
using DAL.Notificacoes;

namespace BLL.Notificacoes {

    public interface INotificacaoSistemaEnvioOperacaoBL {

        UtilRetorno vincularTarefaPorNotificacao(int idNotificacao, int idTarefa);

		UtilRetorno vincularTarefaLote(List<NotificacaoSistemaEnvio> listaNotificacaoPessoa, int idTarefa);

        bool registrarLeitura(int id);

    }

}
