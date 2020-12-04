using FluentValidation;
using BLL.OrgaosClasses;

namespace WEB.Areas.OrgaosClasses.ViewModels {

    //
    public class OrgaoClasseFormValidator : AbstractValidator<OrgaoClasseForm> {

        //Atributos
        private IOrgaoClasseBL _OrgaoClasseBL;

        //Propriedades
        private IOrgaoClasseBL OOrgaoClasseBL => _OrgaoClasseBL = _OrgaoClasseBL ?? new OrgaoClasseBL();

        //
        public OrgaoClasseFormValidator() {

            RuleFor(x => x.OrgaoClasse.descricao)
                .NotEmpty().WithMessage("Esse campo é obrigatório.")
                .Must( (x, descricao) => !this.existe(x))
                .WithMessage("Já existe um registro cadastrado com essa descrição.");
        }

        //
        private bool existe(OrgaoClasseForm ViewModel) {

            return OOrgaoClasseBL.existe(ViewModel.OrgaoClasse, ViewModel.OrgaoClasse.id);

        }
    }
}