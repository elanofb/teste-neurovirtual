using System;
using System.Data.Entity;
using System.Linq;
using BLL.AssociadosContribuicoes.Events;
using BLL.Core.Events;
using BLL.Services;
using DAL.AssociadosContribuicoes;
using EntityFramework.Extensions;

namespace BLL.AssociadosContribuicoes {

    public class AssociadoContribuicaoExclusaoBL : DefaultBL, IAssociadoContribuicaoExclusaoBL{

        //Atributos

        //Servicos

        //Propriedades

        //Events
        private EventAggregator onContribuicaoDesvinculada = OnContribuicaoDesvinculada.getInstance;

        //Construtor
        public AssociadoContribuicaoExclusaoBL() {

        }

        //Desvincula de um associado a contribuição.
		public virtual bool excluir(int id, string observacoes, int idUsuarioExclusaoParam) {

            var OAssociadoContribuicao = db.AssociadoContribuicao.condicoesSeguranca().AsNoTracking().FirstOrDefault(x => x.id == id);

			if (OAssociadoContribuicao == null) {
				return false;
			}

		    OAssociadoContribuicao.motivoExclusao = observacoes.abreviar(255);

		    OAssociadoContribuicao.dtExclusao = DateTime.Now;

		    db.AssociadoContribuicao.condicoesSeguranca()
                                    .Where(x => x.id == id || x.idAssociadoContribuicaoPrincipal == id)
		                            .Update(x => new AssociadoContribuicao
		                            {
                                        idUsuarioExclusao = idUsuarioExclusaoParam,

                                        dtExclusao = OAssociadoContribuicao.dtExclusao,

                                        motivoExclusao =  OAssociadoContribuicao.motivoExclusao
		                            });


            var flagExiste = db.AssociadoContribuicao.Any(x => x.id == id && x.dtExclusao == null);

		    if (!flagExiste) {

		        onContribuicaoDesvinculada.subscribe(new OnContribuicaoDesvinculadaHandler());

                onContribuicaoDesvinculada.publish( OAssociadoContribuicao as object);

		    }
            return true;
		}
    }
}