using System;
using DAL.Notificacoes;

namespace BLL.Notificacoes.Interface {
    
    public interface IGeradorNotificacaoBL {

        UtilRetorno gerarNotificacao(object OrigemNotificacao);

    }
    
}