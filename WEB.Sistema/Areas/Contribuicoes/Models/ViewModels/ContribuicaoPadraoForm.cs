using System.Linq;
using FluentValidation.Attributes;
using DAL.Contribuicoes;

namespace WEB.Areas.Contribuicoes.ViewModels {

    [Validator(typeof(ContribuicaoPadraoFormValidator))]
    public class ContribuicaoPadraoForm {

        //Atributos
       

        //Propriedades
        public Contribuicao Contribuicao { get; set; }


        public ContribuicaoPadraoForm() {

            this.Contribuicao = new Contribuicao();
        }

        //
        public void carregarDadosContribuicao() {

            if(this.Contribuicao.id > 0) {

                this.Contribuicao.listaContribuicaoVencimento = this.Contribuicao.listaContribuicaoVencimento.Where(x => x.dtExclusao == null).ToList();

            }

        }
    }
}