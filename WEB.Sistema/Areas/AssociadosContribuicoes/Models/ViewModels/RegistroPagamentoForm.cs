using DAL.AssociadosContribuicoes;
using DAL.Financeiro;
using FluentValidation.Attributes;

namespace WEB.Areas.AssociadosContribuicoes.ViewModels {

    [Validator(typeof(RegistroPagamentoValidator))]
    public class RegistroPagamentoForm {

        //Atributos


        //Propriedades
        public AssociadoContribuicao AssociadoContribuicao { get; set; }

        public TituloReceita TituloReceita { get; set; }

        public TituloReceitaPagamento TituloReceitaPagamento { get; set; }


        //Construtor
        public RegistroPagamentoForm() {

            this.AssociadoContribuicao = new AssociadoContribuicao();

            this.TituloReceita = new TituloReceita();

            this.TituloReceitaPagamento = new TituloReceitaPagamento();
        }

    }

}