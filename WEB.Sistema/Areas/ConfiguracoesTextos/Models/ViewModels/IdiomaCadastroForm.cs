using BLL.ConfiguracoesTextos;
using DAL.ConfiguracoesTextos;
using FluentValidation;
using FluentValidation.Attributes;

namespace WEB.Areas.ConfiguracoesTextos.ViewModels{

    [Validator(typeof(IdiomaCadastroFormValidator))]
	public class IdiomaCadastroForm {

        //Atributos
        private IIdiomaConsultaBL _IdiomaConsultaBL;

        //Propriedades
        private IIdiomaConsultaBL OIdiomaConsultaBL => this._IdiomaConsultaBL = this._IdiomaConsultaBL ?? new IdiomaConsultaBL();
        
        //Propriedades
		public Idioma Idioma { get; set; }

        //Construtor
        public IdiomaCadastroForm() { 
			this.Idioma = new Idioma();
        }

        public void carregar(int id) {
            this.Idioma = OIdiomaConsultaBL.carregar(id);
        }
    }

    internal class IdiomaCadastroFormValidator : AbstractValidator<IdiomaCadastroForm> {


        public IdiomaCadastroFormValidator(){
            
            RuleFor(x => x.Idioma.descricao).NotEmpty().WithMessage("Informe a descrição do idioma");
            RuleFor(x => x.Idioma.sigla).NotEmpty().WithMessage("Informe a sigla do idioma");
            
        }

    }
}