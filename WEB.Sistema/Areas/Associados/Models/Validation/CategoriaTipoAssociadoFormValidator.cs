using FluentValidation;
using BLL.Associados;
using System;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.Associados.ViewModels {

    public class CategoriaTipoAssociadoFormValidator : AbstractValidator<CategoriaTipoAssociadoForm> {

        //Atributos
        private ICategoriaTipoAssociadoBL _CategoriaTipoAssociadoBL;

        //Propriedades
        private ICategoriaTipoAssociadoBL OCategoriaTipoAssociadoBL => (this._CategoriaTipoAssociadoBL = this._CategoriaTipoAssociadoBL ?? new CategoriaTipoAssociadoBL());

        //Construtor
        public CategoriaTipoAssociadoFormValidator() {


            RuleFor(x => x.CategoriaTipoAssociado.descricao)
                .NotEmpty().WithMessage("Informe qual é o Título.")
                .Must((x, cpf) => !this.existe(x)).WithMessage("Já existe uma categoria com essa descrição.");
        }


        #region Validacoes Banco

        //Verificar existência de registro para evitar duplicidades  
        private bool existe(CategoriaTipoAssociadoForm ViewModel) {
            int idDesconsiderado = ViewModel.CategoriaTipoAssociado.id;

            int? idOrganizacao = ViewModel.CategoriaTipoAssociado.idOrganizacao;
            if (HttpContextFactory.Current.User.idOrganizacao() > 0) {
                idOrganizacao = HttpContextFactory.Current.User.idOrganizacao();
            }

            bool flagExiste = this.OCategoriaTipoAssociadoBL.existe(ViewModel.CategoriaTipoAssociado.descricao, idDesconsiderado, idOrganizacao);
            return flagExiste;
        }

        #endregion
    }
}