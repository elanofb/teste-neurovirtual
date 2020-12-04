using DAL.SegmentosAtuacao;
using FluentValidation.Attributes;

namespace WEB.Areas.SegmentosAtuacao.ViewModels {

	[Validator(typeof(SegmentoAtuacaoValidator))]
	public class SegmentoAtuacaoForm {

		//Propriedades
		public SegmentoAtuacao OSegmentoAtuacao { get; set; }

        //Construtor
        public SegmentoAtuacaoForm() { 
		}
		
	}
}