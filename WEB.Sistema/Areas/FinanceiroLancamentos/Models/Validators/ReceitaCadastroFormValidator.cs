using System;
using System.Linq;
using BLL.FinanceiroLancamentos;
using DAL.Financeiro;
using FluentValidation;

namespace WEB.Areas.FinanceiroLancamentos.ViewModels {

    public class ReceitaCadastroFormValidator : AbstractValidator<ReceitaCadastroForm> {

        //Atributos
        private DevedorVWBL _DevedorVWBL;

        //Propriedades
        private DevedorVWBL ODevedorVWBL => _DevedorVWBL = _DevedorVWBL ?? new DevedorVWBL();

        public ReceitaCadastroFormValidator() {

            RuleFor(x => x.TituloReceita.descricao)
                .NotEmpty().WithMessage("Titulo é obrigatória.");

            RuleFor(x => x.idReferenciaPessoa)
                .NotEmpty().WithMessage("Informe o Devedor.")
                .Must((x, credor) => this.verificarDevedor(x.idReferenciaPessoa)).WithMessage("O devedor informado não pode ser localizado");

            RuleFor(x => x.flagTipoRepeticao)
                .NotEmpty().WithMessage("Informe o tipo de repetição");

            When(x => x.flagTipoRepeticao == TipoRepeticaoConst.NENHUMA, () => {
                RuleFor(x => x.TituloReceita.dtVencimento)
                    .NotEmpty().WithMessage("Informe a data de vencimento");

                RuleFor(x => x.TituloReceita.valorTotal)
                    .NotEmpty().WithMessage("Informe o valor da despesa")
                    .GreaterThan(0).WithMessage("O valor da despesa deve ser maior que zero");
            });

            When(x => x.flagTipoRepeticao == TipoRepeticaoConst.PARCELAMENTO, () => {

                RuleFor(x => x.TituloReceita.qtdeRepeticao)
                    .NotEmpty().WithMessage("Informe a Qntd. de parcelas.")
                    .GreaterThan(1).WithMessage("A quantidade de parcelas não pode ser menor que 2.");

                RuleFor(x => x.TituloReceita.qtdeRepeticao)
                    .Must((x, parcelas) => (x.TituloReceita.listaTituloReceitaPagamento?.Count == x.TituloReceita.qtdeRepeticao))
                    .WithMessage("Não foi registrado todas as parcelas");

                When(x => x.flagValorTotalParcelamento == "S", () => {
                    RuleFor(x => x.TituloReceita.valorTotal)
                        .NotEmpty().WithMessage("Informe o valor total da despesa")
                        .GreaterThan(0).WithMessage("O valor total da despesa deve ser maior que zero");

                    RuleFor(x => x.TituloReceita.qtdeRepeticao)
                        .Must((x, parcelas) => (x.TituloReceita.listaTituloReceitaPagamento.Sum(y => y.valorOriginal) == x.TituloReceita.valorTotal))
                        .WithMessage(x => $"O valor total de {x.TituloReceita.valorTotal} difere da soma das parcelas de {x.TituloReceita.listaTituloReceitaPagamento.Sum(y => y.valorOriginal).ToString("C")}");
                });

                When(x => x.flagValorTotalParcelamento != "S", () => {
                    RuleFor(x => x.valorParcelas)
                        .NotEmpty().WithMessage("Informe o valor da parcela")
                        .GreaterThan(0).WithMessage("O valor da parcela deve ser maior que zero");
                });

                When(x => x.TituloReceita.dtVencimento != null, () => {
                    RuleFor(x => x.TituloReceita.qtdeRepeticao)
                        .Must((x, parcelas) => !(x.TituloReceita.listaTituloReceitaPagamento.Any(y => y.dtVencimento < x.TituloReceita.dtVencimento)))
                        .WithMessage("Não pode haver uma parcela com Dt. Vencimento menor que a data do primeiro vencimento");
                });
            });
        }

        /// <summary>
        /// Verifica se o credor informado existe de acordo com sua categoria
        /// </summary>
        private bool verificarDevedor(string idReferenciaPessoa) {

            if (string.IsNullOrEmpty(idReferenciaPessoa)) {
                return false;
            }

            var array = idReferenciaPessoa.Split('#');
            var flagCategoriaPessoa = array[0];
            var idPessoa = Convert.ToInt32(array[1]);

            return this.ODevedorVWBL.listar("").Any(x => x.flagCategoriaPessoa == flagCategoriaPessoa && x.idPessoa == idPessoa);
        }
    }
}
