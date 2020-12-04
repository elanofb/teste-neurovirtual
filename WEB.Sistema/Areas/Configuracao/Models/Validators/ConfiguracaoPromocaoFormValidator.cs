using System;
using BLL.Financeiro;
using FluentValidation;

using WEB.Areas.ConfiguracoesTextos.Models.Forms;

namespace WEB.Areas.Configuracao.ViewModels {
    
    public class ConfiguracaoPromocaoFormValidator : AbstractValidator<ConfiguracaoPromocaoForm> {
        
        public ConfiguracaoPromocaoFormValidator() {
            
            RuleFor(x => x.ConfiguracaoPromocao.dtInicioPremioNovoMembro)
                .NotEmpty().WithMessage("Data de Inicio deve ser informada!");
            
            RuleFor(x => x.ConfiguracaoPromocao.dtFimPremioNovoMembro)
                .NotEmpty().WithMessage("Data de Término deve ser informada!");
            
            RuleFor(x => x.ConfiguracaoPromocao.valorPremioNovoMembro)
                .NotEmpty().WithMessage("O valor do prêmio deverá ser informada!");
        }
    }
}