using System;
using System.Security.Principal;
using BLL.Publicacoes;
using DAL.Permissao.Security.Extensions;
using FluentValidation;

namespace WEB.Areas.Publicacoes.ViewModels {

    //
    public class ConteudoFormValidation : AbstractValidator<ConteudoForm> {

        //Atributos
        private IConteudoConsultaBL _ConteudoConsultaBL;

        //Propriedades
        private IConteudoConsultaBL OConteudoConsultaBL => (this._ConteudoConsultaBL = this._ConteudoConsultaBL ?? new ConteudoConsultaBL());
        
        //Cont
        private IPrincipal User => HttpContextFactory.Current.User;
        
        //
        public ConteudoFormValidation() {
            
            RuleFor(x => x.Conteudo.titulo)
                .NotEmpty().WithMessage("Informe o título");
                        
            RuleFor(x => x.Conteudo.idInterno)
                .NotEmpty().WithMessage("Informe o ID Interno");
            
            RuleFor(x => x.Conteudo.conteudo)
                .NotEmpty().WithMessage("Insira o conteúdo");
            
            
            RuleFor(x => x.Conteudo.idInterno)
                .Must((x, titulo) => !this.existe(x))
                .WithMessage("Já existe uma página cadastrada com esse ID Interno.");
            
        }
                    
        //Validação de duplicados
        public bool existe(ConteudoForm ViewModel) {
                                    
            return this.OConteudoConsultaBL.existe(ViewModel.Conteudo.idInterno, ViewModel.Conteudo.id, User.idOrganizacao());
        }
        
    }
}