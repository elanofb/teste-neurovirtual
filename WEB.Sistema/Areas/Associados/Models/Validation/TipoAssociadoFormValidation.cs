using FluentValidation;
using BLL.Associados;
using System;
using DAL.Permissao.Security.Extensions;

namespace WEB.Areas.Associados.ViewModels {

    public class TipoAssociadoFormValidator : AbstractValidator<TipoAssociadoForm> {

        //Atributos
        private ITipoAssociadoBL _TipoAssociadoBL;

        //Propriedades
        private ITipoAssociadoBL OTipoAssociadoBL => (this._TipoAssociadoBL = this._TipoAssociadoBL ?? new TipoAssociadoBL());

        //Construtor
        public TipoAssociadoFormValidator() {

            this.validarDados();
        }

        //Validacao para dados pessoais
        private void validarDados() {

            RuleFor(x => x.TipoAssociado.descricao)
                .NotEmpty().WithMessage("Informe qual é o Título.")
                .Must((x, cpf) => !this.existe(x)).WithMessage("Já existe um título com essa descrição.");

            RuleFor(x => x.TipoAssociado.flagIsento)
                .NotNull().WithMessage("Informe se esse tipo de associado será isento das contribuições.");

            RuleFor(x => x.TipoAssociado.valorTaxaInscricao)
                .GreaterThanOrEqualTo(0).WithMessage("Informe um valor válido.");

            RuleFor(x => x.TipoAssociado.diasPrazoPrimeiraCobranca)
                .GreaterThanOrEqualTo((byte)0).WithMessage("O valor informado está incorreto.")
                .LessThanOrEqualTo((byte)100).WithMessage("O prazo deve ser menor do que o informado.");

            When(x => x.TipoAssociado.valorTaxaInscricao > 0, () => {

                RuleFor(x => x.TipoAssociado.diasPrazoTaxaInscricao)
                    .GreaterThanOrEqualTo((byte)0).WithMessage("O valor informado está incorreto.")
                    .LessThanOrEqualTo((byte)100).WithMessage("O prazo deve ser menor do que o informado.");
            });
        }

        #region Validacoes Banco

        //Verificar existencia de registro para evitar duplicidades  
        private bool existe(TipoAssociadoForm ViewModel) {
            int idDesconsiderado = ViewModel.TipoAssociado.id;

            int? idOrganizacao = ViewModel.TipoAssociado.idOrganizacao;
            if (HttpContextFactory.Current.User.idOrganizacao() > 0) {
                idOrganizacao = HttpContextFactory.Current.User.idOrganizacao();
            }

            bool flagExiste = this.OTipoAssociadoBL.existe(ViewModel.TipoAssociado.descricao, UtilNumber.toInt32(ViewModel.TipoAssociado.idCategoria), idDesconsiderado, idOrganizacao);
            return flagExiste;
        }

        #endregion
    }
}