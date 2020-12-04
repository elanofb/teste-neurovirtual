using System;
using System.Linq;
using BLL.Services;
using DAL.Associados;

namespace BLL.Associados {

    public class AssociadoEmailVWBL : DefaultBL, IAssociadoEmailVWBL {

        //
        public IQueryable<AssociadoEmailVW> listar(int? idTipoEmail, string flagSituacao, string valorBusca, string ativo) {

            var query = from Ass in db.AssociadoEmailVW.AsNoTracking()
                        select Ass;

            query = query.condicoesSeguranca();

            if (idTipoEmail > 0) {
                query = query.Where(x => x.idTipoEmail == idTipoEmail);
            }

            if (!String.IsNullOrEmpty(flagSituacao)) {
                //query = query.Where(x => x.flagSituacaoContribuicao == flagSituacao);
            }
            
            if (!String.IsNullOrEmpty(valorBusca)) {

                string valorBuscaSoNumeros = UtilString.onlyNumber(valorBusca);
                int intValorBusca = UtilNumber.toInt32(valorBuscaSoNumeros);

                query = query.Where(x => x.id == intValorBusca ||
                                         x.nome.Contains(valorBusca) || x.email.Contains(valorBusca));

            }

            if (!String.IsNullOrEmpty(ativo)) {
                query = query.Where(x => x.ativo == ativo);
            }
            
            return query;
        }
    }
}