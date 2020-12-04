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
using BLL.Organizacoes;
using DAL.Permissao;
using DAL.Repository.Base;

namespace BLL.AvisosNotificacoes.Services {

    public class MensageiroAppFactoryBL : DefaultBL {

        //Atributos
        private static MensageiroAppFactoryBL _instance;
        
        //Propriedades
        public static MensageiroAppFactoryBL getInstance => _instance = _instance ?? new MensageiroAppFactoryBL();
        
        //Definir qual título receita irá fazer os tratamentos
        public IMensageiroAppBL factory(NotificacaoSistema ONotificacao) {
         
            if(ONotificacao.idGatewayPush == GatewayNotificacaoConst.ONESIGNAL) {
                
            }
         
            return null;
        }
    }
    
}
