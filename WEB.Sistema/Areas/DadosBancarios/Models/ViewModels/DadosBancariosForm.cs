using System;
using System.Security.Principal;
using BLL.DadosBancarios.Interfaces;
using BLL.DadosBancarios.Services;
using DAL.DadosBancarios;
using FluentValidation.Attributes;

namespace WEB.Areas.DadosBancarios.ViewModels {
                    
    [Validator(typeof(DadosBancariosFormValidator))]
    public class DadosBancariosForm {
        
        // Atributos
        private IDadoBancarioConsultaBL _IDadoBancarioConsultaBL;
        private IDadoBancarioCadastroBL _IDadoBancarioCadastroBL;
        
        // Serviços
        private IDadoBancarioConsultaBL ODadoBancarioConsultaBL => _IDadoBancarioConsultaBL = _IDadoBancarioConsultaBL ?? new DadoBancarioConsultaBL();
        private IDadoBancarioCadastroBL ODadoBancarioCadastroBL => _IDadoBancarioCadastroBL = _IDadoBancarioCadastroBL ?? new DadoBancarioCadastroBL();
        
        // Propriedades
        public DadoBancario DadoBancario { get; set; }
        
        private IPrincipal User => HttpContextFactory.Current.User;
        
        public DadosBancariosForm(){
            this.DadoBancario = new DadoBancario();
        }
        
        public void carregarInformacoes(int id){

            if (id == 0){
                return;
            }
            
            this.DadoBancario = this.ODadoBancarioConsultaBL.carregar(id);
        }
        
        public bool salvar(){
            return ODadoBancarioCadastroBL.salvar(this.DadoBancario);
        }        
               
    }
}