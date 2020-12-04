using System;
using System.Linq;
using BLL.Emails;
using DAL.Emails;

namespace WEB.Areas.Associados.ViewModels {
    
    public class ConfiguracaoEmailForm {
        
        
        // Atributos Serviços     
        private IMensagemEmailConsultaBL _MensagemEmailConsultaBL;
        
        // Propriedades Serviços        
        private IMensagemEmailConsultaBL OMensagemEmailConsultaBL => _MensagemEmailConsultaBL = _MensagemEmailConsultaBL ?? new MensagemEmailConsultaBL();
        
        // Propriedades
        public MensagemEmail MensagemEmailAtualizacaoCadastral { get; set; }
        
        public int? idReferencia { get; set; }
        
        //
        public ConfiguracaoEmailForm() {            
            
            this.MensagemEmailAtualizacaoCadastral = new MensagemEmail();        
        }
        
        /// <summary>
        /// Carregar e-mails configurados
        /// </summary>
        public void carregarEmails(){
                                    
            this.MensagemEmailAtualizacaoCadastral = this.OMensagemEmailConsultaBL.listar(IdentificacaoMensagemEmailConst.ASSOCIADO_LINK_ATUALIZACAO).FirstOrDefault() ?? new MensagemEmail();
            
            this.MensagemEmailAtualizacaoCadastral.codigoIdentificacao = IdentificacaoMensagemEmailConst.ASSOCIADO_LINK_ATUALIZACAO;
            
            this.carregarConteudoPadrao();
        }
        
        /// <summary>
        /// Carregar conteudos Padrao
        /// </summary>
        public void carregarConteudoPadrao(){
            
            if (this.MensagemEmailAtualizacaoCadastral.titulo.isEmpty()){
                this.MensagemEmailAtualizacaoCadastral.titulo = AssociadoEmailsConst.tituloEmailAtualizacaoCadastral;
            }            
            if (this.MensagemEmailAtualizacaoCadastral.corpoEmail.isEmpty()){
                this.MensagemEmailAtualizacaoCadastral.corpoEmail = AssociadoEmailsConst.corpoEmailAtualizacaoCadastral;
            }
            
        }
        
    }

}