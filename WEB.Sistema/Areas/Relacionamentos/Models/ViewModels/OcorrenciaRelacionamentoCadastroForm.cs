using DAL.Relacionamentos;
using FluentValidation.Attributes;

namespace WEB.Areas.Relacionamentos.ViewModels {

    [Validator(typeof(OcorrenciaRelacionamentoCadastroFormValidator))]
    public class OcorrenciaRelacionamentoCadastroForm {

        public OcorrenciaRelacionamento OcorrenciaRelacionamento { get; set; }

        public bool flagRecarregar { get; set; }
        
        public string idComboSelecionar { get; set; }

        //
        public OcorrenciaRelacionamentoCadastroForm() {

        }
        
    }

}