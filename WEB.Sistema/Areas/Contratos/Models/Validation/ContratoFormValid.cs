using FluentValidation;
using BLL.Contratos;

//using UTIL.Upload;

namespace WEB.Areas.Contratos.ViewModels {
        
    //
    public class ContratoValidator : AbstractValidator<ContratoForm> {

        //Atributos
        private IContratoBL _ContratoBL;

        //Propriedades
        private IContratoBL OContratoBL { get { return (this._ContratoBL = this._ContratoBL ?? new ContratoBL()); } }

        //
        public ContratoValidator() {

            RuleFor(x => x.Contrato.idTipoContrato)
                .GreaterThan(0).WithMessage("Informe o tipo do contrato.");

            RuleFor(x => x.Contrato.idFornecedor)
                .GreaterThan(0).WithMessage("Informe o contratado.");

            RuleFor(x => x.Contrato.titulo)
                .NotEmpty().WithMessage("O título do contrato é obrigatório.")
                .Must((x, descricao) => !this.existe(x))
                .WithMessage("Já existe um contrato cadastrado com esse título.");

            RuleFor(x => x.Contrato.objetoContrato)
                .NotEmpty().WithMessage("Informe o objeto do contrato")
                .Length(3, 255).WithMessage("O objeto do contrato deve ter entre 3 e 255 caraceteres.");            

            //RuleFor(x => x.flagSituacao).NotEmpty().WithMessage("Esse campo é obrigatório. Escolha alguma situação.");

            //RuleFor(x => x.Contrato.flagRenovado)
            //    .NotEmpty().WithMessage("Informe se o contrato está sendo renovado.");

            RuleFor(x => x.Contrato.dtInicioVigencia)
                .NotEmpty().WithMessage("Informe a data de início da vigência.");

            RuleFor(x => x.Contrato.dtFimVigencia)
                .NotEmpty().WithMessage("Informe a data de término da vigência.");


            RuleFor(x => x.Contrato.flagOperacaoFinanceira)
                .NotEmpty().WithMessage("Informe a operação financeira.");

            RuleFor(x => x.Contrato.valorTotal)
                .NotEmpty().GreaterThan(0).WithMessage("Informe o valor total do contrato.");


           // When(x => (x.OArquivoContrato != null), () => {
				//RuleFor(x => x.OArquivoContrato)
					//.Must((x, OArquivoContrato) => UploadConfig.validarArquivo(OArquivoContrato)).WithMessage("Insira um arquivo válido.");
			//});

        }

        //
        private bool existe(ContratoForm ViewModel) {
            return OContratoBL.existe(ViewModel.Contrato.titulo, ViewModel.Contrato.id);
        }
    }
}