using DAL.Contatos;
using FluentValidation.Attributes;

namespace WEB.Areas.Contatos.ViewModels {

    [Validator(typeof(TipoContatoFormValidator))]
    public class TipoContatoForm {

        public TipoContato TipoContato { get; set; }

        public bool flagRecarregar { get; set; }
        
        public string idComboSelecionar { get; set; }

        //
        public TipoContatoForm() {

        }
        
    }

}