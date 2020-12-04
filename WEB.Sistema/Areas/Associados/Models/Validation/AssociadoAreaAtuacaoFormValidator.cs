using System;
using FluentValidation;
using BLL.Associados;

namespace WEB.Areas.Associados.ViewModels{

    //
    public class AssociadoAreaAtuacaoFormValidator : AbstractValidator<AssociadoAreaAtuacaoForm> {
        
		//Atributos
		private IAssociadoAreaAtuacaoBL _AssociadoAreaAtuacaoBL; 

		//Propriedades
		private IAssociadoAreaAtuacaoBL OAssociadoAreaAtuacaoBL { get{ return (this._AssociadoAreaAtuacaoBL = this._AssociadoAreaAtuacaoBL ?? new AssociadoAreaAtuacaoBL() ); }}

        //Construtor
        public AssociadoAreaAtuacaoFormValidator() {
            
            RuleFor(x => x.AssociadoAreaAtuacao.idAssociado)
				.NotEmpty().WithMessage("Informe a qual associado será vinculado o AreaAtuacao.");

            RuleFor(x => x.AssociadoAreaAtuacao.idAreaAtuacao)
				.GreaterThan(0).WithMessage("Informe a atividade.")
				.Must((x, idAreaAtuacao) => !this.existe(x) ).WithMessage("Já há um registro cadastrado com essas informações.");

        }

        //Verificar se o contato já existe
        public bool existe(AssociadoAreaAtuacaoForm ViewModel) {
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.AssociadoAreaAtuacao.id);
			return this.OAssociadoAreaAtuacaoBL.existe(ViewModel.AssociadoAreaAtuacao, idDesconsiderado);
        }

    }
}
