using FluentValidation;
using BLL.Permissao;
using System.Linq;

namespace WEB.Areas.Permissao.ViewModels{
 
   //
    public class PerfilAcessoValidator : AbstractValidator<PerfilAcessoForm> {

            //Propriedades 
            private IPerfilAcessoBL _PerfilAcesso;

            //Atributos
            private IPerfilAcessoBL OPerfilAcesso => this._PerfilAcesso = this._PerfilAcesso ?? new PerfilAcessoBL();

            //Construtor
            public PerfilAcessoValidator() {

                RuleFor(x => x.PerfilAcesso.descricao)
                    .NotEmpty().WithMessage("Esse campo é obrigatório.")
                    .Must((x, descricao) => !this.existe(x))
                    .WithMessage("Já existe um registro cadastrado com essa descrição.");

            }

            private bool existe(PerfilAcessoForm OPerfilAcessoForm) {

                bool existePerfil = this.OPerfilAcesso.listar(0, "", "S")
                                                      .Any(x => x.descricao == OPerfilAcessoForm.PerfilAcesso.descricao
                                                            && x.id != OPerfilAcessoForm.PerfilAcesso.id);

                return existePerfil;
            }
        }
}
