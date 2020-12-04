using FluentValidation.Attributes;
using DAL.Permissao;
using DAL.Pessoas;

namespace WEB.Areas.Permissao.ViewModels{
 
    //
    [Validator(typeof(UsuarioSistemaValidator))]
    public class UsuarioSistemaForm {

        public UsuarioSistema UsuarioSistema { get; set; }
        public UsuarioSistema UsuarioSistemaLogado { get; set; }

        public bool flagLogAcesso { get; set; }

        //Construtor
        public UsuarioSistemaForm() {
            
            this.UsuarioSistema = new UsuarioSistema();

            this.UsuarioSistema.Pessoa = new Pessoa();
        }
    }

}
