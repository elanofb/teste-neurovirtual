using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using BLL.Notificacoes;
using BLL.Services;
using DAL.Associados;
using DAL.Emails;
using DAL.Notificacoes;
using DAL.Contribuicoes;
using BLL.Core.Events;
using BLL.Notificacoes.Events;

namespace BLL.AvisosNotificacoes.Services {

    public interface INotificacaoUsuarioAvulsaBL {

        bool salvar(NotificacaoSistema ONotificacao, List<NotificacaoSistemaEnvio> listaEnvios);

    }
    
}
