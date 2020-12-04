using FluentValidation;
using BLL.Publicacoes;

namespace WEB.Areas.Publicacoes.ViewModels {

    //
    public class RevistaFormValidator : AbstractValidator<RevistaForm> {

		//Atributos
		private IPublicacaoBL _RevistaBL;

		//Propriedades
		private IPublicacaoBL ORevistaBL { get { return (this._RevistaBL = this._RevistaBL ?? new RevistaBL() ); } }

        //Construtor
        public RevistaFormValidator() {

            RuleFor(x => x.Revista.titulo)
				.NotEmpty().WithMessage("Informe o título/edição da revista")
				.Must( (x, titulo) => !this.existe(x) ).WithMessage("Já há uma revista cadastrada com esse título/edição.");

            RuleFor(x => x.Revista.descricao)
				.NotEmpty().WithMessage("Insira o conteúdo para exibição online da revista");

			When(x => x.Revista.id == 0, () => {

				RuleFor(x => x.Foto)
					.NotNull().WithMessage("Insira uma imagem de ilustração para a revista.")
					.Must( (x, Foto) => UTIL.Upload.UploadConfig.validarImagem(Foto)).WithMessage("Informe uma imagem válida para a ilustração.");
			});
        }

		//
		private bool existe(RevistaForm ViewModel){
			return this.ORevistaBL.existe(ViewModel.Revista);
		}
    }
}