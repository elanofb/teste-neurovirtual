using FluentValidation;

namespace WEB.Areas.Publicacoes.ViewModels {

    //
    public class VagaEstagioFormValidation : AbstractValidator<VagaEstagioForm> {
        //
        public VagaEstagioFormValidation() {
            RuleFor(x => x.Noticia.titulo).NotEmpty().WithMessage("Informe o título da vaga de estágio");
            RuleFor(x => x.Noticia.autor).NotEmpty().WithMessage("Informe o autor da vaga de estágio");
            RuleFor(x => x.Noticia.dtNoticia).NotEmpty().WithMessage("Informe a data de postagem da vaga de estágio");
            RuleFor(x => x.Noticia.ativo).NotEmpty().WithMessage("Informe o status");
            RuleFor(x => x.Noticia.descricao).NotEmpty().WithMessage("Insira a descrição da vaga de estágio");
        }
    }
}