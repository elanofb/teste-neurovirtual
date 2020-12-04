using BLL.Contribuicoes;
using BLL.Associados;
using DAL.Contribuicoes;
using FluentValidation.Attributes;

namespace WEB.Areas.Contribuicoes.ViewModels {

    [Validator(typeof(ContribuicaoTabelaPrecoValidator))]
    public class ContribuicaoTabelaPrecoForm {

        //Atributos
        private IContribuicaoPrecoBL _ContribuicaoPrecoBL;
        private TipoAssociadoBL _TipoAssociadoBL;
        
        //
        private IContribuicaoPrecoBL OContribuicaoPrecoBL => (this._ContribuicaoPrecoBL = this._ContribuicaoPrecoBL ?? new ContribuicaoPrecoBL());
        private TipoAssociadoBL OTipoAssociadoBL => (this._TipoAssociadoBL = this._TipoAssociadoBL ?? new TipoAssociadoBL());

        //Propriedades
        public ContribuicaoTabelaPreco ContribuicaoTabelaPreco { get; set; }


        public ContribuicaoTabelaPrecoForm() {

            this.ContribuicaoTabelaPreco = new ContribuicaoTabelaPreco();
        }


    }
}