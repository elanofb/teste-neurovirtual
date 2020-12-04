using System.Collections.Generic;
using FluentValidation.Attributes;

namespace WEB.Areas.ConfiguracoesAssociados.ViewModels{

    [Validator(typeof(AssociadoCampoClonagemFormValidator))]
	public class AssociadoCampoClonagemForm {

        //Atributos

        //Servicos

        //Propriedades
        public short? tipoCadastro { get; set; }
        
        public int? idTipoAssociadoOrigem { get; set; }
        
        public List<int> idsTiposAssociadoDestinos { get; set; }
       
        //Construtor
        public AssociadoCampoClonagemForm() {

            this.idTipoAssociadoOrigem = 0;
            this.idsTiposAssociadoDestinos = new List<int>();

        }
    }

}