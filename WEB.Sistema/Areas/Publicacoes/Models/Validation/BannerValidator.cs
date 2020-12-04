using FluentValidation;
//using UTIL.Upload;

namespace WEB.Areas.Publicacoes.ViewModels {
	//
    public class BannerValidator : AbstractValidator<BannerForm> {

        //Construtor
		public BannerValidator() {

			RuleFor(x => x.Banner.posicao)
				.NotEmpty().WithMessage("Informe qual é a posição que o banner deve ficar.");
			
			RuleFor(x => x.Banner.flagBlank)
				.NotEmpty().WithMessage("Informe se o link do banner deve abrir em uma nova janela.");

			When(x => x.Banner.dtInicioExibicao.HasValue && x.Banner.dtFimExibicao.HasValue, () => {
				
				RuleFor(x => x.Banner.dtFimExibicao)
					.GreaterThan(x => x.Banner.dtInicioExibicao).WithMessage("A data final para exibição do banner deve ser superior à data inicial.");	
				
			});
			
        }

    }
	
}