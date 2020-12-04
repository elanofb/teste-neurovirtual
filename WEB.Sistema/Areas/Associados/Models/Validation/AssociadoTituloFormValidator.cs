using System;
using FluentValidation;
using BLL.Associados;

namespace WEB.Areas.Associados.ViewModels {

    //
    public class AssociadoTituloFormValidator : AbstractValidator<AssociadoTituloForm> {

        //Atributos
        private IAssociadoTituloBL _AssociadoTituloBL;

        //Propriedades
        private IAssociadoTituloBL OAssociadoTituloBL => (this._AssociadoTituloBL = this._AssociadoTituloBL ?? new AssociadoTituloBL());

        //Construtor
        public AssociadoTituloFormValidator() {

            RuleFor(x => x.AssociadoTitulo.idAssociado)
                .NotEmpty().WithMessage("Informe a qual associado será vinculado ao título.");

            RuleFor(x => x.AssociadoTitulo.idTipoTitulo)
                .GreaterThan(0).WithMessage("Informe qual é o título/certificação.");

            RuleFor(x => x.AssociadoTitulo.dtAquisicao)
                .NotEmpty().WithMessage("Informe a data de aquisição do título.");

            When(x => x.AssociadoTitulo.dtAquisicao != DateTime.MinValue, () => {

                RuleFor(x => x.AssociadoTitulo.dtAquisicao)
                    .GreaterThan(new DateTime(1920, 1, 1)).WithMessage("Informe uma data válida da aquisição do título.");
            });

            When(x => x.AssociadoTitulo.dtProximaRenovacao != DateTime.MinValue, () => {

                RuleFor(x => x.AssociadoTitulo.dtProximaRenovacao)
                    .GreaterThan(x => x.AssociadoTitulo.dtAquisicao).WithMessage("A data de renovação não pode ser menor do que a data de aquisição.");
            });
        }

        //Verificar se o contato já existe
        public bool existe(AssociadoTituloForm ViewModel) {
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.AssociadoTitulo.id);
            return this.OAssociadoTituloBL.existe(ViewModel.AssociadoTitulo, idDesconsiderado);
        }

    }
}
