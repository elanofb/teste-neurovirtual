using FluentValidation.Attributes;


namespace WEB.Areas.Permissao.ViewModels{
 
    //
    [Validator(typeof(AlterarSenhaValidator))]
    public class AlterarSenhaForm {

        public string senhaAtual { get; set; }

        public string senha { get; set; }

        public string confirmarSenha { get; set; }
    }

}
