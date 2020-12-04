using System;
using System.Linq;
using System.Security.Principal;
using BLL.DadosBancarios.Interfaces;
using BLL.DadosBancarios.Services;
using DAL.Permissao.Security.Extensions;
using FluentValidation;

namespace WEB.Areas.DadosBancarios.ViewModels {

    //
    public class DadosBancariosFormValidator : AbstractValidator<DadosBancariosForm> {
        
        // Atributos
        private IDadoBancarioConsultaBL _IDadoBancarioConsultaBL;        
        
        // Serviços
        private IDadoBancarioConsultaBL ODadoBancarioConsultaBL => _IDadoBancarioConsultaBL = _IDadoBancarioConsultaBL ?? new DadoBancarioConsultaBL();
        
        private IPrincipal User => HttpContextFactory.Current.User;
                
        public DadosBancariosFormValidator() {
                        
            RuleFor(x => x.DadoBancario.idPessoa)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Informe a pessoa vinculada a conta.");
            
            RuleFor(x => x.DadoBancario.idBanco)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Informe o Banco.");
            
            RuleFor(x => x.DadoBancario.flagTipoConta).NotEmpty().WithMessage("Informe o tipo da conta.");
            
            RuleFor(x => x.DadoBancario.nroAgencia).NotEmpty().WithMessage("Informe o número da agência.");
            
            RuleFor(x => x.DadoBancario.nroConta).NotEmpty().WithMessage("Informe o número da conta.");
            
            RuleFor(x =>x.DadoBancario.idBanco)
                .Must( (x, idBanco) => !this.existe(x) )
                .WithMessage("Já existe uma conta bancária cadastrada com essas informações.");
                                    
        }
        
        public bool existe(DadosBancariosForm ViewModel){
            
            int idDesconsiderado = ViewModel.DadoBancario.id.toInt();
            
            return this.ODadoBancarioConsultaBL.query(User.idOrganizacao())
                .Any(x => x.idBanco == ViewModel.DadoBancario.idBanco 
                          && x.flagTipoConta == ViewModel.DadoBancario.flagTipoConta 
                          && x.nroConta == ViewModel.DadoBancario.nroConta 
                          && x.idPessoa == ViewModel.DadoBancario.idPessoa
                          && x.id != idDesconsiderado);

        }
        
    }
}