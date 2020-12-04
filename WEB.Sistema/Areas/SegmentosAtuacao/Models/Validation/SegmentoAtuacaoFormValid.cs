using System;
using BLL.SegmentosAtuacao;
using FluentValidation;

namespace WEB.Areas.SegmentosAtuacao.ViewModels{

    //
    public class SegmentoAtuacaoValidator : AbstractValidator<SegmentoAtuacaoForm> {
        
		//Atributos
		private ISegmentoAtuacaoConsultaBL _SegmentoAtuacaoConsultaBL; 

		//Propriedades
		private ISegmentoAtuacaoConsultaBL OSegmentoAtuacaoConsultaBL { get{ return (this._SegmentoAtuacaoConsultaBL = this._SegmentoAtuacaoConsultaBL ?? new SegmentoAtuacaoConsultaBL() ); }}

        //Construtor
        public SegmentoAtuacaoValidator() {
            
	        RuleFor(x => x.OSegmentoAtuacao.descricao).NotEmpty().WithMessage("Informe a descrição.");
	        
			When(x => !String.IsNullOrEmpty(x.OSegmentoAtuacao.descricao), () =>{
				RuleFor(x => x.OSegmentoAtuacao.descricao).Must( (x, descricao) => !this.existe(x) ).WithMessage("Já existe um segmento cadastrado com essa descrição.");
			});


        }

        //Verificar se existe outra empresa com o mesmo CNPJ
        public bool existe(SegmentoAtuacaoForm ViewModel) {
            int idDesconsiderado = UtilNumber.toInt32(ViewModel.OSegmentoAtuacao.id);
			return this.OSegmentoAtuacaoConsultaBL.existe(ViewModel.OSegmentoAtuacao.descricao, idDesconsiderado);
        }

    }
}
