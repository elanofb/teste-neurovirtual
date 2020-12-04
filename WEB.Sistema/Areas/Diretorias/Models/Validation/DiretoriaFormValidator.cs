using FluentValidation;
using BLL.Diretorias;

namespace WEB.Areas.Diretorias.ViewModels {

    //
    public class DiretoriaFormValidator : AbstractValidator<DiretoriaForm> {

        //Atributos
        private DiretoriaBL _DiretoriaBL;

        //Propriedades
        private DiretoriaBL ODiretoriaBL => _DiretoriaBL = _DiretoriaBL ?? new DiretoriaBL();

        //
        public DiretoriaFormValidator() {

            RuleFor(x => x.Diretoria.anoInicioGestao)
                .GreaterThan(x => x.Diretoria.anoFimGestao)
                .WithMessage("O ano de inicio da gestão deverá ser menor que o ano do fim da gestão.");
        }

        //
        private bool existe(DiretoriaForm ViewModel) {

            return ODiretoriaBL.existe(ViewModel.Diretoria, ViewModel.Diretoria.id);

        }
    }
}