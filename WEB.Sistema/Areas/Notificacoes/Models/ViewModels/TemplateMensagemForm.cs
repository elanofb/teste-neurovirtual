using System;
using System.Security.Principal;
using BLL.Notificacoes;
using DAL.Notificacoes;

namespace WEB.Areas.Notificacoes.ViewModels {


    public class TemplateMensagemForm {

        // Atributos Serviços
        private ITemplateMensagemConsultaBL _ITemplateMensagemConsultaBL;

        // Propriedades Serviços
        private ITemplateMensagemConsultaBL OTemplateMensagemConsultaBL => _ITemplateMensagemConsultaBL = _ITemplateMensagemConsultaBL ?? new TemplateMensagemConsultaBL();

        //Propriedades
        public TemplateMensagem TemplateMensagem { get; set; }
        
        // Constants
        private IPrincipal User => HttpContextFactory.Current.User;

        //
        public TemplateMensagemForm() {

            this.TemplateMensagem = new TemplateMensagem();
            
        }

        public void carregar(int id) {

            if(id == 0){return;}
            
            this.TemplateMensagem = OTemplateMensagemConsultaBL.carregar(id);

        }

    }
    
}