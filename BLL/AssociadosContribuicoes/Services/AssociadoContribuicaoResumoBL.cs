using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Services;
using DAL.AssociadosContribuicoes;

namespace BLL.AssociadosContribuicoes {

    public class AssociadoContribuicaoResumoBL : DefaultBL, IAssociadoContribuicaoResumoBL {

        //Atributos

        //Propriedades

        //Events

        //
        public IQueryable<AssociadoContribuicaoResumoVW> listar(int idContribuicao, int idAssociado) {

            var query = from Fil in db.AssociadoContribuicaoResumoVW
                        select Fil;

            if (idContribuicao > 0) {

                query = query.Where(x => x.idContribuicao == idContribuicao);

            }

            if (idAssociado > 0) {

                query = query.Where(x => x.idAssociado == idAssociado);

            }

            query = query.condicoesSeguranca();

            return query;
        }


    }
}