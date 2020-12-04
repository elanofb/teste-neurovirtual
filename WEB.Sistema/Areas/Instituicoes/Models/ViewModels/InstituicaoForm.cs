using FluentValidation.Attributes;
using DAL.Instituicoes;

namespace WEB.Areas.Instituicoes.ViewModels {

    [Validator(typeof(InstituicaoValidator))]
    public class InstituicaoForm {

        //Atributos

        //Propriedades
        public Instituicao Instituicao { get; set; }

        public InstituicaoForm() { 
            this.Instituicao = new Instituicao();
        }

    }

}