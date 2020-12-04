using System;
using FluentValidation;
using BLL.Profissoes;

namespace WEB.Areas.Profissoes.ViewModels{

    //
    public class ProfissaoValidator : AbstractValidator<ProfissaoForm> {
        
		//Atributos
		private IProfissaoConsultaBL _ProfissaoConsultaBL; 

		//Propriedades
		private IProfissaoConsultaBL OProfissaoConsultaBL { get{ return (this._ProfissaoConsultaBL = this._ProfissaoConsultaBL ?? new ProfissaoConsultaBL() ); }}

        //Construtor
        public ProfissaoValidator() {
            
            RuleFor(x => x.OProfissao.descricao).NotEmpty().WithMessage("Informe a descrição.");

	        When(x => !String.IsNullOrEmpty(x.OProfissao.descricao), () =>{

		        RuleFor(x => x.OProfissao.descricao)
			        .Must( (x, descricao) => !this.existe(x) ).WithMessage("Essa descrição já está sendo usado por outra profissão.");
	        });


        }

        //Verificar se existe outra empresa com o mesmo CNPJ
        public bool existe(ProfissaoForm ViewModel) {
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.OProfissao.id);
			return this.OProfissaoConsultaBL.existe(ViewModel.OProfissao.descricao, idDesconsiderado);
        }

    }
}
