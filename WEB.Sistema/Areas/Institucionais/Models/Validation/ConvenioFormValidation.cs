using System;
using BLL.Institucionais;
using FluentValidation;

namespace WEB.Areas.Institucionais.ViewModels {

    //
    public class ConvenioFormValidation : AbstractValidator<ConvenioForm> {

        //Atributos
        private IConvenioBL _ConvenioBL;

        //Propriedades
        private IConvenioBL OConvenioBL => this._ConvenioBL = this._ConvenioBL ?? new ConvenioBL();

        //
        public ConvenioFormValidation() {
            RuleFor(x => x.Convenio.titulo)
                .NotEmpty().WithMessage("Informe o título");

            RuleFor(x => x.Convenio.titulo)
                .Must((x, titulo) => !this.existe(x))
                .WithMessage("Já existe um registro cadastrado com esse título.");

            RuleFor(x => x.Convenio.ativo)
                .NotEmpty().WithMessage("Informe o status");

            RuleFor(x => x.Convenio.descricao)
                .NotEmpty().WithMessage("Insira a descrição");
        }

        //Validação de duplicados
        public bool existe(ConvenioForm ViewModel) {

            ViewModel.Convenio.titulo = ViewModel.Convenio.titulo ?? "";

            int idDesconsiderado = UtilNumber.toInt32(ViewModel.Convenio.id);

            return this.OConvenioBL.existe(ViewModel.Convenio.titulo, UtilString.cleanAccents(ViewModel.Convenio.titulo.ToLower().Replace(" ", "_")), idDesconsiderado);

        }
    }
}