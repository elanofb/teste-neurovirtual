using System;
using BLL.Publicacoes;
using FluentValidation;

namespace WEB.Areas.Publicacoes.ViewModels {

    //
    public class NoticiaFormValidation : AbstractValidator<NoticiaForm> {

        //Atributos
        private INoticiaBL _NoticiaBL;

        //Propriedades
        private INoticiaBL ONoticiaBL => (this._NoticiaBL = this._NoticiaBL ?? new NoticiaBL());

        //
        public NoticiaFormValidation() {
            RuleFor(x => x.Noticia.titulo)
                .NotEmpty().WithMessage("Informe o título");

            RuleFor(x => x.Noticia.titulo)
                .Must((x, titulo) => !this.existe(x))
                .WithMessage("Já existe um registro cadastrado com esse título.");

            RuleFor(x => x.Noticia.dtNoticia)
                .NotEmpty().WithMessage("Informe a data");

            RuleFor(x => x.Noticia.ativo)
                .NotEmpty().WithMessage("Informe o status");

            RuleFor(x => x.Noticia.descricao)
                .NotEmpty().WithMessage("Insira a descrição");
        }

        //Validação de duplicados
        public bool existe(NoticiaForm ViewModel) {

            int idDesconsiderado = UtilNumber.toInt32(ViewModel.Noticia.id);
            return this.ONoticiaBL.existeUrl(UtilString.cleanAccents(ViewModel.Noticia.titulo).ToLower().Replace(" ", "_"), idDesconsiderado, ViewModel.Noticia.idTipoNoticia);
        }
    }
}