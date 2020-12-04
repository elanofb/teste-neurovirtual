using DAL.Configuracoes;

using FluentValidation.Attributes;

using WEB.Areas.Configuracao.ViewModels;

namespace WEB.Areas.ConfiguracoesTextos.Models.Forms {

    [Validator(typeof(ConfiguracaoPromocaoFormValidator))]
    public class ConfiguracaoPromocaoForm {
        public ConfiguracaoPromocao ConfiguracaoPromocao { get; set; }
    }

}