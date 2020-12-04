using System;
using BLL.AssociadosContribuicoes.Events;
using BLL.Core.Events;
using BLL.Services;
using System.Linq;
using DAL.AssociadosContribuicoes;
using EntityFramework.Extensions;

namespace BLL.AssociadosContribuicoes {

    public class AssociadoContribuicaoIsencaoBL : DefaultBL, IAssociadoContribuicaoIsencaoBL{

        //Atributos

        //Servicos

        //Propriedades

        //Events
        private readonly EventAggregator onContribuicaoIsenta = OnContribuicaoIsenta.getInstance;

        //Construtor
        public AssociadoContribuicaoIsencaoBL() {

        }

        //Desvincula de um associado a contribuição.
		public bool concederIsencao(int id, string observacoes, int idUsuarioIsencaoParam) {

            var OAssociadoContribuicao = db.AssociadoContribuicao.Find(id);

			if (OAssociadoContribuicao == null) {
				return false;
			}

		    OAssociadoContribuicao.motivoIsencao = observacoes;

            OAssociadoContribuicao.idUsuarioIsencao = idUsuarioIsencaoParam;

            OAssociadoContribuicao.dtIsencao = DateTime.Now;

		    db.AssociadoContribuicao
                .Where(x => x.id == id || x.idAssociadoContribuicaoPrincipal == id)
		        .Update(x => new AssociadoContribuicao{
		            flagIsento = true,
                    idUsuarioIsencao = idUsuarioIsencaoParam,
                    dtIsencao = OAssociadoContribuicao.dtIsencao,
                    motivoIsencao = OAssociadoContribuicao.motivoIsencao
		        });



		    onContribuicaoIsenta.subscribe(new OnContribuicaoIsentaHandler());

            onContribuicaoIsenta.publish( OAssociadoContribuicao as object);

            return true;
		}
    }
}