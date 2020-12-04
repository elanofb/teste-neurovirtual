using System.Linq;
using System.Web.Mvc;
using DAL.Financeiro;
using FluentValidation;
using FluentValidation.Attributes;

namespace WEB.Areas.Financeiro.ViewModels {

    [Validator(typeof(ReceitaPagamentoExcluirFormValidator))]
    public class ReceitaPagamentoExcluirForm {

        public TituloReceitaPagamento TituloReceitaPagamento { get; set; }

        public string flagAtualizarOutros { get; set; }

        public bool flagHabilitarAtualizarTodos { get; set; }

        public bool flagHabilitarAtualizarProximos { get; set; }

        public SelectList selectList(string selected) {
            var list = new[] {
                flagHabilitarAtualizarTodos ? new{value = "ALL", text = "todos os pagamentos da receita"} : null,
                flagHabilitarAtualizarProximos ? new{value = "NEXT", text = "este e todos após o mesmo"} : null
            };

            var lista = list.Where(x => x != null).ToList();

            return new SelectList(lista, "value", "text", selected);
        }
    }

    internal class ReceitaPagamentoExcluirFormValidator : AbstractValidator<ReceitaPagamentoExcluirForm> {
        public ReceitaPagamentoExcluirFormValidator() {
            RuleFor(x => x.TituloReceitaPagamento.motivoExclusao)
                .NotEmpty().WithMessage("Informe o motivo da exclusão.");
        }
    }
}