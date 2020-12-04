using FluentValidation;

namespace WEB.Areas.Planos.ViewModels {

    
    public  class PlanoAnuncioFormValidator : AbstractValidator<PlanoAnuncioForm> {

		//Atributos
		private IAnuncioBL _AnuncioBL;

		//Propriedades
		private IAnuncioBL OAnuncioBL{ get{ return (this._AnuncioBL = this._AnuncioBL ?? new AnuncioBL() ); } }


        //Construtor
        public PlanoAnuncioFormValidator() {

            this.RuleFor(x => x.Anuncio.titulo)
				.NotEmpty().WithMessage("Favor preencher o campo nome");

            this.RuleFor(x => x.Anuncio.telefone)
				.NotEmpty().WithMessage("Favor preencher o campo telefone");

            this.RuleFor(x => x.Anuncio.email)
				.NotEmpty().WithMessage("Favor preencher o campo e-mail");
			
			When(x => x.OArquivo != null, () => {
				
			});
        }

        //
        private bool existe(PlanoAnuncioForm ViewModel) {
            return this.OAnuncioBL.existe(ViewModel.Anuncio.titulo, ViewModel.Anuncio.id);
        }
    }
}