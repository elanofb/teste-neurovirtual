using System;
using System.Linq;
using BLL.Services;
using DAL.Relacionamentos;

namespace BLL.Relacionamentos {

    public class PessoaRelacionamentoVWBL : DefaultBL, IPessoaRelacionamentoVWBL {

        //
        public IQueryable<PessoaRelacionamentoVW> listar(string valorBusca) {

            var query = from PC in db.PessoaRelacionamentoVW.condicoesSeguranca()
                        select PC;

            if (!String.IsNullOrEmpty(valorBusca)) {
                query = query.Where(x => x.observacao.Contains(valorBusca) || x.descricaoTipoOcorrencia.Contains(valorBusca));
            }

            return query;
            
        }
        
    }
    
}