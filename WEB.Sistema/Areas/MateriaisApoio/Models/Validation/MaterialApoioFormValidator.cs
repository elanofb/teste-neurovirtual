using FluentValidation;
using BLL.MateriaisApoio;

namespace WEB.Areas.MateriaisApoio.ViewModels{

    //
    public class MaterialApoioFormValidator : AbstractValidator<MaterialApoioForm> {
        
		//Atributos
		private IMaterialApoioBL _MaterialApoioBL; 

		//Propriedades
		private IMaterialApoioBL OMaterialApoioBL { get{ return (this._MaterialApoioBL = this._MaterialApoioBL ?? new MaterialApoioBL() ); }}

        //Construtor
        public MaterialApoioFormValidator() {

            RuleFor(x => x.MaterialApoio.titulo)
				.NotEmpty().WithMessage("Informe o título do material.");

            RuleFor(x => x.OArquivo)
				.NotNull().When(x => x.MaterialApoio.id == 0).WithMessage("Selecione um arquivo");
			
			When(x => x.OArquivo != null, () => {

				RuleFor(x => x.OArquivo)
					.Must( (x, OArquivo) => OArquivo.ContentLength <= 20971520).WithMessage("O Arquivo deve possuir no máximo 20MB.");
				
			});
        }
    }
}
