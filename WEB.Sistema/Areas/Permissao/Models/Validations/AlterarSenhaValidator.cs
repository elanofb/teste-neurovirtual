using System;
using FluentValidation;
using BLL.Permissao;
using System.Linq;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.Permissao.ViewModels{
 
   //
    public class AlterarSenhaValidator : AbstractValidator<AlterarSenhaForm> {
        
        //Atributos
        private IUsuarioSistemaBL _UsuarioSistemaBL;

        //Propriedades
        private IUsuarioSistemaBL OUsuarioSistemaBL => _UsuarioSistemaBL = _UsuarioSistemaBL ?? new UsuarioSistemaBL();

        //
        public AlterarSenhaValidator() {

            RuleFor(x => x.senhaAtual)
                .NotEmpty().WithMessage("Informe a sua senha atual")
                .Must( (x, senhaAtual) => verificarSenha(x) ).WithMessage("A senha atual está incorreta");

            RuleFor(x => x.senha)
                .NotEmpty().WithMessage("Informe a nova senha")
                .Length(4, 12).WithMessage("A senha deve ter no mínimo 4 e no máximo 12 caracteres");

            RuleFor(x => x.confirmarSenha)
                .Must((x, confirmarSenha) => confirmarSenha == x.senha).WithMessage("A confirmação da senha está incorreta");

        }

        //
        public bool verificarSenha(AlterarSenhaForm ViewModel) {

            int idUsuario = HttpContextFactory.Current.User.id();

            string password = UtilCrypt.SHA512(ViewModel.senhaAtual);

            bool flagExiste = OUsuarioSistemaBL.listar(0, "", "S").Any(x => x.id == idUsuario && x.senha == password);

            return flagExiste;
        }

    }
}
