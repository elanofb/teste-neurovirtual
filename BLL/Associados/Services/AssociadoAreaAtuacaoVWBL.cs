using System;
using System.Collections.Generic;
using System.Linq;
using BLL.Services;
using DAL.Associados;

namespace BLL.Associados {

    public class AssociadoAreaAtuacaoVWBL : DefaultBL, IAssociadoAreaAtuacaoVWBL {
        
        //
        public IQueryable<AssociadoAreaAtuacaoVW> listar(List<int> idsAreaAtuacao, string flagSituacao, string valorBusca, string ativo) {

            var query = from Ass in db.AssociadoAreaAtuacaoVW.AsNoTracking()
                        select Ass;
            
            query = query.condicoesSeguranca();
            
            if (idsAreaAtuacao.Any()){
                query = query.Where(x => idsAreaAtuacao.Contains(x.idAreaAtuacao));
            }
            
            if (!String.IsNullOrEmpty(flagSituacao)) {
                query = query.Where(x => x.flagSituacaoContribuicao == flagSituacao);
            }
            
            if (!String.IsNullOrEmpty(valorBusca)) {

                string valorBuscaSoNumeros = UtilString.onlyNumber(valorBusca);
                int intValorBusca = UtilNumber.toInt32(valorBuscaSoNumeros);

                query = query.Where(x => x.id == intValorBusca || x.nome.Contains(valorBusca));

            }
            
            if (!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == ativo);
            }
            
            return query;
        }
    }
}