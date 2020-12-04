using System;
using System.Linq;
using BLL.Associados;
using BLL.Contribuicoes;
using BLL.Pessoas;
using BLL.Services;
using DAL.Associados;
using DAL.Pessoas;
using DAL.Relacionamentos;
using EntityFramework.Extensions;

namespace BLL.AssociadosContribuicoes {

    public class AssociadoAlteracaoBL: DefaultBL, IAssociadoAlteracaoBL {

        //Atributos
        private IAssociadoBL _AssociadoBL;
        private IPessoaRelacionamentoBL _PessoaRelacionamentoBL;
        private IContribuicaoBL _ContribuicaoBL;

        //Propriedades
        private IAssociadoBL OAssociadoBL => _AssociadoBL = _AssociadoBL ?? new AssociadoBL();
        private IPessoaRelacionamentoBL OPessoaRelacionamentoBL => _PessoaRelacionamentoBL = _PessoaRelacionamentoBL ?? new PessoaRelacionamentoBL();
        private IContribuicaoBL OContribuicaoBL => _ContribuicaoBL = _ContribuicaoBL ?? new ContribuicaoPadraoBL();

        //Events


        //Definir o vencimento da anuidade com base nas configurações
        public UtilRetorno alterarDados(int idAssociado, string informacao, object novoValor, int idUsuarioOperacao) {

            var OAssociado = this.OAssociadoBL.carregar(idAssociado);

            if (OAssociado == null) {
                return UtilRetorno.newInstance(false, "O associado informado não foi localizado.");
            }



            return UtilRetorno.newInstance(true, "A informação passada não possui tratamento.");
        }


    }
}