using System;
using FluentValidation;
using BLL.Associados;

namespace WEB.Areas.Associados.ViewModels{

    //
    public class AssociadoInstituicaoFormValidator : AbstractValidator<AssociadoInstituicaoForm> {
        
		//Atributos
		private IAssociadoInstituicaoBL _AssociadoInstituicaoBL; 

		//Propriedades
		private IAssociadoInstituicaoBL OAssociadoInstituicaoBL { get{ return (this._AssociadoInstituicaoBL = this._AssociadoInstituicaoBL ?? new AssociadoInstituicaoBL() ); }}

        //Construtor
        public AssociadoInstituicaoFormValidator() {
            
            RuleFor(x => x.AssociadoInstituicao.idAssociado)
				.NotEmpty().WithMessage("Informe a qual associado será vinculado a instituição.");

            RuleFor(x => x.AssociadoInstituicao.idInstituicao)
				.GreaterThan(0).WithMessage("Informe a instituição.")
				.Must((x, idInstituicao) => !this.existe(x) ).WithMessage("Já há um registro cadastrado com essas informações.");

        }

        //Verificar se o contato já existe
        public bool existe(AssociadoInstituicaoForm ViewModel) {

            int idDesconsiderado = UtilNumber.toInt32(ViewModel.AssociadoInstituicao.id);

            return this.OAssociadoInstituicaoBL.existe(ViewModel.AssociadoInstituicao, idDesconsiderado);
        }

    }
}
