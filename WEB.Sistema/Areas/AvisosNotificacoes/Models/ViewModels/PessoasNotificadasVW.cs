using System.Collections.Generic;
using DAL.Notificacoes;
using PagedList;

namespace WEB.Areas.AvisosNotificacoes.ViewModels {

    
    public class PessoasNotificadasVW {
        
        public int idNotificacao { get; set; }

        public IPagedList<NotificacaoSistemaEnvio> listaPessoasNotificadas { get; set; }
        
        public int qtdeEnviados { get; set; }

        public int qtdeProblemaEnvio { get; set; }

        public int qtdeFila { get; set; }

        public PessoasNotificadasVW() {

            this.listaPessoasNotificadas = new List<NotificacaoSistemaEnvio>().ToPagedList(1, 20);

        }
    }
}