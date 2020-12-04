using System;
using FluentValidation;
using BLL.Associados;

namespace WEB.Areas.Associados.ViewModels{

    //
    public class AssociadoAbrangenciaFormValidator : AbstractValidator<AssociadoAbrangenciaForm> {
        
		//Atributos
		private IAssociadoAbrangenciaBL _AssociadoAbrangenciaBL; 

		//Propriedades
		private IAssociadoAbrangenciaBL OAssociadoAbrangenciaBL { get{ return (this._AssociadoAbrangenciaBL = this._AssociadoAbrangenciaBL ?? new AssociadoAbrangenciaBL() ); }}

        //Construtor
        public AssociadoAbrangenciaFormValidator() {
            
            RuleFor(x => x.AssociadoAbrangencia.idAssociado)
				.NotEmpty().WithMessage("Informe a qual associado será vinculado o Abrangencia.");

            RuleFor(x => x.AssociadoAbrangencia.idCidade)
				.GreaterThan(0).WithMessage("Informe a cidade atendida.")
				.Must((x, idCidade) => !this.existe(x) ).WithMessage("Já há um registro cadastrado com essas informações.");


        }

        //Verificar se o contato já existe
        public bool existe(AssociadoAbrangenciaForm ViewModel) {
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.AssociadoAbrangencia.id);
			return this.OAssociadoAbrangenciaBL.existe(ViewModel.AssociadoAbrangencia, idDesconsiderado);
        }

    }
}
