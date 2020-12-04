using DAL.Contribuicoes;
using BLL.Configuracoes;
using DAL.AssociadosContribuicoes;
using DAL.Associados;
using System;
using BLL.Contribuicoes;
using DAL.Configuracoes;

namespace BLL.AssociadosContribuicoes {

    public class AssociadoContribuicaoPadraoBL : IAssociadoContribuicaoBL {

        //Atributos
        private IContribuicaoAtualBL _IContribuicaoAtualBL;

        //Servicos
        private IContribuicaoAtualBL OContribuicaoAtualBL => this._IContribuicaoAtualBL = this._IContribuicaoAtualBL ?? new ContribuicaoAtualBL();

        //Propriedades
        private IAssociadoContribuicaoBL OAssociadoContribuicaoBL;

        private ConfiguracaoContribuicao OConfiguracaoContribuicao;

        //Events

        //Construtor
        public AssociadoContribuicaoPadraoBL() {

            this.OConfiguracaoContribuicao = ConfiguracaoContribuicaoBL.getInstance.carregar();

        }


    }
}