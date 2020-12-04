using System;
using BLL.Financeiro;
using FluentValidation;

namespace WEB.Areas.Configuracao.ViewModels {

    //
    public class ConfiguracaoContribuicaoFormValidator : AbstractValidator<ConfiguracaoContribuicaoForm> {

        private ICentroCustoBL _CentroCustoBL;

        private ICentroCustoBL OCentroCustoBL => _CentroCustoBL = _CentroCustoBL ?? new CentroCustoBL();

        //
        public ConfiguracaoContribuicaoFormValidator() {

            When(x => x.ConfiguracaoContribuicao.idCentroCustoPadrao > 0, () => {
                RuleFor(x => x.ConfiguracaoContribuicao.idCentroCustoPadrao)
                    .Must((x, id) => this.checkCentroCustoPadrao(x.ConfiguracaoContribuicao.idCentroCustoPadrao))
                    .WithMessage("Não foi possível salvar com esse centro de custo");
            });

        }

        private bool checkCentroCustoPadrao(int? idCentroCusto) {
            return OCentroCustoBL.carregar(idCentroCusto.toInt()).id > 0;
        }
    }
}