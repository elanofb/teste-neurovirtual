using System;
using System.Linq;
using BLL.Services;
using DAL.Notificacoes;

namespace BLL.Notificacoes {

    public class NotificacaoPostbackConsultaBL : DefaultBL, INotificacaoPostbackConsultaBL {
        
        // 
        public IQueryable<NotificacaoPostback> query() {

            var query = from PA in db.NotificacaoPostback
                where PA.dtExclusao == null
                select PA;

            return query;

        }

        //
        public NotificacaoPostback carregar(int id) {

            var query = this.query().condicoesSeguranca();

            return query.FirstOrDefault(x => x.id == id);
        }


        //
        public IQueryable<NotificacaoPostback> listar(bool? ativo = true) {

            var query = this.query().condicoesSeguranca();

            if(ativo != null) {
                query = query.Where(x => x.ativo == ativo);
            }

            return query;
        }

    }
    
}