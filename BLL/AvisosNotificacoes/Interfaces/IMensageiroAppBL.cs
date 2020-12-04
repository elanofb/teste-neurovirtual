using System;
using System.Collections.Generic;
using DAL.Notificacoes;

namespace BLL.AvisosNotificacoes.Services {

    public interface IMensageiroAppBL {

        UtilRetorno enviar(NotificacaoSistema ONotificacao, List<PessoaDevice> listaDispositivos);
        
    }
    
}
