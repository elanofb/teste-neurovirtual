using System.Collections.Generic;
using DAL.Notificacoes;

namespace BLL.Notificacoes{
    
    public interface INotificacaoSistemaCadastroBL{
        
        /// <summary>
        /// Atualizar ou criar uma nova notificacao
        /// </summary>
        bool salvar(NotificacaoSistema ONotificacaoSistema);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listaNotificacoesVinculadas"></param>
        void salvarDetalhesNotificacao(List<NotificacaoSistemaEnvio> listaNotificacoesVinculadas);
    }
}