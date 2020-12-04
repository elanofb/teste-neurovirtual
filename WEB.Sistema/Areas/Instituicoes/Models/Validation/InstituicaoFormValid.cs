using System;
using FluentValidation;
using BLL.Instituicoes;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.Instituicoes.ViewModels {

    //
    public  class InstituicaoValidator : AbstractValidator<InstituicaoForm> {

		//Atributos
		private IInstituicaoBL _InstituicaoBL;

		//Propriedades
		private IInstituicaoBL OInstituicaoBL => this._InstituicaoBL = this._InstituicaoBL ?? new InstituicaoBL();

        //Construtor
        public InstituicaoValidator() {

            RuleFor(x => x.Instituicao.descricao)
                .NotEmpty().WithMessage("O nome da instituição é obrigatório.")
                .Must((x, descricao) => !this.existe(x))
                .WithMessage("Já existe uma instituição cadastrada com esse nome.");

        }

        //
        private bool existe(InstituicaoForm ViewModel){

            bool flagExiste = this.OInstituicaoBL.existe(ViewModel.Instituicao.descricao, HttpContextFactory.Current.User.idOrganizacao(), ViewModel.Instituicao.id);

            return flagExiste;
        }
    }
}