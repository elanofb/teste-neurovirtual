using System;
using FluentValidation;
using BLL.Permissao;
using System.Linq;

namespace WEB.Areas.Permissao.ViewModels{

    //
    public class UsuarioSistemaValidator : AbstractValidator<UsuarioSistemaForm> {

        //Atributos
        private IUsuarioSistemaBL _UsuarioSistemaBL;

        //Propriedades
        private IUsuarioSistemaBL OUsuarioSistemaBL => this._UsuarioSistemaBL = this._UsuarioSistemaBL ?? new UsuarioSistemaBL();
        
        //Construtor
        public UsuarioSistemaValidator() {

            RuleFor(x => x.UsuarioSistema.Pessoa.nome)
                .NotEmpty().WithMessage("Informe o nome do usuário");

            RuleFor(x => x.UsuarioSistema.idPerfilAcesso)
                .NotEmpty().WithMessage("Informe o perfil de acesso do usuário.");

            RuleFor(x => x.UsuarioSistema.idOrganizacao)
                .NotEmpty().WithMessage("Informe a Associação do usuário.");

            RuleFor(x => x.UsuarioSistema.Pessoa.emailPrincipal)
                .NotEmpty().WithMessage("Informe o e-mail do usuário")
                .EmailAddress().WithMessage("O e-mail informado é inválido");

            RuleFor(x => x.UsuarioSistema.login)
                .NotEmpty().WithMessage("Informe o login do usuário")
                .Must( (x, login) => !exists(x, login) ).WithMessage("O login informado já existe para outro usuário.");

            When(x => !String.IsNullOrEmpty(x.UsuarioSistema.Pessoa.nroDocumento), () => {

                RuleFor(x => x.UsuarioSistema.Pessoa.nroDocumento)
                .Must((model, value) => UtilValidation.isCPFCNPJ(value))
                .WithMessage("O número do CPF está inválido");
            });

            When(x => x.UsuarioSistema.dtInicioDegustacao != null, () => {

                RuleFor(x => x.UsuarioSistema.dtFimDegustacao)
                    .NotEmpty().WithMessage("Informe a data final da degustação");

            });

            When(x => x.UsuarioSistema.dtFimDegustacao != null, () => {

                RuleFor(x => x.UsuarioSistema.dtInicioDegustacao)
                    .NotEmpty().WithMessage("Informe a data início da degustação");

            });


        }

        //
        public bool exists(UsuarioSistemaForm User, string login) {

            int currentId = User.UsuarioSistema.id;
            bool existeUsuario = this.OUsuarioSistemaBL.listar(0, "", "S")
                                                       .Any(x => x.login == login && x.id != currentId);
            return existeUsuario;
        }
    }
}
