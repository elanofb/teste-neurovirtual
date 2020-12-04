using System;
using System.Linq;
using BLL.Services;
using DAL.Emails;
using DAL.Permissao.Security.Extensions;

namespace BLL.Emails {

    public class MensagemEmailCadastroBL : MensagemEmailConsultaBL, IMensagemEmailCadastroBL {
        
        // Atributos
        private IMensagemEmailExclusaoBL _MensagemEmailExclusaoBL;        

        // Serviços
        private IMensagemEmailExclusaoBL OMensagemEmailExclusaoBL => _MensagemEmailExclusaoBL = _MensagemEmailExclusaoBL ?? new MensagemEmailExclusaoBL();        
        
        /// <summary>
        /// Persistir e salvar os dados
        /// </summary>
        public bool salvar(MensagemEmail OMensagemEmail) {
            
            OMensagemEmail.emailsCopia = OMensagemEmail.emailsCopia.abreviar(255);
            
            OMensagemEmail.titulo = OMensagemEmail.titulo.abreviar(255);
            
            OMensagemEmail.corpoEmail = OMensagemEmail.corpoEmail.abreviar(8000);
            
            int idOrganizao = User.idOrganizacao();
                                        
            if (this.existe(OMensagemEmail.codigoIdentificacao, idOrganizao)){               
                this.OMensagemEmailExclusaoBL.excluir(OMensagemEmail.id);
            }
                                                                                                
            return this.inserir(OMensagemEmail);
            
        }
        
        //Persistir o objecto e salvar na base de dados
        private bool inserir(MensagemEmail OMensagemEmail) {
            
            OMensagemEmail.setDefaultInsertValues();

            db.MensagemEmail.Add(OMensagemEmail);

            db.SaveChanges();

            return (OMensagemEmail.id > 0);
        }
                

    }
}