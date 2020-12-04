using System.Linq;
using System.Web.Mvc;
using DAL.Financeiro;
using FluentValidation;
using FluentValidation.Attributes;

namespace WEB.Areas.Financeiro.ViewModels {

    [Validator(typeof(DespesaPagamentoExcluirFormValidator))]
    public class DespesaPagamentoExcluirForm {

        public TituloDespesaPagamento TituloDespesaPagamento { get; set; }

        public string flagAtualizarOutros { get; set; }

        public bool flagHabilitarAtualizarTodos { get; set; }

        public bool flagHabilitarAtualizarProximos { get; set; }

        public SelectList selectList(string selected) {
            var list = new[] {
                flagHabilitarAtualizarTodos ? new{value = "ALL", text = "todos os pagamentos da despesa"} : null,
                flagHabilitarAtualizarProximos ? new{value = "NEXT", text = "este e todos após o mesmo"} : null
            };

            var lista = list.Where(x => x != null).ToList();

            return new SelectList(lista, "value", "text", selected);
        }
    }

    internal class DespesaPagamentoExcluirFormValidator : AbstractValidator<DespesaPagamentoExcluirForm> {
        public DespesaPagamentoExcluirFormValidator() {
            RuleFor(x => x.TituloDespesaPagamento.motivoExclusao)
                .NotEmpty().WithMessage("Informe o motivo da exclusão.");
        }
    }
}