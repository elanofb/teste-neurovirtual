using FluentValidation;
using BLL.Publicacoes;

namespace WEB.Areas.Publicacoes.ViewModels {

    
    public  class ParceiroFormValidator : AbstractValidator<ParceiroForm> {

		//Atributos
		private IParceiroBL _ParceiroBL;

		//Propriedades
		private IParceiroBL OParceiroBL{ get{ return (this._ParceiroBL = this._ParceiroBL ?? new ParceiroBL() ); } }


        //Construtor
        public ParceiroFormValidator() {

            this.RuleFor(x => x.Parceiro.nome)
				.NotEmpty().WithMessage("Favor preencher o campo nome");

            this.RuleFor(x => x.Parceiro.link)
				.NotEmpty().WithMessage("Favor preencher o campo link");
			
			When(x => x.OArquivo != null, () => {
				
			});
        }

        //
        private bool existe(ParceiroForm ViewModel) {
            return this.OParceiroBL.existe(ViewModel.Parceiro.nome, ViewModel.Parceiro.id);
        }
    }
}