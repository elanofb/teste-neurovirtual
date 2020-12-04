using FluentValidation.Attributes;
using DAL.Permissao;

namespace WEB.Areas.Permissao.ViewModels {

    [Validator(typeof(PerfilAcessoValidator))]
    public class PerfilAcessoForm {

        public PerfilAcesso PerfilAcesso { get; set; }

        public PerfilAcessoForm() {
            this.PerfilAcesso = new PerfilAcesso();
        }
      
    }
    
}