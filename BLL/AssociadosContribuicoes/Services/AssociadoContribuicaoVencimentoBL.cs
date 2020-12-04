using System;
using System.Linq;
using BLL.Associados;
using BLL.Services;
using DAL.Associados;
using DAL.Contribuicoes;

namespace BLL.AssociadosContribuicoes {

    public class AssociadoContribuicaoVencimentoBL : DefaultBL, IAssociadoContribuicaoVencimentoBL {

		//Atributos
        public IAssociadoBL _AssociadoBL; 
        public IAssociadoContribuicaoBL _AssociadoContribuicaoBL; 

		//Propriedades
        private IAssociadoBL OAssociadoBL => _AssociadoBL = _AssociadoBL ?? new AssociadoBL();
        private IAssociadoContribuicaoBL OAssociadoContribuicaoBL => _AssociadoContribuicaoBL = _AssociadoContribuicaoBL ?? new AssociadoContribuicaoBL(); 

        //Propriedades

        //Events

        //Construtor
        public AssociadoContribuicaoVencimentoBL() {

        }

        //Desvincula de um associado a contribuição.
		public ContribuicaoVencimento retornarProximoVencimento(Contribuicao Contribuicao, int idAssociado) {

		    if (Contribuicao.idTipoVencimento == TipoVencimentoConst.FIXO_PELA_CONTRIBUICAO) {

		        return this.retornarVencimentoFixo(Contribuicao);

		    }

            var OVencimento = this.retornarVencimentoAdmissao(Contribuicao, idAssociado);

            return OVencimento;
		}

        /// <summary>
        /// Calcular o vencimento da contribuicao com base na data de admissao do associado
        /// </summary>
        public ContribuicaoVencimento retornarVencimentoAdmissao(Contribuicao Contribuicao, int idAssociado) {

            var OVencimento = new ContribuicaoVencimento();

            var query = this.OAssociadoContribuicaoBL.listar(Contribuicao.id, idAssociado, null, null, "");

		    var dtUltimaContribuicao = query.OrderByDescending(x => x.dtVencimentoOriginal)
                                            .Select(x => x.dtVencimentoOriginal)
                                            .FirstOrDefault();

		    if (dtUltimaContribuicao != DateTime.MinValue) {

		        var dtProximoVencimento = dtUltimaContribuicao.AddMonths(Contribuicao.PeriodoContribuicao.qtdeMeses);

		        OVencimento.dtVencimento = dtProximoVencimento;

		        OVencimento.dtInicioVigencia = dtUltimaContribuicao.AddDays(1);

		        OVencimento.dtFimVigencia = dtProximoVencimento;

		        return OVencimento;
		    }

            var OAssociado = this.OAssociadoBL.carregar(UtilNumber.toInt32(idAssociado)) ?? new Associado();

            if (!OAssociado.dtAdmissao.HasValue) {

                return null;

            }

		    var dtVencimento = OAssociado.dtAdmissao.Value.AddMonths(Contribuicao.PeriodoContribuicao.qtdeMeses);
		    
		    OVencimento.dtVencimento = dtVencimento;

		    OVencimento.dtInicioVigencia = dtVencimento.AddDays(1);

		    OVencimento.dtFimVigencia = dtVencimento.AddMonths(Contribuicao.PeriodoContribuicao.qtdeMeses);

            return OVencimento;
        }

        //
        public ContribuicaoVencimento retornarVencimentoFixo(Contribuicao Contribuicao) {

            var OVencimento = Contribuicao.retornarProximoVencimento();

            return OVencimento;
        }
    }
}